using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.EncounterModification.Activities;

public class ModifyMedicalRecordActivity : IStateMachineActivity<EncounterModificationState, ModifyEncounter>
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

    public async Task Execute(BehaviorContext<EncounterModificationState, ModifyEncounter> context, IBehavior<EncounterModificationState, ModifyEncounter> next)
    {
        var medicalRecord = await _repository.Get(context.Message.MedicalRecord.Id!.Value, context.CancellationToken);
        
        medicalRecord.Modify(context.Message.MedicalRecord.FirstName!
            , context.Message.MedicalRecord.LastName!
            , context.Message.MedicalRecord.Birthday!);
        
        _repository.Update(medicalRecord);
        await _repository.Commit(context.CancellationToken);

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<EncounterModificationState, ModifyEncounter, TException> context
        , IBehavior<EncounterModificationState, ModifyEncounter> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}