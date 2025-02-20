using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class CompleteEncounterActivity(IAggregateRepository<Domain.Encounter> repository)
    : AdvanceTheEncounterActivityBase<CompleteEncounter>(repository)
{
    public override async Task Execute(BehaviorContext<PatientEncounterState, CompleteEncounter> context, IBehavior<PatientEncounterState, CompleteEncounter> next)
    {
        await AdvanceTheEncounter(context.Message.EncounterId, context.CancellationToken);
        await next.Execute(context);
    }
}