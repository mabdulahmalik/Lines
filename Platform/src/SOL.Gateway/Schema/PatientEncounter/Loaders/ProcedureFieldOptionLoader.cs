using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Procedure.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldOptionLoader : GroupedDataLoader<Guid, ProcedureFieldOptionView>
{
    private readonly IDomainQuery<ProcedureFieldOptionView> _getFieldOptionQuery;

    public ProcedureFieldOptionLoader(IDomainQuery<ProcedureFieldOptionView> getFieldOptionQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getFieldOptionQuery = getFieldOptionQuery;
    }

    protected override async Task<ILookup<Guid, ProcedureFieldOptionView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var query = await _getFieldOptionQuery.Query
            .Where(x => keys.Contains(x.FieldId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return query.ToLookup(x => x.FieldId);
    }
}
