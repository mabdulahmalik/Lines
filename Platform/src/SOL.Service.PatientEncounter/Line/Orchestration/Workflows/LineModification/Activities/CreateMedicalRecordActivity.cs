using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Workflows.LineModification.Activities;

public class CreateMedicalRecordActivity : IStateMachineActivity<LineModificationState, ModifyLine>
{
    private readonly IAggregateRepository<MedicalRecord.Domain.MedicalRecord> _repository;

    public CreateMedicalRecordActivity(IAggregateRepository<MedicalRecord.Domain.MedicalRecord> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateMedicalRecordActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LineModificationState, ModifyLine> context, IBehavior<LineModificationState, ModifyLine> next)
    {
        var medicalRecord = MedicalRecord.Domain.MedicalRecord.Create(context.Message.FacilityId, context.Message.MedicalRecord.Number!
            , context.Message.MedicalRecord.FirstName!, context.Message.MedicalRecord.LastName!
            , context.Message.MedicalRecord.Birthday!);

        await _repository.Add(medicalRecord, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.MedicalRecordId = medicalRecord.Id;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<LineModificationState, ModifyLine, TException> context
        , IBehavior<LineModificationState, ModifyLine> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}