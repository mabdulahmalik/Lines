using MassTransit;
using NodaTime;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.ServiceMesh.Workflows.JobIntake.Activities;

class JobScheduleActivity : IStateMachineActivity<JobIntakeState, ObjectCreated>
{
    private readonly ICommandBus _commandBus;

    public JobScheduleActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(JobScheduleActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobIntakeState, ObjectCreated> context, IBehavior<JobIntakeState, ObjectCreated> next)
    {
        var jobSchedulingCmd = new EstablishScheduledJobRunTime {
            JobId = context.Message.Id,
            FacilityId = context.Saga.FacilityId,
            Date = LocalDate.FromDateOnly(context.Saga.ScheduledDate!.Value)
        };

        await _commandBus.SendAsync(jobSchedulingCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobIntakeState, ObjectCreated, TException> context
        , IBehavior<JobIntakeState, ObjectCreated> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}