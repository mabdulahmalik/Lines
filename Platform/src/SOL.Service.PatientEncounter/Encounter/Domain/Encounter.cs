using Dawn;
using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public class Encounter : AggregateRoot, ITrackableAggregate
{
    private Tracked<EncounterPriority> _priority = new();
    private Tracked<EncounterStage> _stage = new();
    private List<Assignment> _assignments = new();
    private List<Procedure> _procedures = new();
    private List<Photo> _photos = new();
    private List<Alert> _alerts = new();

    private Encounter(Guid id, Guid jobId) 
        : base(id)
    { 
        JobId = jobId;
    }

    public Guid JobId { get; private set; }
    public Guid? PurposeId { get; private set; }
    public Guid? MedicalRecordId { get; private set; }
    public Guid FacilityRoomId  { get; private set; }
    public EncounterPriority Priority => _priority.Current.Value;
    public EncounterStage Stage => _stage.Current.Value;
    public IReadOnlyList<Assignment> Assignments => _assignments.Where(x => !x.WithdrawnAt.HasValue).ToList();
    public IReadOnlyList<Procedure> Procedures => _procedures;
    public IReadOnlyList<Photo> Photos => _photos;
    public IReadOnlyList<Alert> Alerts => _alerts;

    internal static Encounter Create(Guid jobId, Guid facilityRoomId, Guid purposeId, Guid? medicalRecordId = null) {    
        Guard.Argument(jobId).NotDefault("Cannot create 'Encounter'. Invalid JobId provided.");
        Guard.Argument(purposeId).NotDefault("Cannot create 'Encounter'. Invalid PurposeId provided.");
        Guard.Argument(facilityRoomId).NotDefault("Cannot create 'Encounter'. Invalid RoomId provided.");

        var retVal = new Encounter(Guid.NewGuid(), jobId) {
            MedicalRecordId = medicalRecordId,
            FacilityRoomId = facilityRoomId,
            PurposeId = purposeId
        };

        retVal._priority.Add(EncounterPriority.Normal);
        retVal._stage.Add(EncounterStage.Waiting);

        retVal.RaiseEventCreated();
        return retVal;
    }

    public void Modify(Guid facilityRoomId, Guid? medicalRecordId) {
        Guard.Argument(facilityRoomId).NotDefault($"Cannot modify 'Encounter' [{Id}]. Invalid RoomId provided.");
        
        var roomChanged = FacilityRoomId != facilityRoomId;
        var medicalRecordChanged = MedicalRecordId != medicalRecordId;
        
        if (roomChanged) {
            FacilityRoomId = facilityRoomId;
            RaiseEvent(new EncounterRoomChanged(this));
            RaiseEventModified();
        }
        
        if (medicalRecordChanged) {
            MedicalRecordId = medicalRecordId;
            RaiseEvent(new EncounterMedicalRecordChanged(this));
            RaiseEventModified();
        }
    }
    
    public void Advance() {
        Guard.Argument(Stage).Invariant(arg => arg != EncounterStage.Completed, a => $"Cannot Advance this 'Encounter' [{Id}], it has already completed.");
        Guard.Argument(_assignments).Invariant(arg => arg.Any(), a => $"Cannot Advance this 'Encounter' [{Id}], it does not have any Assignees.");
        Guard.Argument(_procedures).Invariant(arg => arg.Any() || Stage < EncounterStage.Attending, _ => $"Cannot Advance this 'Encounter' [{Id}], it does not have any Procedures.");
        
        var nextStage = Stage switch {
            EncounterStage.Waiting => EncounterStage.Assigned,
            EncounterStage.Assigned => EncounterStage.Attending,
            EncounterStage.Attending => EncounterStage.Charting,
            EncounterStage.Charting => EncounterStage.Completed,
            _ => throw new ArgumentOutOfRangeException()
        };

        SetStage(nextStage);
    }    
    
    public void Assign(params Assignment[] assignments) {
        Guard.Argument(assignments).NotEmpty(a => $"Cannot Assign Clinicians to this 'Encounter' [{Id}]. Must provide a valid collection of Assignments.");
        Guard.Operation(assignments.All(a => a.ClinicianId != default), $"Cannot Assign Clinicians to this 'Encounter' [{Id}]. One or more Assignments need to specify a Clinician.");
        Guard.Argument(Stage).Invariant(arg => arg <= EncounterStage.Attending, a => $"Cannot Assign Clinicians to an 'Encounter' [{Id}] that has already been attended to.");

        var activeAssignments = _assignments.FindAll(a => !a.WithdrawnAt.HasValue);
        var existingAssignments = assignments
            .Where(a => activeAssignments.Any(x => x.ClinicianId == a.ClinicianId))
            .Select(x => x.ClinicianId)
            .ToList();

        Guard.Argument(existingAssignments).Invariant(arg => !arg.Any(), _ =>
            $"Cannot Assign the following Clinician(s) to this 'Encounter' [{Id}] as they are already assigned: {String.Join(", ", existingAssignments)}.");
        
        _assignments.AddRange(assignments);
        
        RaiseEvent(new CliniciansAssigned(this, assignments));
        
        if(Stage == EncounterStage.Waiting) {
            SetStage(EncounterStage.Assigned);
        }
        
        if (_alerts.Any(x => x.Type == EncounterAlertType.AssistanceRequested) 
            && assignments.Any(x => x.Position != EncounterAssigneePosition.Trainee)) {
            CancelAssistance();
        }
    }

    public void Withdraw(params Guid[] clinicianIds) {
        Guard.Argument(clinicianIds).NotEmpty(a => $"Cannot Withdraw Clinicians from this 'Encounter' [{Id}]. Must provide a valid collection of Clinician Ids.");
        Guard.Argument(Stage).Invariant(arg => arg <= EncounterStage.Attending, a => $"Cannot Withdraw Clinicians from an 'Encounter' [{Id}] that has already been attended to.");

        var removedAssignments = _assignments.FindAll(a => clinicianIds.Contains(a.ClinicianId) && !a.WithdrawnAt.HasValue);
        removedAssignments.ForEach(a => a.Withdraw());
        
        RaiseEvent(new CliniciansWithdrawn(this, removedAssignments));

        if (_assignments.All(x => x.WithdrawnAt.HasValue)) {
            SetStage(EncounterStage.Waiting);
        } else if (!_assignments.Any(x => !x.WithdrawnAt.HasValue && x.Position == EncounterAssigneePosition.Primary)) {
            SetStage(EncounterStage.Assigned);
        }
        
        var alertedBy = _alerts.SingleOrDefault(x => x.Type == EncounterAlertType.AssistanceRequested)?.AlertedBy;
        if (alertedBy.HasValue && removedAssignments.Select(x => x.ClinicianId).Contains(alertedBy.Value)) {
            CancelAssistance();
        }
    }

    public void AttachPhotos(params Photo[] photos) {
        Guard.Argument(photos).NotEmpty(a => $"Cannot Attach Photos to 'Encounter' [{Id}]. Must provide a valid collection of Photos.");
        Guard.Operation(photos.All(a => a.Id != default && !String.IsNullOrWhiteSpace(a.FileName)), $"Cannot Attach Photos to 'Encounter' [{Id}]. One or more Photos are missing required data.");

        _photos.AddRange(photos);
        
        RaiseEvent(new EncounterPhotosAttached(this, photos));
    }

    public void DetachPhotos(params Guid[] photoIds) {
        Guard.Argument(photoIds).NotEmpty(a => $"Cannot detach Photos from 'Encounter' [{Id}]. Must provide a valid collection of Photo Ids.");

        var detachedPhotos = _photos.FindAll(a => photoIds.Contains(a.Id));
        _photos.RemoveAll(a => photoIds.Contains(a.Id));
        
        RaiseEvent(new EncounterPhotosDetached(this, detachedPhotos));
    }

    public void ApplyProcedures(params Procedure[] procedures) {
        Guard.Argument(procedures).NotEmpty(a => $"Cannot Apply Procedures to 'Encounter' [{Id}]. Must provide a valid collection of Procedures.");
        Guard.Operation(procedures.All(a => a.Id != default), $"Cannot Apply Procedures to 'Encounter' [{Id}]. One or more Procedures are missing required data.");
        Guard.Argument(Stage).Invariant(arg => arg == EncounterStage.Attending, a => $"Cannot Apply Procedures to 'Encounter' [{Id}]. Cannot Apply Procedures to an 'Encounter' that is no longer being attended to.");

        _procedures.AddRange(procedures);
        
        RaiseEvent(new EncounterProceduresApplied(this, procedures));
    }
    
    public void ModifyProcedure(Guid encounterProcedureId, params ProcedureValue[] values) {
        Guard.Argument(encounterProcedureId).NotDefault($"Cannot Modify Procedure for 'Encounter' [{Id}]. Must provide a valid EncounterProcedureId.");
        Guard.Argument(_procedures).Require(arg => arg.Any(x => x.Id == encounterProcedureId), _ => $"Cannot Modify Procedure for 'Encounter' [{Id}]. Invalid Procedure Id provided.");
        Guard.Argument(values).NotEmpty(a => $"Cannot Modify Procedure [{encounterProcedureId}] to 'Encounter' [{Id}]. Must provide a valid collection of Procedure Values.");
        Guard.Operation(values.All(a => a.FieldId != default && a.Value != default), $"Cannot Modify Procedure [{encounterProcedureId}] to 'Encounter' [{Id}]. One or more Procedure Values are missing required data.");
        
        var procedure = _procedures.Single(x => x.Id == encounterProcedureId);
        procedure.SetValues(values);
        
        RaiseEvent(new EncounterProcedureModified(this, procedure));
    }

    public void UndoProcedures(params Guid[] encounterProcedureIds) {
        Guard.Argument(encounterProcedureIds).NotEmpty(a => $"Cannot Undo Procedures for 'Encounter' [{Id}]. Must provide a valid collection of Procedure Ids.");

        var undoneProcedures = _procedures.FindAll(p => encounterProcedureIds.Contains(p.Id));
        _procedures.RemoveAll(p => encounterProcedureIds.Contains(p.Id));
        
        if (undoneProcedures.Any())
            RaiseEvent(new EncounterProceduresUndone(this, undoneProcedures));
    }

    public void PlaceOnHold(Guid userId, string? reason = null) {
        Guard.Argument(Stage).Invariant(arg => arg <= EncounterStage.Attending, a => $"Cannot 'Place on Hold' an Encounter [{Id}] that has already been attended to.");
        Guard.Argument(_alerts).Invariant(arg => arg.All(x => x.Type != EncounterAlertType.OnHold), a => $"Cannot 'Place on Hold' an Encounter [{Id}] that is already on Hold.");

        _alerts.Add(new Alert(EncounterAlertType.OnHold, userId, reason));
        RaiseEvent(new EncounterAlertedAdded(Id, JobId, _alerts.Last()));
    }

    public void RemoveHold() {
        Guard.Argument(_alerts).Invariant(arg => arg.Any(x => x.Type == EncounterAlertType.OnHold), a => $"Cannot Remove a Hold from this Encounter [{Id}] because it is not currently on Hold.");
        
        _alerts.RemoveAll(x => x.Type == EncounterAlertType.OnHold);
        RaiseEvent(new EncounterAlertedRemoved(Id, EncounterAlertType.OnHold));
    }

    public void RequestAssistance(Guid userId, string? reason = null) {
        Guard.Argument(Stage).Invariant(arg => arg <= EncounterStage.Attending, a => $"Cannot Request Assistance for this Encounter [{Id}], it has already been attended to.");
        Guard.Argument(_alerts).Invariant(arg => arg.All(x => x.Type != EncounterAlertType.AssistanceRequested), a => $"Cannot Request Assistance for this Encounter [{Id}], it has already requested assistance.");

        _alerts.Add(new Alert(EncounterAlertType.AssistanceRequested, userId, reason));
        RaiseEvent(new EncounterAlertedAdded(Id, JobId, _alerts.Last()));
    }
    
    public void CancelAssistance() {
        Guard.Argument(_alerts).Invariant(arg => arg.Any(x => x.Type == EncounterAlertType.AssistanceRequested), a => $"Cannot Cancel Assistance request for this Encounter [{Id}] because it has not requested assistance.");
        
        _alerts.RemoveAll(x => x.Type == EncounterAlertType.AssistanceRequested);
        RaiseEvent(new EncounterAlertedRemoved(Id, EncounterAlertType.AssistanceRequested));
    }

    public void Escalate(string? reason = null) {
        Guard.Argument(Stage).Invariant(arg => arg <= EncounterStage.Attending, a => $"Cannot Escalate the Priority of this Encounter [{Id}], it has already been attended to.");

        SetPriority(EncounterPriority.Stat, reason);
    }
    
    public void Deescalate() {
        Guard.Argument(Stage).Invariant(arg => arg <= EncounterStage.Attending, a => $"Cannot Deescalate the Priority of this Encounter [{Id}], it has already been attended to.");
        
        var previousPriority = _priority.Where(x => x.Value != EncounterPriority.Stat)
            .OrderByDescending(x => x.TimeStamp).Select(x => x.Value)
            .FirstOrDefault();

        SetPriority(previousPriority != default ? previousPriority : EncounterPriority.Normal);
    }

    private void SetStage(EncounterStage stage, object? args = null) {
        if (stage == Stage) 
            return;
        
        var rightNow = SystemClock.Instance.GetCurrentInstant();
        var duration = rightNow.Minus(_stage.Current.TimeStamp);
        var elapsedTime = (int)Math.Round(duration.TotalMinutes, 0);

        _stage.Add(stage);
        RaiseEvent(new EncounterProgressed(this, elapsedTime, args));
    }

    private void SetPriority(EncounterPriority priority, object? args = null) {
        if (priority == Priority) 
            return;
        
        _priority.Add(priority);
        RaiseEvent(new EncounterPriorityChanged(this, args));
    }
}