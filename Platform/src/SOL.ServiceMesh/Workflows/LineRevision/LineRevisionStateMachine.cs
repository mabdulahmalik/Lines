using MassTransit;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.ServiceMesh.Workflows.LineRevision.Activities;

namespace SOL.ServiceMesh.Workflows.LineRevision;

public class LineRevisionStateMachine : MassTransitStateMachine<LineRevisionState>
{
    public State LocationIdentified { get; set; }
    
    public Event<EnactLineRevision> RevisionEnacted { get; set; }
    public Event<ObjectModified> InstanceModified { get; set; }

    public LineRevisionStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => RevisionEnacted, c => c.CorrelateById(z => z.CorrelationId!.Value));
        Event(() => InstanceModified, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(RevisionEnacted)
                .Then(StageSagaData)
                .IfElse(cd => !cd.Message.Location.RoomId.HasValue
                    , ab => ab.Activity(x => x.OfType<CreateRoomActivity>())
                    , ab => ab.TransitionTo(LocationIdentified)
                )
        );
        
        During(Initial,
            When(InstanceModified, ctx => ctx.Message.Name == "Room")
                .Then(ctx => ctx.Saga.RoomId = ctx.Message.Id)
                .TransitionTo(LocationIdentified)
        );
        
        WhenEnter(LocationIdentified, 
            ctx => ctx.Activity(x => x.OfType<LineModificationActivity>())
        );
        
        During(LocationIdentified,
            When(InstanceModified, ctx => ctx.Message.Name == "Line")
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }
    
    private void StageSagaData(BehaviorContext<LineRevisionState, EnactLineRevision> ctx)
    {
        ctx.Saga.LineId = ctx.Message.Id;
        ctx.Saga.LineName = ctx.Message.Name;
        
        ctx.Saga.RoomId = ctx.Message.Location.RoomId.GetValueOrDefault();
        ctx.Saga.RoomName = ctx.Message.Location.RoomName;
        ctx.Saga.FacilityId = ctx.Message.Location.FacilityId;
        
        ctx.Saga.MedicalRecordId = ctx.Message.MedicalRecord?.Id;
        ctx.Saga.FirstName = ctx.Message.MedicalRecord?.FirstName;
        ctx.Saga.LastName = ctx.Message.MedicalRecord?.LastName;
        ctx.Saga.Birthday = ctx.Message.MedicalRecord?.Birthday;
        ctx.Saga.Number = ctx.Message.MedicalRecord?.Number;
    }    
}