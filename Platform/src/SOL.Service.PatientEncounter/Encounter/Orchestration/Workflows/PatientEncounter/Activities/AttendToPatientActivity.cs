using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class AttendToPatientActivity(IAggregateRepository<Domain.Encounter> repository)
    : AdvanceTheEncounterActivityBase<AttendToPatient>(repository)
{
    public override async Task Execute(BehaviorContext<PatientEncounterState, AttendToPatient> context, IBehavior<PatientEncounterState, AttendToPatient> next)
    {
        await AdvanceTheEncounter(context.Message.EncounterId, context.CancellationToken);
        await next.Execute(context);
    }
}