using Newtonsoft.Json;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Activities.Aggregates.Activities;
using SOL.Activities.Persistence;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;

namespace SOL.Activities.Interceptors;

public class ActivityInterceptor : ICommitInterceptor
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly Lazy<ActivitiesContext> _activitiesContext;
    private readonly IServiceBus _serviceBus;

    public ActivityInterceptor(IOperationContextFactory operationContextFactory
        , IDbContextFactory<ActivitiesContext> activityCtxFactory
        , IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
        _operationContext = new(operationContextFactory.Get);
        _activitiesContext = new(activityCtxFactory.CreateDbContext);
    }

    public async Task Commit(IEnumerable<AggregateRoot> aggregates, CancellationToken stoppageToken = default)
    {
        var activities = aggregates
            .Where(aggregate => aggregate.GetType().IsAssignableTo(typeof(ITrackableAggregate)))
            .SelectMany(aggregate => aggregate.Changes
                .Select(change => new
                {
                    Aggregate = aggregate,
                    Change = change,
                    TrackableActivityAttribute = change.Value.GetType().GetCustomAttribute<TrackableActivity>()
                })
                .Where(item => item.TrackableActivityAttribute != null)
                .Select(item => new Activity(Guid.NewGuid())
            {
                    AggregateType = item.Aggregate.GetType().Name,
                    ActivityType = item.TrackableActivityAttribute!.Name,
                    AggregateId = item.Aggregate.Id,
                    Metadata = JsonConvert.SerializeObject(item.Change.Value),
                    UserId = _operationContext.Value.ActorId,
                    Timestamp = item.Change.TimeStamp
            }))
            .OrderBy(activity => activity.Timestamp)
            .ToList();

         _activitiesContext.Value.Set<Activity>().AddRange(activities);
        await _activitiesContext.Value.SaveChangesAsync(stoppageToken);

        await _serviceBus.PublishAsync(new ActivitiesTracked {
            Activities = activities.Select(activity => new TrackedActivity {
                Id = activity.Id,
                ActivityType = activity.ActivityType,
                AggregateType = activity.AggregateType,
                AggregateId = activity.AggregateId,
                Metadata = activity.Metadata,
                UserId = activity.UserId,
                Timestamp = activity.Timestamp
            }).ToList()
        }, stoppageToken);
    }
}