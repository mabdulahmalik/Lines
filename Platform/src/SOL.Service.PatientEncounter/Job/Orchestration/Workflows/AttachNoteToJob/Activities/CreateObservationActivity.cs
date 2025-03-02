using MassTransit;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.MedicalRecord.Domain;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.AttachNoteToJob.Activities;

public class CreateObservationActivity : IStateMachineActivity<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob>
{
    private readonly IAggregateRepository<MedicalRecord.Domain.MedicalRecord> _repository;

    public CreateObservationActivity(IAggregateRepository<MedicalRecord.Domain.MedicalRecord> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateObservationActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob> context
        , IBehavior<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob> next)
    {
        var medicalRecord = await _repository.Get(context.Message.MedicalRecordId!.Value, context.CancellationToken);
        var observation = new Observation(context.Saga.Note.Id, ObservationType.Note, context.Saga.Note.CreatedAt);
        
        medicalRecord.AddObservations(observation);
        
        await _repository.Update(medicalRecord, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob, TException> context
        , IBehavior<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}