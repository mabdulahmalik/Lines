using MassTransit;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation.Activities;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation;

public class JobFormationStateMachine : MassTransitStateMachine<JobFormationState>
{
    public Event<PrepareJob> PrepareJob { get; set; }

    public JobFormationStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => PrepareJob, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(PrepareJob)
                .Then(ctx => {
                    ctx.Saga.LineId = ctx.Message.Line?.LineId;
                    ctx.Saga.MedicalRecordId = ctx.Message.MedicalRecord?.Id;                    
                })
                .If(x => !x.Saga.MedicalRecordId.HasValue && !String.IsNullOrWhiteSpace(x.Message.MedicalRecord?.Number)
                    , act => act.Activity(x => x.OfType<CreateMedicalRecordActivity>()))
                .If(x => !x.Saga.LineId.HasValue && (x.Message.Line?.IsExternallyPlaced ?? false)
                    , act => act.Activity(x => x.OfType<CreateExternalLineActivity>()))
                .Activity(x => x.OfType<CreateJobActivity>())
                .If(x => x.Message.Schedule == null
                    , act => act
                        .Activity(x => x.OfType<CreateEncounterActivity>())
                        .If(x => x.Saga.LineId.HasValue
                            , z => z.Activity(x => x.OfType<ConnectLineActivity>())))
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }
}