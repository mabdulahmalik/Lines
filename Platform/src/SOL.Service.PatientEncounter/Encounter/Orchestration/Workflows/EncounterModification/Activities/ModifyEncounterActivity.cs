using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.EncounterModification.Activities;

public class ModifyEncounterActivity : IStateMachineActivity<EncounterModificationState, ModifyEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public ModifyEncounterActivity(IAggregateRepository<Domain.Encounter> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ModifyEncounterActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncounterModificationState, ModifyEncounter> context, IBehavior<EncounterModificationState, ModifyEncounter> next)
    {
        var encounter = await _repository.Get(context.Message.Id!.Value, context.CancellationToken);
        encounter.Modify(context.Message.RoomId, context.Saga.MedicalRecordId);
        
        _repository.Update(encounter);
        await _repository.Commit(context.CancellationToken);
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<EncounterModificationState, ModifyEncounter, TException> context, IBehavior<EncounterModificationState, ModifyEncounter> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}