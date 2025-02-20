using MassTransit;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.EncounterModification.Activities;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.EncounterModification;

public class EncounterModificationStateMachine : MassTransitStateMachine<EncounterModificationState>
{
    public Event<ModifyEncounter> EncounterModified { get; set; }

    public EncounterModificationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => EncounterModified, x => x.CorrelateById(c => c.CorrelationId!.Value));

        Initially(
            When(EncounterModified)
                .Then(ctx => ctx.Saga.MedicalRecordId = ctx.Message.MedicalRecord.Id)
                .IfElse(ShouldCreateMedicalRecord, x => x.Activity(act => act.OfType<CreateMedicalRecordActivity>())
                    , e => e.If(ShouldModifyMedicalRecord, x => x.Activity(act => act.OfType<ModifyMedicalRecordActivity>())))
                .If(ShouldModifyEncounter, x => x.Activity(act => act.OfType<ModifyEncounterActivity>()))
                .Activity(x => x.OfType<ModifyJobActivity>())
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
    
    static bool ShouldCreateMedicalRecord(BehaviorContext<EncounterModificationState,ModifyEncounter> context)
    {
        return !context.Message.MedicalRecord.Id.HasValue
               && !String.IsNullOrWhiteSpace(context.Message.MedicalRecord.Number);
    }
    
    static bool ShouldModifyMedicalRecord(BehaviorContext<EncounterModificationState,ModifyEncounter> context)
    {
        return context.Message.MedicalRecord.Id.HasValue 
               && (!String.IsNullOrWhiteSpace(context.Message.MedicalRecord.FirstName)
                   || !String.IsNullOrWhiteSpace(context.Message.MedicalRecord.LastName)
                   || context.Message.MedicalRecord.Birthday.HasValue);
    } 
    
    static bool ShouldModifyEncounter(BehaviorContext<EncounterModificationState,ModifyEncounter> context)
    {
        return context.Message.Id.HasValue;
    }
}