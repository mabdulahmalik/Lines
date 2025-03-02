using MassTransit;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.ServiceMesh.Workflows.JobReschedule.Activities;

namespace SOL.ServiceMesh.Workflows.JobReschedule;

public class JobRescheduleStateMachine : MassTransitStateMachine<JobRescheduleState>
{
    public State EstablishingRunTime { get; set; }
    public State ReschedulingJob { get; set; }
    
    public Event<EnactJobReschedule> JobRescheduleEnacted { get; set; }
    public Event<ScheduledJobRunTimeEstablished> RunTimeEstablished { get; set; }
    public Event<JobRescheduled> JobRescheduled { get; set; }
    
    public JobRescheduleStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => JobRescheduleEnacted, x => x.CorrelateById(context => context.Message.Id));
        Event(() => RunTimeEstablished, x => x.CorrelateById(context => context.Message.JobId));
        Event(() => JobRescheduled, x => x.CorrelateById(context => context.Message.Id));

        Initially(
            When(JobRescheduleEnacted)
                .Then(StageSagaData)
                .Activity(act => act.OfType<EstablishRunTimeActivity>())
                .TransitionTo(EstablishingRunTime)
        );
        
        During(EstablishingRunTime,
            When(RunTimeEstablished)
                .Activity(act => act.OfType<RescheduleJobActivity>())
                .TransitionTo(ReschedulingJob)
            );
        
        During(ReschedulingJob,
            When(JobRescheduled).Finalize()
        );

        SetCompletedWhenFinalized();
    }

    static void StageSagaData(BehaviorContext<JobRescheduleState, EnactJobReschedule> ctx)
    {
        ctx.Saga.JobId = ctx.Message.Id;
        ctx.Saga.Reason = ctx.Message.Reason;
        ctx.Saga.Date = ctx.Message.Date.ToDateOnly();
        
        if (ctx.Message.Time.HasValue)
            ctx.Saga.Time = ctx.Message.Time.Value.ToTimeOnly();
    }
}