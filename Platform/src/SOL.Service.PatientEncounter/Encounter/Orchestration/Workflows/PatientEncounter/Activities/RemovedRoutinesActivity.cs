using MassTransit;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class RemovedRoutinesActivity : IStateMachineActivity<PatientEncounterState>
{
    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(RemovedRoutinesActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Task Execute(BehaviorContext<PatientEncounterState> context, IBehavior<PatientEncounterState> next)
    {
        context.Saga.RoutinesRemoved.ForEach(rr 
            => context.Saga.RoutinesAssigned.RemoveAll(x => x.Item2 == rr.Item2));
        
        return Task.CompletedTask;
    }

    public Task Execute<T>(BehaviorContext<PatientEncounterState, T> context, IBehavior<PatientEncounterState, T> next) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<PatientEncounterState, TException> context, IBehavior<PatientEncounterState> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<PatientEncounterState, T, TException> context, IBehavior<PatientEncounterState, T> next) where T : class where TException : Exception
    {
        throw new NotImplementedException();
    }
}