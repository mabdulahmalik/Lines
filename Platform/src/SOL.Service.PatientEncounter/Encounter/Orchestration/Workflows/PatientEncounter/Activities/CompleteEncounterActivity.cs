using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class CompleteEncounterActivity(IAggregateRepository<Domain.Encounter> repository, IAggregateReader<Purpose.Domain.Purpose> purposeReader, IOperationContextFactory operationContextFactory)
    : AdvanceTheEncounterActivityBase<CompleteEncounter>(repository)
{
    private readonly Lazy<IOperationContext> _operationContext = new(operationContextFactory.Get);
    
    public override async Task Execute(BehaviorContext<PatientEncounterState, CompleteEncounter> context, IBehavior<PatientEncounterState, CompleteEncounter> next)
    {
        await AdvanceTheEncounter(context.Message.EncounterId, context.CancellationToken);
        var encounter = await Repository.Get(context.Message.EncounterId, context.CancellationToken);
        var purpose = await purposeReader.Get(encounter.PurposeId!.Value, context.CancellationToken);
        
        var encounterCompletedEvt = new EncounterCompleted
        {
            Id = context.Message.EncounterId,
            Purpose = purpose.Name,
            CompletedBy = _operationContext.Value.ActorId,
            Attendees = encounter.Assignments.Select(x => new PatientAttendee
            {
                ClinicianId = x.ClinicianId,
                Position = x.Position
            })
        };
        
        await context.Publish(encounterCompletedEvt, context.CancellationToken);
        await next.Execute(context);
    }
}