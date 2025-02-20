using MassTransit;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Workflow.EncounterRevision.Activities;

namespace SOL.Workflow.EncounterRevision;

public class EncounterRevisionStateMachine : MassTransitStateMachine<EncounterRevisionState>
{
    public State LocationDefined { get; set; }
    
    public Event<EnactEncounterRevision> RevisionEnacted { get; set; }
    public Event<ObjectCreated> InstanceCreated { get; set; }

    public EncounterRevisionStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => RevisionEnacted, c => c.CorrelateById(z => z.CorrelationId!.Value));
        Event(() => InstanceCreated, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(RevisionEnacted)
                .Then(StageSagaData)
                .IfElse(cd => !cd.Message.Location.RoomId.HasValue
                    , ab => ab.Activity(x => x.OfType<CreateRoomActivity>())
                    , ab => ab.TransitionTo(LocationDefined)
                )
        );
        
        During(Initial,
            When(InstanceCreated, ctx => ctx.Message.Name == "Room")
                .Then(ctx => ctx.Saga.RoomId = ctx.Message.Id)
                .TransitionTo(LocationDefined)
        );
        
        WhenEnter(LocationDefined, 
            ctx => ctx.Activity(x => x.OfType<EncounterModificationActivity>())
        );
        
        During(LocationDefined,
            When(InstanceCreated, ctx => ctx.Message.Name == "Job")
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }

    private void StageSagaData(BehaviorContext<EncounterRevisionState, EnactEncounterRevision> ctx)
    {
        ctx.Saga.JobId = ctx.Message.JobId;
        ctx.Saga.EncounterId = ctx.Message.EncounterId;
        
        ctx.Saga.RoomId = ctx.Message.Location.RoomId.GetValueOrDefault();
        ctx.Saga.RoomName = ctx.Message.Location.RoomName;
        ctx.Saga.FacilityId = ctx.Message.Location.FacilityId;
        ctx.Saga.OrderingProvider = ctx.Message.OrderingProvider;
        ctx.Saga.Contact = ctx.Message.Contact;
        
        ctx.Saga.MedicalRecordId = ctx.Message.MedicalRecord?.Id;
        ctx.Saga.FirstName = ctx.Message.MedicalRecord?.FirstName;
        ctx.Saga.LastName = ctx.Message.MedicalRecord?.LastName;
        ctx.Saga.Birthday = ctx.Message.MedicalRecord?.Birthday;
        ctx.Saga.Number = ctx.Message.MedicalRecord?.Number;
    }
}