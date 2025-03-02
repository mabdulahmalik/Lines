using MassTransit;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.ServiceMesh.Workflows.JobIntake.Activities;

namespace SOL.ServiceMesh.Workflows.JobIntake;

public class JobIntakeStateMachine : MassTransitStateMachine<JobIntakeState>
{
    public State LocationDetermined { get; set; }
    
    public Event<EnactJobIntake> IntakeEnacted { get; set; }
    public Event<ObjectCreated> DependencyCreated { get; set; }
    
    public JobIntakeStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => IntakeEnacted, c => c.CorrelateById(z => z.CorrelationId!.Value));
        Event(() => DependencyCreated, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(IntakeEnacted)
                .Then(StageSagaData)
                .IfElse(cd => !cd.Message.Location.RoomId.HasValue
                    , ab => ab.Activity(x => x.OfType<CreateRoomActivity>())
                    , ab => ab.TransitionTo(LocationDetermined)
                )
        );
        
        During(Initial,
            When(DependencyCreated, ctx => ctx.Message.Name == "Room")
                .Then(ctx => ctx.Saga.RoomId = ctx.Message.Id)
                .TransitionTo(LocationDetermined)
        );
        
        WhenEnter(LocationDetermined, ctx => ctx
            .Activity(x => x.OfType<JobPreperationActivity>())
        );
        
        During(LocationDetermined, 
            When(DependencyCreated, ctx => ctx.Message.Name == "Job")
                .If(cnd => cnd.Saga.ScheduledDate.HasValue
                    , act => act.Activity(x => x.OfType<JobScheduleActivity>())
                )
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }
    
    static void StageSagaData(BehaviorContext<JobIntakeState, EnactJobIntake> ctx)
    {
        ctx.Saga.RoomId = ctx.Message.Location.RoomId.GetValueOrDefault();
        ctx.Saga.FacilityId = ctx.Message.Location.FacilityId;
        ctx.Saga.PurposeId = ctx.Message.PurposeId;
        ctx.Saga.OrderingProvider = ctx.Message.OrderingProvider;
        ctx.Saga.Contact = ctx.Message.Contact;
        ctx.Saga.PreNote = ctx.Message.PreNote;
        ctx.Saga.ExternalLineName = ctx.Message.Line?.Name;
        ctx.Saga.ExternalLinePlacedBy = ctx.Message.Line?.PlacedBy;
        ctx.Saga.ExternalLinePlaced = ctx.Message.Line?.IsExternallyPlaced ?? false;
        ctx.Saga.ExternalLineInsertedOn = ctx.Message.Line?.InsertedOn?.ToDateTimeUtc();
        ctx.Saga.IsStat = ctx.Message.Urgency?.IsStat ?? false;
        ctx.Saga.IsOnHold = ctx.Message.Urgency?.IsOnHold ?? false;
        ctx.Saga.StatReason = ctx.Message.Urgency?.StatReason;
        ctx.Saga.HoldReason = ctx.Message.Urgency?.HoldReason;
        ctx.Saga.ScheduledDate = ctx.Message.Schedule?.Date.ToDateOnly();
        ctx.Saga.ScheduledTime = ctx.Message.Schedule?.Time?.ToTimeOnly();
        ctx.Saga.MedicalRecordId = ctx.Message.MedicalRecord?.Id;
        ctx.Saga.MedicalRecordNumber = ctx.Message.MedicalRecord?.Number;
        ctx.Saga.LineId = ctx.Message.Line?.LineId;
    }
}