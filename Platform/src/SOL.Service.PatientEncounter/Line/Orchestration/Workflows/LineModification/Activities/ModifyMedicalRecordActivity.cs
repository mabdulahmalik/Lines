using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Workflows.LineModification.Activities;

public class ModifyMedicalRecordActivity : IStateMachineActivity<LineModificationState, ModifyLine>
{
    private readonly IAggregateRepository<MedicalRecord.Domain.MedicalRecord> _repository;

    public ModifyMedicalRecordActivity(IAggregateRepository<MedicalRecord.Domain.MedicalRecord> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ModifyMedicalRecordActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LineModificationState, ModifyLine> context, IBehavior<LineModificationState, ModifyLine> next)
    {
        var medicalRecord = await _repository.Get(context.Message.MedicalRecord.Id!.Value, context.CancellationToken);
        
        medicalRecord.Modify(context.Message.MedicalRecord.FirstName!
            , context.Message.MedicalRecord.LastName!
            , context.Message.MedicalRecord.Birthday!);
        
        await _repository.Update(medicalRecord, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<LineModificationState, ModifyLine, TException> context
        , IBehavior<LineModificationState, ModifyLine> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}