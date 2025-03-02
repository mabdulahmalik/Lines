using SOL.Gateway.Schema.Common;
using SOL.Gateway.Schema.Activities.Filters;
using SOL.Gateway.Schema.Activities.Objects;
using SOL.Gateway.Schema.Activities.Sorters;
using SOL.Gateway.Views.Activity;

namespace SOL.Gateway.Schema.Activities;

public class QueryActivitiesExtensions : ObjectTypeExtension<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("activities")
            .Description("Get all Activities.")
            .UseOffsetPaging<ActivityType>()
            .UseProjection()
            .UseFiltering<ActivitiesFilterType>()
            .UseSorting<ActivitiesSorterType>()
            .ResolveWith<QueryResolver<ActivityView>>(x => x.Results(default!));
    }
}