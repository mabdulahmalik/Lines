using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.ServiceMesh.Workflows.JobReschedule.Activities;

public class EstablishRunTimeActivity : IStateMachineActivity<JobRescheduleState, EnactJobReschedule>
{
    private readonly ICommandBus _commandBus;

    public EstablishRunTimeActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(EstablishRunTimeActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobRescheduleState, EnactJobReschedule> context, IBehavior<JobRescheduleState, EnactJobReschedule> next)
    {
        var runTimeCmd = new EstablishScheduledJobRunTime {
            FacilityId = context.Message.FacilityId,
            Date = context.Message.Date,
            JobId = context.Message.Id
        };

        await _commandBus.SendAsync(runTimeCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobRescheduleState, EnactJobReschedule, TException> context
        , IBehavior<JobRescheduleState, EnactJobReschedule> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}