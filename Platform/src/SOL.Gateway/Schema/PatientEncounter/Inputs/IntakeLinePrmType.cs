using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class IntakeLinePrmType : InputObjectType<IntakeLine>
{
    protected override void Configure(IInputObjectTypeDescriptor<IntakeLine> descriptor)
    {
        descriptor
            .Name("IntakeLinePrm")
            .Description("Holds the Line data for the Job Intake process.");        

        descriptor
            .Field(t => t.LineId)
            .Name("id")
            .Description("The unique identifier of the existing Line.");        
        
        descriptor
            .Field(t => t.IsExternallyPlaced)
            .Name("externallyPlaced")
            .Description("Whether or not the Line is externally placed.");        
        
        descriptor
            .Field(t => t.PlacedBy)
            .Name("placedBy")
            .Description("The name of the entity who placed the Line externally.");        
        
        descriptor
            .Field(t => t.Name)
            .Name("name")
            .Description("The name of the external Line.");
        
        descriptor
            .Field(t => t.InsertedOn)
            .Name("insertedOn")
            .Description("The date in which the external Line was inserted.");           
    }
}