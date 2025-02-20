using SOL.Abstractions.Persistence;

namespace SOL.Service.OrganizationMgmt.Facility.Domain.Specifications;

public class FacilitiesInTimeZoneSpecification : AggregateQuerySpecification<Facility>
{
    public FacilitiesInTimeZoneSpecification(string timeZone)
        : base(f => f.TimeZone == timeZone)
    { }
}