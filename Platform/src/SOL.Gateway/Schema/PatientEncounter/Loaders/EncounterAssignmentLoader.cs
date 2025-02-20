using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAssignmentLoader : GroupedDataLoader<Guid, EncounterAssignmentView>
{
    private readonly IDomainQuery<EncounterAssignmentView> _getAssignments;

    public EncounterAssignmentLoader(IDomainQuery<EncounterAssignmentView> getAssignments, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    { 
        _getAssignments = getAssignments;
    }

    protected override async Task<ILookup<Guid, EncounterAssignmentView>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var assignments = await _getAssignments.Query
            .Where(x => keys.Contains(x.EncounterId))
            .OrderBy(x => x.AssignedAt)
            .ToListAsync(cancellationToken);

        return assignments.ToLookup(x => x.EncounterId);
    }
}