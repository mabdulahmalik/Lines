
using SOL.Abstractions.Messaging;

namespace SOL.Service.OrganizationMgmt.Facility.Domain;

public record FacilityRoutinesRemoved : IMessage
{
    public Guid FacilityId { get; }
    public IReadOnlyList<Guid> AssignmentIds { get; }

    internal FacilityRoutinesRemoved(Guid facilityId, IEnumerable<RoutineAssignment> assignments)
    {
        FacilityId = facilityId;
        AssignmentIds = assignments.Select(a => a.Id).ToList();
    }
}