using MassTransit;
using SOL.Service.PatientEncounter.Job.Orchestration.Workflows.AttachNoteToJob.Activities;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.AttachNoteToJob;

public class AttachNoteToJobStateMachine : MassTransitStateMachine<AttachNoteToJobState> 
{
    public Event<Messaging.Contracts.PatientEncounter.AttachNoteToJob> AttachNoteToJob { get; set; }

    public AttachNoteToJobStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => AttachNoteToJob, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(AttachNoteToJob)
                .Activity(x => x.OfType<CreateJobNoteActivity>())
                .If(cnd => cnd.Message.MedicalRecordId.HasValue, 
                    x => x.Activity(y => y.OfType<CreateObservationActivity>()))
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }
}