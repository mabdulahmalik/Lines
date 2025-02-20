using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class CreateFacilityTypeCmdType : InputObjectType<CreateFacilityType>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateFacilityType> descriptor)
    {
        descriptor
            .Name("CreateFacilityTypeCmd")
            .Description("The Command for creating a Facility Type.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the facility type.");
    }
}