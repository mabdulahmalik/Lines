using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation.Activities;

public class CreateMedicalRecordActivity : IStateMachineActivity<JobFormationState, PrepareJob>
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

    public async Task Execute(BehaviorContext<JobFormationState, PrepareJob> context, IBehavior<JobFormationState, PrepareJob> next)
    {
        var medicalRecord = MedicalRecord.Domain.MedicalRecord.Create(context.Message.FacilityId, context.Message.MedicalRecord.Number);

        await _repository.Add(medicalRecord, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.MedicalRecordId = medicalRecord.Id;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobFormationState, PrepareJob, TException> context, IBehavior<JobFormationState, PrepareJob> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}