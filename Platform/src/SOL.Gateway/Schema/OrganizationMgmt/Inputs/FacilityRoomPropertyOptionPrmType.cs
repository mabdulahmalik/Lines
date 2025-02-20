using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyOptionPrmType : InputObjectType<FacilityRoomPropertyOption>
{
    protected override void Configure(IInputObjectTypeDescriptor<FacilityRoomPropertyOption> descriptor)
    {
        descriptor
            .Name("FacilityRoomPropertyOptionPrm")
            .Description("The Parameters for the Room Property Option.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room Property Option.");

        descriptor
            .Field(x => x.Text)
            .Name("text")
            .Description("The text for Room Property Option.");
    }
}