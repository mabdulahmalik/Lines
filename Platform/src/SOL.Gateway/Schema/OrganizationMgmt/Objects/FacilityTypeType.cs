using SOL.Service.OrganizationMgmt.FacilityType.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityTypeType : ObjectType<FacilityTypeView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityTypeView> descriptor)
    {
        descriptor
            .Name("FacilityType")
            .Description("A Facility Type.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility Type.")
            .IsProjected();

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Facility Type.");

        descriptor
            .Field(x => x.Active)
            .Name("isActive")
            .Description("Whether the Facility Type is active.");

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Facility Type was created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Facility Type was last modified.");

        descriptor
            .Field("properties")
            .Description("A list of facility type room properties.")
            .Type<ListType<FacilityRoomPropertyType>>()
            .Resolve(async ctx => await ctx.DataLoader<FacilityRoomPropertyLoader>()
                .LoadAsync(ctx.Parent<FacilityTypeView>().Id, ctx.RequestAborted));
    }
}