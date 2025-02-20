using MassTransit;
using NodaTime;
using SOL.Abstractions.Persistence;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class InsertedLinesActivity : IStateMachineActivity<PatientEncounterState>
{
    private readonly IAggregateRepository<Line.Domain.Line> _repository;

    public InsertedLinesActivity(IAggregateRepository<Line.Domain.Line> repository)
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
        foreach (var keyValuePair in context.Saga.LinesInserted)
        {
            var lineData = keyValuePair.Value;
            var line = Line.Domain.Line.Create(context.Saga.FacilityRoomId, lineData.Name, lineData.Type
                , insertedWith: keyValuePair.Key
                , insertedOn: Instant.FromDateTimeUtc(lineData.InsertedOn)
                , medicalRecordId: context.Saga.MedicalRecordId);
            
            await _repository.Add(line);
            lineData.Id = line.Id;
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