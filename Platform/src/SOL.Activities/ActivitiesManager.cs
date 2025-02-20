using Microsoft.EntityFrameworkCore;
using SOL.Activities.Aggregates.Activities;
using SOL.Activities.Persistence;

namespace SOL.Activities
{
    public class ActivitiesManager
    {
        private readonly ActivitiesContext _context;

        public ActivitiesManager( ActivitiesContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Activity>> List(Guid aggregateId, string activity, CancellationToken stoppageToken)
        {
            return await _context.Set<Activity>()
                .Where(x=> x.AggregateId == aggregateId && x.ActivityType == activity)
                .OrderByDescending(x=> x.Timestamp)
                .ToListAsync(stoppageToken);
        }

        public async Task Update(Activity activity, CancellationToken stoppageToken)
        {
            _context.Set<Activity>().Update(activity);
            await _context.SaveChangesAsync(stoppageToken);
        }

        public async Task Remove(Guid activityId, CancellationToken stoppageToken)
        {
            var activity = await _context.Set<Activity>().SingleAsync(x => x.Id == activityId);

            _context.Set<Activity>().Remove(activity);
            await _context.SaveChangesAsync(stoppageToken);
        }
    }
}
