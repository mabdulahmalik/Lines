using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.MedicalRecord;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MedicalRecordObservationLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, MedicalRecordObservationView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, MedicalRecordObservationView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var observations = await dbCtx.Set<MedicalRecordObservationView>()
            .Where(x => keys.Contains(x.MedicalRecordId))
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);

        return observations.ToLookup(x => x.MedicalRecordId);
    }
}