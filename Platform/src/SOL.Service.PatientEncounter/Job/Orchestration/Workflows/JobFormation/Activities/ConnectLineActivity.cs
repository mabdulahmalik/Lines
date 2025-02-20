using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation.Activities;

public class ConnectLineActivity : IStateMachineActivity<JobFormationState, PrepareJob>
{
    private readonly IAggregateRepository<Line.Domain.Line> _repository;

    public ConnectLineActivity(IAggregateRepository<Line.Domain.Line> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ConnectLineActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobFormationState, PrepareJob> context, IBehavior<JobFormationState, PrepareJob> next)
    {
        var line = await _repository.Get(context.Saga.LineId!.Value, context.CancellationToken);
        line.Modify(line.Name, context.Message.RoomId
            , context.Saga.MedicalRecordId ?? line.MedicalRecordId);
        line.AttachEncounters(context.Saga.EncounterId);
        
        _repository.Update(line);
        await _repository.Commit(context.CancellationToken);
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobFormationState, PrepareJob, TException> context, IBehavior<JobFormationState, PrepareJob> next) where TException : Exception
    {
        await next.Execute(context);
    }
}