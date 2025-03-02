using SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomPropertyType : ObjectType<FacilityRoomPropertyView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityRoomPropertyView> descriptor)
    {
        descriptor
            .Name("FacilityRoomProperty")
            .Description("The Room Property definition for a Facility Type.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room Property.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Room Property.");

        descriptor
            .Field("options")
            .Description("A list of Options for room property.")
            .Type<ListType<FacilityRoomPropertyOptionType>>()
            .Resolve(async ctx => await ctx.DataLoader<FacilityRoomPropertyOptionLoader>()
                .LoadAsync(ctx.Parent<FacilityRoomPropertyView>().Id, ctx.RequestAborted));            
            
    }
}
