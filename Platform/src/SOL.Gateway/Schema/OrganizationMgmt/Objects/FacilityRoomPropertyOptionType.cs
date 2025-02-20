using SOL.Service.OrganizationMgmt.FacilityRoom.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyOptionType : ObjectType<FacilityRoomPropertyOptionView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityRoomPropertyOptionView> descriptor)
    {
        descriptor
            .Name("FacilityRoomPropertyOption")
            .Description("The Option definition for a Room Property.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Option.");

        descriptor
            .Field(x => x.Text)
            .Name("text")
            .Description("The text for Option.");
    }
}