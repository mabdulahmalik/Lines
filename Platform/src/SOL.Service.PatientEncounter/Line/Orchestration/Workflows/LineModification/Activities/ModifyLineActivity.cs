using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Workflows.LineModification.Activities;

public class ModifyLineActivity : IStateMachineActivity<LineModificationState, ModifyLine>
{
    private readonly IAggregateRepository<Domain.Line> _repository;

    public ModifyLineActivity(IAggregateRepository<Domain.Line> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ModifyLineActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LineModificationState, ModifyLine> context, IBehavior<LineModificationState, ModifyLine> next)
    {
        var line = await _repository.Get(context.Message.Id, context.CancellationToken);
        line.Modify(context.Message.Name, context.Message.RoomId, context.Saga.MedicalRecordId);
        
        await _repository.Update(line, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<LineModificationState, ModifyLine, TException> context
        , IBehavior<LineModificationState, ModifyLine> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}