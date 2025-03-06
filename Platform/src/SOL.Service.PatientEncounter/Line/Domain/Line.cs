using Dawn;
using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Line.Domain;

public class Line : AggregateRoot, ITrackableAggregate
{
    private Tracked<Guid> _encounterIds = new();
    private Tracked<Guid> _facilityRoomIds = new();

    private Line(Guid id, string? name)
        : base(id)
    { 
        Name = name;
    }

    public string? Name { get; private set; }
    public string? Type { get; private set; }
    public Guid? FollowUpId { get; private set; }
    public Guid FacilityRoomId => _facilityRoomIds.Current.Value;
    public Guid? MedicalRecordId { get; private set; }    
    public bool ExternallyPlaced { get; private set; }
    public string? ExternallyPlacedBy { get; private set; }
    public LocalDate? InfectedOn { get; private set; }
    public Instant? InsertedOn { get; private set; }
    public Guid? InsertedWith { get; private set; }
    public Instant? RemovedOn { get; private set; }
    public Guid? RemovedWith { get; private set; }
    public IReadOnlyList<TimeStamped<Guid>> EncounterIds => _encounterIds;
    public LineDwelling Dwelling => !InsertedOn.HasValue
        ? LineDwelling.Undetermined
        : !RemovedOn.HasValue
            ? LineDwelling.In
            : LineDwelling.Out;

    internal static Line Create(Guid facilityRoomId, string? name = null, string? type = null, Instant? insertedOn = null
        , Guid? insertedWith = null, bool externallyPlaced = false, string? externallyPlacedBy = null, Guid? medicalRecordId = null)
    {
        Guard.Argument(facilityRoomId).NotDefault();

        var retVal = new Line(Guid.NewGuid(), name) {
            ExternallyPlaced = !String.IsNullOrWhiteSpace(externallyPlacedBy) || externallyPlaced,
            ExternallyPlacedBy = externallyPlacedBy,
            MedicalRecordId = medicalRecordId,
            InsertedWith = insertedWith,
            InsertedOn = insertedOn,
            Type = type
        };
        
        retVal._facilityRoomIds.Add(facilityRoomId);
        retVal.RaiseEventCreated();
        return retVal;
    }
    public void Modify(string? name, Guid facilityRoomId, Guid? medicalRecordId) {
        Guard.Argument(facilityRoomId).NotDefault("Invalid RoomId provided.");
        
        if (Name == null || !Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) {
            Name = name;
            RaiseEvent(new LineRenamed(this));
        }
        
        if (MedicalRecordId != medicalRecordId) {
            var oldMedicalRecordId = MedicalRecordId;

            MedicalRecordId = medicalRecordId; 
            RaiseEvent(new LineMedicalRecordModified(Id, MedicalRecordId, oldMedicalRecordId));
        }
        
        if (FacilityRoomId != facilityRoomId) {
            _facilityRoomIds.Add(facilityRoomId);
            RaiseEvent(new LineFacilityRoomChanged(Id, FacilityRoomId));
        }
    }
    
    public void FollowUp(Guid followUpJobId) {
        Guard.Argument(Dwelling).Invariant(arg => arg != LineDwelling.Out, a => "Cannot 'Follow Up' on a Line that has been removed already.");

        FollowUpId = followUpJobId;
        RaiseEventModified();
    }

    public void AttachEncounters(params Guid[] encounterIds) {
        Guard.Argument(encounterIds).NotEmpty(a => "Must provide a valid collection of Encounters.");
        Guard.Argument(Dwelling).Invariant(arg => arg != LineDwelling.Out, a => "Cannot Attach Encounters to a Line that has been pulled out.");

        var encIds = encounterIds.Except(_encounterIds.Select(x => x.Value));
        _encounterIds.AddRange(encIds);

        RaiseEvent(new LineEncounterAttached(Id, encIds));
    }

    public void DetachEncounters(params Guid[] encounterIds) {
        Guard.Argument(encounterIds).NotEmpty(a => "Must provide a valid collection of Jobs.");
        Guard.Argument(Dwelling).Invariant(arg => arg != LineDwelling.Out, a => "Cannot Detach Encounters from a Line that has been pulled out.");

        var encIds = encounterIds.Intersect(_encounterIds.Select(x => x.Value));
        _encounterIds.RemoveAll(x => encIds.Contains(x.Value));

        RaiseEvent(new LineEncounterDetached(Id, encIds));
    }
    
    public void Close(Instant removedOn, Guid? removedWith = null) {
        Guard.Argument(Dwelling).Invariant(arg => arg != LineDwelling.Out, a => "Cannot 'Close' a Line that has been removed already.");

        RemovedOn = removedOn;
        RemovedWith = removedWith;
        RaiseEvent(new LineRemoved(this));
    }

    public void RecordInfection(LocalDate infectedOn) {
        InfectedOn = infectedOn;
        RaiseEvent(new LineInfectionStatusChanged(Id, infectedOn));
    }

    public void ClearInfection() {
        InfectedOn = null;
        RaiseEvent(new LineInfectionStatusChanged(Id, null));
    }

    public void PlacedExternally(Instant placedOn, string? placedBy = null) {
        InsertedOn = placedOn;
        ExternallyPlaced = true;
        ExternallyPlacedBy = placedBy;
        RaiseEvent(new LinePlacementChanged(Id, true, Dwelling, placedBy, placedOn));
    }

    public void PlacedInternally() {
        InsertedOn = null;
        ExternallyPlaced = false;
        ExternallyPlacedBy = null;
        RaiseEvent(new LinePlacementChanged(Id, false, Dwelling));
    }
}