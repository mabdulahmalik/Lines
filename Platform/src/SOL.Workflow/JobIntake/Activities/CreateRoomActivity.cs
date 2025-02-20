using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Workflow.JobIntake.Activities;

class CreateRoomActivity : IStateMachineActivity<JobIntakeState, EnactJobIntake>
{
    private readonly ICommandBus _commandBus;

    public CreateRoomActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }
    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateRoomActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobIntakeState, EnactJobIntake> context, IBehavior<JobIntakeState, EnactJobIntake> next)
    {
        var createRoomCmd = new CreateFacilityRoom {
            FacilityId = context.Message.Location.FacilityId,
            Name = context.Message.Location.RoomName
        };
        
        await _commandBus.SendAsync(createRoomCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobIntakeState, EnactJobIntake, TException> context, IBehavior<JobIntakeState, EnactJobIntake> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}