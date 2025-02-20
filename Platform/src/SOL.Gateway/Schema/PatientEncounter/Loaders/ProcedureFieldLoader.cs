using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Procedure.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldLoader : GroupedDataLoader<Guid, ProcedureFieldView>
{
    private readonly IDomainQuery<ProcedureFieldView> _getProcedureFieldQuery;

    public ProcedureFieldLoader(IDomainQuery<ProcedureFieldView> getProcedureFieldQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getProcedureFieldQuery = getProcedureFieldQuery;
    }

    protected override async Task<ILookup<Guid, ProcedureFieldView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getProcedureFieldQuery.Query
            .Where(x => keys.Contains(x.ProcedureId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.ProcedureId);
    }
}