using SOL.Gateway.Views.OrganizationMgmt.FacilityRoom;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityRoomType : ObjectType<FacilityRoomView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityRoomView> descriptor)
    {
        descriptor
            .Name("FacilityRoom")
            .Description("A Room within a Facility.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Room.")
            .IsProjected();

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Room.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier of the Facility.");

        descriptor
            .Field("properties")
            .Description("The list of Properties of the Room.")
            .Type<ListType<FacilityRoomPropertyValueType>>()
            .Resolve(async ctx => await ctx.DataLoader<FacilityRoomPropertyValueLoader>()
                .LoadAsync(ctx.Parent<FacilityRoomView>().Id, ctx.RequestAborted));

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the facility was created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the facility was last modified.");
    }
}
