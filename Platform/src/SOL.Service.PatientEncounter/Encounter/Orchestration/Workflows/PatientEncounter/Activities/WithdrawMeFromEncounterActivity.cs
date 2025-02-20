using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class WithdrawMeFromEncounterActivity : IStateMachineActivity<PatientEncounterState, WithdrawMeFromEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public WithdrawMeFromEncounterActivity(IAggregateRepository<Domain.Encounter> repository, IOperationContextFactory operationContextFactory)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(WithdrawMeFromEncounterActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<PatientEncounterState, WithdrawMeFromEncounter> context, IBehavior<PatientEncounterState, WithdrawMeFromEncounter> next)
    {
        var encounter = await _repository.Get(context.Message.EncounterId, context.CancellationToken);
        encounter.Withdraw([_operationContext.Value.ActorId]);

        _repository.Update(encounter);
        await _repository.Commit(context.CancellationToken);

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<PatientEncounterState, WithdrawMeFromEncounter, TException> context, IBehavior<PatientEncounterState, WithdrawMeFromEncounter> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}