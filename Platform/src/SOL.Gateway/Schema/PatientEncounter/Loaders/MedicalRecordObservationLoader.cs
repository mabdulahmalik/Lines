using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.MedicalRecord.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MedicalRecordObservationLoader : GroupedDataLoader<Guid, MedicalRecordObservationView>
{
    private readonly IDomainQuery<MedicalRecordObservationView> _getObservations;

    public MedicalRecordObservationLoader(IDomainQuery<MedicalRecordObservationView> getObservations
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _getObservations = getObservations;
    }

    protected override async Task<ILookup<Guid, MedicalRecordObservationView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var observations = await _getObservations.Query
            .Where(x => keys.Contains(x.MedicalRecordId))
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);

        return observations.ToLookup(x => x.MedicalRecordId);
    }
}