using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProgressLoader : GroupedDataLoader<Guid, EncounterProgressStage>
{
    private readonly IDomainQuery<EncounterProgressView> _getProgress;

    public EncounterProgressLoader(IDomainQuery<EncounterProgressView> getProgress
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _getProgress = getProgress;
    }

    protected override async Task<ILookup<Guid, EncounterProgressStage>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var progress = await _getProgress.Query
            .Where(x => keys.Contains(x.EncounterId))
            .ToListAsync(cancellationToken);

        var stages = progress.SelectMany(view =>
        {
            var stgs = new List<EncounterProgressStage>();
            
            if (view.WaitingDuration.HasValue && view.WaitingDuration >= 0)
                stgs.Add(new EncounterProgressStage { Stage = EncounterStage.Waiting, Timestamp = view.Waiting, Duration = view.WaitingDuration, EncounterId = view.EncounterId });
            else
                stgs.Add(new EncounterProgressStage { Stage = EncounterStage.Waiting, Timestamp = view.Waiting, EncounterId = view.EncounterId });

            if (view.Assigned.HasValue && view.AssignedDuration >= 0) {
                stgs.Add(new EncounterProgressStage {
                    Stage = EncounterStage.Assigned, Timestamp = view.Assigned.Value, Duration = view.AssignedDuration, EncounterId = view.EncounterId
                });
            } 
            else if(view.Assigned.HasValue) {
                stgs.Add(new EncounterProgressStage { Stage = EncounterStage.Assigned, Timestamp = view.Assigned.Value, EncounterId = view.EncounterId });
                return stgs;
            }

            if (view.Attending.HasValue && view.AttendingDuration >= 0) {
                stgs.Add(new EncounterProgressStage {
                    Stage = EncounterStage.Attending, Timestamp = view.Attending.Value, Duration = view.AttendingDuration, EncounterId = view.EncounterId
                });
            } 
            else if(view.Attending.HasValue) {
                stgs.Add(new EncounterProgressStage { Stage = EncounterStage.Attending, Timestamp = view.Attending.Value, EncounterId = view.EncounterId });
                return stgs;
            }

            if (view.Charting.HasValue && view.ChartingDuration >= 0) {
                stgs.Add(new EncounterProgressStage {
                    Stage = EncounterStage.Charting, Timestamp = view.Charting.Value, Duration = view.ChartingDuration, EncounterId = view.EncounterId
                });
            } 
            else if(view.Charting.HasValue) {
                stgs.Add(new EncounterProgressStage { Stage = EncounterStage.Charting, Timestamp = view.Charting.Value, EncounterId = view.EncounterId });
                return stgs;
            }
            
            if (view.Completed.HasValue) {
                stgs.Add(new EncounterProgressStage {
                    Stage = EncounterStage.Completed, Timestamp = view.Completed.Value, EncounterId = view.EncounterId
                });
            }            
            
            return stgs;
        });
        
        return stages
            .OrderBy(x => x.Timestamp)
            .ToLookup(x => x.EncounterId);
    }
}

public class EncounterProgressStage
{
    public Guid EncounterId { get; set; }
    public EncounterStage Stage { get; set; }
    public DateTime Timestamp { get; set; }
    public int? Duration { get; set; }
}