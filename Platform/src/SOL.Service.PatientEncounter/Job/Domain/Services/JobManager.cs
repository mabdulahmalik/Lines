using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Job.Domain.Services;

public class JobManager
{
    private readonly Lazy<PatientEncounterDataStore> _dataStore;

    public JobManager(IDbContextFactory<PatientEncounterDataStore> dbCtxFactory)
    {
        _dataStore = new(dbCtxFactory.CreateDbContext);
    }

    public async Task<IReadOnlyList<ScheduledJob>> GetScheduledJobs(DateOnly scheduledDate, IEnumerable<Guid> facilityIds
        , CancellationToken cancellationToken = default)
    {
        await using var dbCtx = _dataStore.Value;

        return await dbCtx.Set<ScheduledJob>()
            .Where(x => x.Status == JobStatus.Scheduled
                        && x.ScheduledDate == scheduledDate
                        && facilityIds.Contains(x.FacilityId))
            .OrderBy(x => x.FacilityId)
                .ThenBy(x => x.Room)
            .ToListAsync(cancellationToken);
    }
}