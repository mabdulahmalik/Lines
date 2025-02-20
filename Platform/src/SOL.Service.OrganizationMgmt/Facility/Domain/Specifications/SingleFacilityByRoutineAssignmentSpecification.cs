using SOL.Abstractions.Persistence;

namespace SOL.Service.OrganizationMgmt.Facility.Domain.Specifications;

public class SingleFacilityByRoutineAssignmentSpecification : AggregateQuerySpecification<Facility>
{
    public SingleFacilityByRoutineAssignmentSpecification(Guid routineAssignmentId)
        : base(x => x.Routines.Any(r => r.Id == routineAssignmentId))
    { }
}