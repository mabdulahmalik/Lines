using Dawn;
using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Job.Domain;

public class Job : AggregateRoot, IRestorable, ITrackableAggregate
{
    private List<ActiveRoutine> _routines = new();
    private Tracked<JobStatus> _status = new();
    private List<Note> _notes = new();

    private Job(Guid id, Guid purposeId) 
        : base(id)
    { 
        PurposeId = purposeId;
    }

    public Guid PurposeId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid? LineId { get; private set; }
    public Guid? MedicalRecordId { get; private set; }
    public string? Contact { get; private set; }
    public string? OrderingProvider { get; private set; }    
    public LocalDate? ScheduledDate { get; private set; }
    public LocalTime? ScheduledTime { get; private set; }
    public Instant? DeletedAt { get; private set; }
    public Instant StatusChangedAt => _status.Current.TimeStamp;
    public JobStatus Status => _status.Current.Value;
    public IReadOnlyList<Note> Notes => _notes;
    public IReadOnlyList<ActiveRoutine> Routines => _routines;

    internal static Job Create(Guid purposeId, Guid roomId, Guid? lineId = null
        , Guid? medicalRecordId = null, string? contact = null, string? orderingProvider = null
        , LocalDate? scheduledDate = null, LocalTime? scheduledTime = null
        , IEnumerable<ActiveRoutine>? routines = null)
    {
        Guard.Argument(roomId).NotDefault("Invalid RoomId provided.");
        Guard.Argument(purposeId).NotDefault("Invalid PurposeId provided.");
        Guard.Argument(scheduledTime).Invariant(arg => !arg.HasValue || scheduledDate.HasValue, a => "ScheduledTime requires a ScheduledDate.");

        var retVal = new Job(Guid.NewGuid(), purposeId) {
            RoomId = roomId,
            Contact = contact,
            LineId = lineId,
            OrderingProvider = orderingProvider,
            MedicalRecordId = medicalRecordId,
            ScheduledDate = scheduledDate,
            ScheduledTime = scheduledTime
        };

        var today = LocalDate.FromDateTime(DateTime.UtcNow);
        retVal._status.Add(scheduledDate.GetValueOrDefault(today) > today 
            ? JobStatus.Scheduled 
            : JobStatus.Underway);

        if (routines != null) {
            retVal._routines.AddRange(routines);
        }

        retVal.RaiseEventCreated();
        return retVal;
    }
    
    public void Modify(Guid roomId, Guid? lineId, Guid? medicalRecordId
        , string? orderingProvider, string? contact) {
        Guard.Argument(roomId).NotDefault("Invalid RoomId provided.");
        
        RoomId = roomId;
        LineId = lineId;
        MedicalRecordId = medicalRecordId;
        OrderingProvider = orderingProvider;
        Contact = contact;
        
        RaiseEventModified();
    }
    
    public void Schedule(LocalDate scheduledDate, LocalTime? scheduledTime = null, string? reason = null, Guid? scheduledBy = null) {
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Completed, a => "Cannot Schedule a Job that has already completed.");
        Guard.Argument(scheduledDate).Invariant(arg => arg.ToDateTimeUnspecified() > DateTime.Today, a => "Scheduling a Job requires a future date.");
        
        ScheduledDate = scheduledDate;
        ScheduledTime = scheduledTime;
        _status.Add(JobStatus.Scheduled);

        RaiseEvent(new JobScheduled(this, scheduledDate, scheduledTime));

        if (!String.IsNullOrWhiteSpace(reason) && scheduledBy.HasValue) {
            AddNotes(new Note(scheduledBy.Value, reason));
        }
    }
    
    public void Start() {
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Completed, a => "Cannot 'Start' a Job that has already completed.");
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Canceled, a => "Cannot 'Start' a Job that has been canceled.");
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Underway, a => "Cannot 'Start' a Job that is already underway.");
        
        setStatus(JobStatus.Underway);
    }

    public void Complete() {
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Completed, a => "Cannot 'Complete' a Job that has already completed.");

        setStatus(JobStatus.Completed);
        RaiseEventModified();
    }

    public void Cancel(Guid? canceledBy, string? reason) {
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Completed, a => "Cannot 'Cancel' a Job that has already completed.");
        Guard.Argument(Status).Invariant(arg => arg != JobStatus.Canceled, a => "Cannot 'Cancel' a Job that has already canceled.");

        setStatus(JobStatus.Canceled);
        
        if (canceledBy.HasValue && !string.IsNullOrWhiteSpace(reason)) {
            AddNotes(new Note(canceledBy.Value, reason));
        }
    }
    
    public void AddNotes(params Note[] notes) {
        _notes.AddRange(notes);
        RaiseEvent(new JobNotesAdded(this, notes));
    }

    public void ModifyNote(Guid noteId, string text) {
        var note = _notes.Single(x => x.Id == noteId);
        note.SetText(text);
        
        RaiseEvent(new JobNotesModified(this, new List<Note> { note }));
    }

    public void RemoveNotes(params Guid[] noteIds) {
        var removedNotes = _notes.FindAll(n => noteIds.Contains(n.Id));
        _notes.RemoveAll(n => noteIds.Contains(n.Id));
        
        RaiseEvent(new JobNotesRemoved(this, removedNotes));
    }
    
    public void PinNote(Note note) {
        Guard.Argument(note).NotNull();
        
        _notes.Where(x => x.Treatment == JobNoteTreatment.Pinned).ToList()
            .ForEach(UnpinNote);
        
        note.Pin();
        
        if (!_notes.Contains(note)) {
            AddNotes(note);
        } else {
            RaiseEvent(new JobNotesModified(this, new List<Note> { note }));
        }
    }
    
    public void UnpinNote(Note note) {
        Guard.Argument(note).NotNull();
        
        note.Unpin();
        
        RaiseEvent(new JobNotesModified(this, new List<Note> { note }));
    }

    private void setStatus(JobStatus status) {
        _status.Add(status);
        RaiseEvent(new JobStatusChanged(this));
    }
}