using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class PlaceLineExternallyCmdType : InputObjectType<PlaceLineExternally>
{
    protected override void Configure(IInputObjectTypeDescriptor<PlaceLineExternally> descriptor)
    {
        descriptor
            .Name("PlaceLineExternallyCmd")
            .Description("The Command that places a Line Externally.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");
        
        descriptor
            .Field(t => t.PlacedOn)
            .Name("placedOn")
            .Description("The date in which the Line was placed.");   
        
        descriptor
            .Field(t => t.PlacedBy)
            .Name("placedBy")
            .Description("By whom the Line was placed.");           
    }
}