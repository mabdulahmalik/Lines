using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class SubmitProceduresActivity(IAggregateRepository<Domain.Encounter> repository, IAggregateReader<Job.Domain.Job> jobReader)
    : AdvanceTheEncounterActivityBase<SubmitProcedures>(repository)
{
    private readonly IAggregateReader<Job.Domain.Job> _jobReader = jobReader;

    public override async Task Execute(BehaviorContext<PatientEncounterState, SubmitProcedures> context
        , IBehavior<PatientEncounterState, SubmitProcedures> next)
    {
        var encounter = await Repository.Get(context.Message.EncounterId, context.CancellationToken);
        var job = await _jobReader.Get(encounter.JobId, context.CancellationToken);
        
        foreach (var activeRoutine in job.Routines.Where(x => x.RoutineAssignmentId.HasValue)) {
            context.Saga.RoutinesAssigned.Add(Tuple.Create(activeRoutine.EncounterProcedureId,
                activeRoutine.RoutineAssignmentId, false));
        }
        
        await AdvanceTheEncounter(context.Message.EncounterId, context.CancellationToken);
        await next.Execute(context);
    }
}