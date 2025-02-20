using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAlertLoader : GroupedDataLoader<Guid, EncounterAlertView>
{
    private readonly IDomainQuery<EncounterAlertView> _getAlerts;

    public EncounterAlertLoader(IDomainQuery<EncounterAlertView> getAlerts
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _getAlerts = getAlerts;
    }

    protected override async Task<ILookup<Guid, EncounterAlertView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var alerts = await _getAlerts.Query
            .Where(x => keys.Contains(x.EncounterId))
            .OrderByDescending(x => x.AlertedAt)
            .ToListAsync(cancellationToken);

        return alerts.ToLookup(x => x.EncounterId);
    }
}