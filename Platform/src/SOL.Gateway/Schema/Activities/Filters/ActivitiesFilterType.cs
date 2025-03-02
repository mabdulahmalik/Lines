using HotChocolate.Data.Filters;
using SOL.Gateway.Views.Activity;

namespace SOL.Gateway.Schema.Activities.Filters;

public class ActivitiesFilterType : FilterInputType<ActivityView>
{
    protected override void Configure(IFilterInputTypeDescriptor<ActivityView> descriptor)
    {
        descriptor
            .Name("ActivitiesFilter")
            .Description("Filters the Activity.");

        descriptor
            .Field(x => x.AggregateType)
            .Name("aggregate")
            .Description("The Aggregate.");

        descriptor
            .Field(x => x.AggregateId)
            .Name("aggregateId")
            .Description("The unique identifier of the Aggregate.");

        descriptor
            .Field(x => x.ActivityType)
            .Name("activityType")
            .Description("The Activity.");

        descriptor
            .Field(x => x.UserId)
            .Name("userId")
            .Description("The unique identifier of the User who performed the Activity.");

        descriptor
            .Field(t => t.Timestamp)
            .Name("timestamp")
            .Description("The date and time the Activity was performed.");
    }
}