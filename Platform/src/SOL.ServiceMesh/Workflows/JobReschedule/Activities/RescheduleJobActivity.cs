using MassTransit;
using NodaTime.Extensions;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.ServiceMesh.Workflows.JobReschedule.Activities;

public class RescheduleJobActivity : IStateMachineActivity<JobRescheduleState, ScheduledJobRunTimeEstablished>
{
    private readonly ICommandBus _commandBus;

    public RescheduleJobActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(RescheduleJobActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobRescheduleState, ScheduledJobRunTimeEstablished> context, IBehavior<JobRescheduleState, ScheduledJobRunTimeEstablished> next)
    {
        var rescheduleJob = new RescheduleJob {
            Id = context.Saga.JobId,
            Reason = context.Saga.Reason,
            Date = context.Saga.Date.ToLocalDate(),
            Time = context.Saga.Time?.ToLocalTime()
        };
        
        await _commandBus.SendAsync(rescheduleJob, context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobRescheduleState, ScheduledJobRunTimeEstablished, TException> context
        , IBehavior<JobRescheduleState, ScheduledJobRunTimeEstablished> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}