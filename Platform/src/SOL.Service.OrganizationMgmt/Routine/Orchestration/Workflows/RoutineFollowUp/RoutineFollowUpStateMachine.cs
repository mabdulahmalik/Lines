using MassTransit;
using NodaTime;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Workflows.RoutineFollowUp;

public class RoutineFollowUpStateMachine : MassTransitStateMachine<RoutineFollowUpState>
{
    public State JobCreation { get; set; }
    
    public Event<CreateRoutineFollowUp> RoutineFollowUp { get; set; }
    public Event<ObjectCreated> JobCreated { get; set; }
    
    public RoutineFollowUpStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => RoutineFollowUp, x => x.CorrelateById(context => context.CorrelationId!.Value));
        Event(() => JobCreated, x => x.CorrelateById(context => context.CorrelationId!.Value));
        
        Initially(
            When(RoutineFollowUp)
                .Activity(act => act.OfType<EstablishNextRunTimeActivity>())
                .IfElse(cnd => cnd.Saga.IsRoutineActive
                    , act => act.Publish(PublishJobCreation).TransitionTo(JobCreation)
                    , act => act.Finalize()
                )
        );
        
        During(JobCreation,
            When(JobCreated)
                .IfElse(cnd => cnd.Saga.IsScheduledForToday
                    , act => act.Publish(ctx 
                        => ctx.Publish(new CreateEncounter {
                            JobId = ctx.Message.Id,
                            FacilityRoomId = ctx.Saga.RoomId,
                            PurposeId = ctx.Saga.PurposeId
                        }, ctx.CancellationToken))
                    , act => act.Publish(ctx 
                        => ctx.Publish(new EstablishScheduledJobRunTime {
                            JobId = ctx.Message.Id,
                            FacilityId = ctx.Saga.FacilityId,
                            Date = LocalDate.FromDateOnly(ctx.Saga.ScheduledDate)
                        }, ctx.CancellationToken))
                )
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }

    private static Task PublishJobCreation(BehaviorContext<RoutineFollowUpState, CreateRoutineFollowUp> ctx) 
        => ctx.Publish(new CreateJob {
            ScheduledDate = LocalDate.FromDateOnly(ctx.Saga.ScheduledDate),
            ScheduledTime = ctx.Saga.ScheduledTime.HasValue 
                ? LocalTime.FromTimeOnly(ctx.Saga.ScheduledTime.Value) 
                : null,
            PurposeId = ctx.Saga.PurposeId,
            RoomId = ctx.Message.FacilityRoomId,
            MedicalRecordId = ctx.Message.MedicalRecordId,
            RoutineProcedureId = ctx.Message.EncounterProcedureId,
            RoutineAssignmentId = ctx.Message.RoutineAssignmentId,
            LineId = ctx.Message.LineId 
        }, ctx.CancellationToken);
}