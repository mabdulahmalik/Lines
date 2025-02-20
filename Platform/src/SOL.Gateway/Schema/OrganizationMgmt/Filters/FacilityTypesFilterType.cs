using HotChocolate.Data.Filters;
using SOL.Service.OrganizationMgmt.FacilityType.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityTypesFilterType : FilterInputType<FacilityTypeView>
{
    protected override void Configure(IFilterInputTypeDescriptor<FacilityTypeView> descriptor)
    {
        descriptor
            .Name("FacilityTypesFilter")
            .Description("Filters the Facility Type Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility Type.");

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
    }
}