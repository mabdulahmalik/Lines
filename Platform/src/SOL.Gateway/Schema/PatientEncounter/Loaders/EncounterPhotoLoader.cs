using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterPhotoLoader(
    IDbContextFactory<LinesDataStore> dbCtxFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<Guid, EncounterPhotoView>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, EncounterPhotoView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var dbCtx = await dbCtxFactory.CreateDbContextAsync(cancellationToken);
        
        var photos = await dbCtx.Set<EncounterPhotoView>()
            .Where(x => keys.Contains(x.EncounterId))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
        
        return photos.ToLookup(x => x.EncounterId);
    }
}