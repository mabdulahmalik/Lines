using HotChocolate.Data.Sorting;
using SOL.Gateway.Views.Activity;

namespace SOL.Gateway.Schema.Activities.Sorters;

public class ActivitiesSorterType : SortInputType<ActivityView>
{
    protected override void Configure(ISortInputTypeDescriptor<ActivityView> descriptor)
    {
        descriptor
            .Name("ActivitiesSorter")
            .Description("Sorting the Activity Query.");

        descriptor
            .Field(t => t.Timestamp)
            .Name("timestamp")
            .Description("The date and time the Activity was performed.");

        descriptor
            .Field(x => x.ActivityType)
            .Name("activityType")
            .Description("The Activity.");

        descriptor
            .Field(x => x.AggregateType)
            .Name("aggregateType")
            .Description("The Aggregate.");
    }
}