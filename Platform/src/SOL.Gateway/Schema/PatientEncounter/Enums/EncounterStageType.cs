using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterStageType : EnumType<EncounterStage>
{
    protected override void Configure(IEnumTypeDescriptor<EncounterStage> descriptor)
    {
        descriptor
            .Name("EncounterStage")
            .Description("The Workflow Stage of the Encounter.");

        descriptor
            .Value(EncounterStage.Waiting)
            .Name("WAITING")
            .Description("The encounter is waiting and has not been assigned to a clinician.");
        
        descriptor
            .Value(EncounterStage.Assigned)
            .Name("ASSIGNED")
            .Description("The encounter has been assigned to one or more clinicians but they are not attending to it yet.");

        descriptor
            .Value(EncounterStage.Attending)
            .Name("ATTENDING")
            .Description("The patient is being attended to by the PRIMARY clinician.");
        
        descriptor
            .Value(EncounterStage.Charting)
            .Name("CHARTING")
            .Description("The patient has been attended to and the procedure details need to be charted.");

        descriptor
            .Value(EncounterStage.Completed)
            .Name("COMPLETED")
            .Description("The procedure details have been charted and the encounter can be closed out.");
    }
}
