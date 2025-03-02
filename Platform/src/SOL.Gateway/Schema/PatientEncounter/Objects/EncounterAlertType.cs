using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAlertType : ObjectType<EncounterAlertView>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterAlertView> descriptor)
    {
        descriptor
            .Name("EncounterAlert")
            .Description("An Alert for an Encounter. Important things that need to be surfaced about the Encounter.");

        descriptor
            .Field(x => x.AlertedAt)
            .Name("alertedAt")
            .Description("The date and time the Alert was created.");
        
        descriptor
            .Field(x => x.AlertedBy)
            .Name("alertedBy")
            .Description("The unique identifier of the User who created the Alert.");        
        
        descriptor
            .Field(x => x.Type)
            .Type<EncounterAlertTypeType>()
            .Name("type")
            .Description("The Type of Alert.");        
        
        descriptor
            .Field(x => x.Text)
            .Name("text")
            .Description("The text/reason for the Alert.");        
    }
}