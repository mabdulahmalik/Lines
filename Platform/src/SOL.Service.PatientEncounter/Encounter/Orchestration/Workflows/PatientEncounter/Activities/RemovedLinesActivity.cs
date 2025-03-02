using MassTransit;
using NodaTime;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Line.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class RemovedLinesActivity : IStateMachineActivity<PatientEncounterState>
{
    private readonly IAggregateRepository<Line.Domain.Line> _repository;

    public RemovedLinesActivity(IAggregateRepository<Line.Domain.Line> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(AssignedRoutinesActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<PatientEncounterState> context, IBehavior<PatientEncounterState> next)
    {
        var lineIds = context.Saga.LinesRemoved
            .Select(x => x.Value)
            .ToList();

        var lines = await _repository.List(new MultipleLinesSpecification(lineIds));
        foreach (var keyValuePair in context.Saga.LinesRemoved) {
            var line = lines.Single(x => x.Id == keyValuePair.Value);
            line.Close(Instant.FromDateTimeUtc(DateTime.Now), keyValuePair.Key);
            await _repository.Update(line, context.CancellationToken);
        }
        
        await _repository.Commit(context.CancellationToken);
        await next.Execute(context);
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