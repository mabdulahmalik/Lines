using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class CreateFacilityCmdType : InputObjectType<CreateFacility>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateFacility> descriptor)
    {
        descriptor
            .Name("CreateFacilityCmd")
            .Description("The Command for creating a new facility.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the facility.");

        descriptor
            .Field(x => x.TypeId)
            .Name("typeId")
            .Description("The unique identifier of the Facility Type.");

        descriptor
            .Field(x => x.TimeZone)
            .Name("timeZone")
            .Description("The time zone for the facility.");

        descriptor
            .Field(x => x.Address)
            .Type<AddressPrmType>()
            .Name("address")
            .Description("The address of the facility.");

        descriptor
            .Field(x => x.RequiredData)
            .Type<ListType<RequiredPatientDataType>>()
            .Name("requiredData")
            .Description("The required Patient data for the facility.");
    }
}