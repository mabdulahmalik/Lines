using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureValueLoader : GroupedDataLoader<Guid, EncounterProcedureValueView>
{
    private readonly IDomainQuery<EncounterProcedureValueView> _getEncounterProcedureValues;

    public EncounterProcedureValueLoader(IDomainQuery<EncounterProcedureValueView> getEncounterProcedureValues
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _getEncounterProcedureValues = getEncounterProcedureValues;
    }

    protected override async Task<ILookup<Guid, EncounterProcedureValueView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var procedureValues = await _getEncounterProcedureValues.Query
            .Where(x => keys.Contains(x.EncounterProcedureId))
            .ToListAsync(cancellationToken);

        return procedureValues.ToLookup(x => x.EncounterProcedureId);
    }
}