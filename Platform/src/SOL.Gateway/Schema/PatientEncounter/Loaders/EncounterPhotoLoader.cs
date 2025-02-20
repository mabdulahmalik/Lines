using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterPhotoLoader : GroupedDataLoader<Guid, EncounterPhotoView>
{
    private readonly IDomainQuery<EncounterPhotoView> _getPhotos;

    public EncounterPhotoLoader(IDomainQuery<EncounterPhotoView> getPhotos, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getPhotos = getPhotos;
    }

    protected override async Task<ILookup<Guid, EncounterPhotoView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var photos = await _getPhotos.Query
            .Where(x => keys.Contains(x.EncounterId))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
        
        return photos.ToLookup(x => x.EncounterId);
    }
}