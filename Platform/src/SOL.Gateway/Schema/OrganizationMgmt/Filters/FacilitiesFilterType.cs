using HotChocolate.Data.Filters;
using SOL.Gateway.Schema.Common;
using SOL.Service.OrganizationMgmt.Facility.View;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilitiesFilterType : FilterInputType<FacilityView>
{
    protected override void Configure(IFilterInputTypeDescriptor<FacilityView> descriptor)
    {
        descriptor
            .Name("FacilitiesFilter")
            .Description("Filters the Facilities Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Facility.");

        descriptor
            .Field(x => x.TypeId)
            .Name("typeId")
            .Description("The unique identifier of the Facility Type.");

        descriptor
            .Field(x => x.TimeZone)
            .Name("timeZone")
            .Description("The time zone of the Facility.");

        descriptor
            .Field(x => x.Address)
            .Type<AddressFilterType>()
            .Name("address")
            .Description("The Address of the Facility.");

        descriptor
            .Field(x => x.RequiredData)
            .Name("requiredData")
            .Description("The required Patient data for the Facility.");

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Facility was Created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Facility was last Modified.");        
    }
}