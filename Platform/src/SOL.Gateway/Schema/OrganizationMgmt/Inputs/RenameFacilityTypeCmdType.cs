using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RenameFacilityTypeCmdType : InputObjectType<RenameFacilityType>
{
    protected override void Configure(IInputObjectTypeDescriptor<RenameFacilityType> descriptor)
    {
        descriptor
            .Name("RenameFacilityTypeCmd")
            .Description("The Command for renaming a Facility Type.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility Type.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Facility Type.");
    }
}