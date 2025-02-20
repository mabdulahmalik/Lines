using MassTransit;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobActivation.Activities;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobActivation;

using CreateEncounter = Activities.CreateEncounter;

public class JobActivationStateMachine : MassTransitStateMachine<JobActivationState>
{
    public Event<ActivateScheduledJobs> ActivateScheduledJobs { get; set; }

    public JobActivationStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => ActivateScheduledJobs, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(ActivateScheduledJobs)
                .Activity(x => x.OfType<ActivateJob>())
                .Activity(x => x.OfType<CreateEncounter>())
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }
}