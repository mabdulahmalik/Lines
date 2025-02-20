using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureLoader : GroupedDataLoader<Guid, EncounterProcedureView>
{
    private readonly IDomainQuery<EncounterProcedureView> _getProcedures;

    public EncounterProcedureLoader(IDomainQuery<EncounterProcedureView> getProcedures, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getProcedures = getProcedures;
    }

    protected override async Task<ILookup<Guid, EncounterProcedureView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var procedures = await _getProcedures.Query
            .Where(x => keys.Contains(x.EncounterId))
            .OrderBy(x => x.PerformedAt)
            .ToListAsync(cancellationToken);

        return procedures.ToLookup(x => x.EncounterId);
    }
}