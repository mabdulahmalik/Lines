namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProgressStageType : ObjectType<EncounterProgressStage>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterProgressStage> descriptor)
    {
        descriptor
            .Name("EncounterProgressStage")
            .Description("Represents the progression of an Encounter through the workflow.");

        descriptor
            .Field(t => t.Stage)
            .Type<EncounterStageType>()
            .Name("stage")
            .Description("The Workflow Stage.");        
        
        descriptor
            .Field(t => t.Timestamp)
            .Name("enteredAt")
            .Description("When the Encounter entered this stage.");
        
        descriptor
            .Field(t => t.Duration)
            .Name("duration")
            .Description("The amount of time spent in this stage (in minutes).");        
    }
}