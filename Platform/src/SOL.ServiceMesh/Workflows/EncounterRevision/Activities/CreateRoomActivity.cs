using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.ServiceMesh.Workflows.EncounterRevision.Activities;

public class CreateRoomActivity : IStateMachineActivity<EncounterRevisionState, EnactEncounterRevision>
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

    public async Task Execute(BehaviorContext<EncounterRevisionState, EnactEncounterRevision> context, IBehavior<EncounterRevisionState, EnactEncounterRevision> next)
    {
        var createRoomCmd = new CreateFacilityRoom {
            FacilityId = context.Saga.FacilityId,
            Name = context.Saga.RoomName!
        };
        
        await _commandBus.SendAsync(createRoomCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<EncounterRevisionState, EnactEncounterRevision, TException> context, IBehavior<EncounterRevisionState, EnactEncounterRevision> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}