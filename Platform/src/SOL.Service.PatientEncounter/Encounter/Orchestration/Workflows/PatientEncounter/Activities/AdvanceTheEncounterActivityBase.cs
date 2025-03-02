using MassTransit;
using SOL.Abstractions.Persistence;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public abstract class AdvanceTheEncounterActivityBase<TEvent> : IStateMachineActivity<PatientEncounterState, TEvent>
    where TEvent : class
{
    protected readonly IAggregateRepository<Domain.Encounter> Repository;

    public AdvanceTheEncounterActivityBase(IAggregateRepository<Domain.Encounter> repository)
    {
        Repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(GetType().Name);
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public abstract Task Execute(BehaviorContext<PatientEncounterState, TEvent> context
        , IBehavior<PatientEncounterState, TEvent> next);

    public async Task Faulted<TException>(BehaviorExceptionContext<PatientEncounterState, TEvent, TException> context
        , IBehavior<PatientEncounterState, TEvent> next) where TException : Exception
    {
        await next.Faulted(context);
    }
    
    protected async Task AdvanceTheEncounter(Guid encounterId, CancellationToken stoppageToken)
    {
        var encounter = await Repository.Get(encounterId, stoppageToken);
        encounter.Advance();
        
        await Repository.Update(encounter, stoppageToken);
        await Repository.Commit(stoppageToken);
    }
}