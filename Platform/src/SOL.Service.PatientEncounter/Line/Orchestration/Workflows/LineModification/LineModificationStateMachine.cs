using MassTransit;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Line.Orchestration.Workflows.LineModification.Activities;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Workflows.LineModification;

public class LineModificationStateMachine : MassTransitStateMachine<LineModificationState>
{
    public Event<ModifyLine> LineModified  { get; set; }

    public LineModificationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => LineModified, x => x.CorrelateById(c => c.CorrelationId!.Value));
        
        Initially(
            When(LineModified)
                .Then(ctx => ctx.Saga.MedicalRecordId = ctx.Message.MedicalRecord.Id)
                .IfElse(ShouldCreateMedicalRecord, x => x.Activity(act => act.OfType<CreateMedicalRecordActivity>())
                    , e => e.If(ShouldModifyMedicalRecord, x => x.Activity(act => act.OfType<ModifyMedicalRecordActivity>())))
                .Activity(x => x.OfType<ModifyLineActivity>())
                .Finalize()
        );

        SetCompletedWhenFinalized();        
    }
    
    static bool ShouldCreateMedicalRecord(BehaviorContext<LineModificationState,ModifyLine> context)
    {
        return !context.Message.MedicalRecord.Id.HasValue
               && !String.IsNullOrWhiteSpace(context.Message.MedicalRecord.Number);
    }
    
    static bool ShouldModifyMedicalRecord(BehaviorContext<LineModificationState,ModifyLine> context)
    {
        return context.Message.MedicalRecord.Id.HasValue 
               && (!String.IsNullOrWhiteSpace(context.Message.MedicalRecord.FirstName)
                   || !String.IsNullOrWhiteSpace(context.Message.MedicalRecord.LastName)
                   || context.Message.MedicalRecord.Birthday.HasValue);
    } 
}