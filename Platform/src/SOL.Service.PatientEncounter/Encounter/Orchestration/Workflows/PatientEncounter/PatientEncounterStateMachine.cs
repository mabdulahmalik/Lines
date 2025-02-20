using MassTransit;
using SOL.Abstractions.Domain;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Encounter.Domain;
using SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;
using EncounterProgressed = SOL.Messaging.Contracts.PatientEncounter.EncounterProgressed;


namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter;

public class PatientEncounterStateMachine : MassTransitStateMachine<PatientEncounterState>
{
    public State Assigned { get; set; }
    public State Attending { get; set; }
    public State Charting { get; set; }
    
    public Event<AssignMeToEncounter> AssignMeToEncounter { get; set; }
    public Event<AttendToPatient> AttendToPatient { get; set; }
    public Event<SubmitProcedures> SubmitProcedures { get; set; }
    public Event<CompleteEncounter> CompleteEncounter { get; set; }
    public Event<WithdrawMeFromEncounter> WithdrawMeFromEncounter { get; set; }
    public Event<ApplyProcedureToEncounter> ApplyProcedureToEncounter { get; set; }
    public Event<EncounterProgressed> EncounterProgressed { get; set; }
    public Event<EncounterRoomChanged> EncounterRoomChanged { get; set; }
    public Event<EncounterMedicalRecordChanged> EncounterMedicalRecordChanged { get; set; }
    
    private EventActivityBinder<PatientEncounterState, AssignMeToEncounter> AssignMeToEncounterTriggered =>
        When(AssignMeToEncounter).Activity(x => x.OfType<AssignMeToEncounterActivity>());
    
    private EventActivityBinder<PatientEncounterState, WithdrawMeFromEncounter> WithdrawMeFromEncounterTriggered =>
        When(WithdrawMeFromEncounter).Activity(x => x.OfType<WithdrawMeFromEncounterActivity>());    
    
    public PatientEncounterStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => AssignMeToEncounter, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => AttendToPatient, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => SubmitProcedures, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => CompleteEncounter, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => WithdrawMeFromEncounter, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => ApplyProcedureToEncounter, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => EncounterProgressed, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => EncounterRoomChanged, c => c.CorrelateById(z => z.Message.EncounterId));
        Event(() => EncounterMedicalRecordChanged, c => c.CorrelateById(z => z.Message.Id));

        DuringAny(
            When(EncounterProgressed)
                .If(cnd => cnd.Message.Stage == EncounterStage.Waiting, act => act.Finalize())
                .If(cnd => cnd.Message.Stage == EncounterStage.Assigned, act => act.TransitionTo(Assigned))
                .If(cnd => cnd.Message.Stage == EncounterStage.Attending, act => act.TransitionTo(Attending))
                .If(cnd => cnd.Message.Stage == EncounterStage.Charting, act => act.TransitionTo(Charting))
                .If(cnd => cnd.Message.Stage == EncounterStage.Completed, act => act.Finalize()),
            When(EncounterRoomChanged)
                .Then(a => a.Saga.FacilityRoomId = a.Message.RoomId),
            When(EncounterMedicalRecordChanged)
                .Then(a => a.Saga.MedicalRecordId = a.Message.MedicalRecordId)
        );
        
        Initially(AssignMeToEncounterTriggered.TransitionTo(Assigned));
        
        During(Assigned,
            When(AttendToPatient).Activity(x => x.OfType<AttendToPatientActivity>()),
            AssignMeToEncounterTriggered,
            WithdrawMeFromEncounterTriggered
        );

        During(Attending,
            When(SubmitProcedures).Activity(x => x.OfType<SubmitProceduresActivity>()),
            When(ApplyProcedureToEncounter).Activity(x => x.OfType<ApplyProcedureToEncounterActivity>()),
            AssignMeToEncounterTriggered,
            WithdrawMeFromEncounterTriggered
        );

        WhenEnter(Charting, x => x
            .If(cnd => cnd.Saga.LinesRemoved.Any(), act => act.Activity(x => x.OfType<RemovedLinesActivity>()))
            .If(cnd => cnd.Saga.LinesInserted.Any(), act => act.Activity(x => x.OfType<InsertedLinesActivity>()))
            .If(cnd => cnd.Saga.RoutinesRemoved.Any(), act => act.Activity(x => x.OfType<RemovedRoutinesActivity>()))
            .If(cnd => cnd.Saga.RoutinesAssigned.Any(), act => act.Activity(x => x.OfType<AssignedRoutinesActivity>()))
        );
        
        During(Charting,
            When(CompleteEncounter).Activity(x => x.OfType<CompleteEncounterActivity>())
        );
        
        SetCompletedWhenFinalized();
    }
}