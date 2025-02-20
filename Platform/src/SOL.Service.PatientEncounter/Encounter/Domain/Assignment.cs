using Dawn;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Domain;

public class Assignment : ValueType<Assignment>
{
    public Guid ClinicianId { get; }
    public EncounterAssigneePosition Position { get; }
    public Instant AssignedAt { get; }
    public Instant? WithdrawnAt { get; private set; }

    internal Assignment(Guid clinicianId, EncounterAssigneePosition position, Instant assignedAt)
    {
        ClinicianId = clinicianId;
        AssignedAt = assignedAt;
        Position = position;
    }

    internal Assignment(Guid clinicianId, EncounterAssigneePosition position) 
        : this(clinicianId, position, SystemClock.Instance.GetCurrentInstant())
    { }

    public void Withdraw()
    {
        Guard.Argument(WithdrawnAt).Invariant(arg => !arg.HasValue, _ => "Cannot withdraw an assignment that has already been withdrawn.");

        WithdrawnAt = SystemClock.Instance.GetCurrentInstant();
    }

    protected override bool EqualsInternal(Assignment other)
    {
        return ClinicianId.Equals(other.ClinicianId) &&
                Position.Equals(other.Position) &&
                AssignedAt.Equals(other.AssignedAt);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(ClinicianId, Position, AssignedAt);
    }
}