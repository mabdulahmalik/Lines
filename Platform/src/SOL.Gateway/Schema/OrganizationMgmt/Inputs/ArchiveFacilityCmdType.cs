using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ArchiveFacilityCmdType : InputObjectType<ArchiveFacility>
{
    protected override void Configure(IInputObjectTypeDescriptor<ArchiveFacility> descriptor)
    {
        descriptor
            .Name("ArchiveFacilityCmd")
            .Description("The Command for Archiving a Facility.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("id")
            .Description("The unique identifier of the Facility to Archive.");        
    }
}