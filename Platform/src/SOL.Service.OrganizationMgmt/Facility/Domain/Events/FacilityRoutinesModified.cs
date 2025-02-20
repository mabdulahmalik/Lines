
using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Facility.Domain;

public record FacilityRoutinesModified : IMessage
{
    public Guid FacilityId { get; }
    public IReadOnlyList<RoutineAssignment> Assignments { get; }

    internal FacilityRoutinesModified(Guid facilityId, IEnumerable<RoutineAssignment> assignments)
    {
        FacilityId = facilityId;
        Assignments = assignments.ToList();
    }
}