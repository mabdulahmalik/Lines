using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class PlaceLineInternallyCmdType : InputObjectType<PlaceLineInternally>
{
    protected override void Configure(IInputObjectTypeDescriptor<PlaceLineInternally> descriptor)
    {
        descriptor
            .Name("PlaceLineInternallyCmd")
            .Description("The Command that places a Line Internally.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");
    }
}