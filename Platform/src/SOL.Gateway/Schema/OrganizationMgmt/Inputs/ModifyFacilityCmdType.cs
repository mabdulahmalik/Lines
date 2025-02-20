using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class ModifyFacilityCmdType : InputObjectType<ModifyFacility>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyFacility> descriptor)
    {
        descriptor
            .Name("ModifyFacilityCmd")
            .Description("The Command for modifying a Facility.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Facility.");

        descriptor
            .Field(x => x.TimeZone)
            .Name("timeZone")
            .Description("The time zone for the Facility.");

        descriptor
            .Field(x => x.Address)
            .Type<AddressPrmType>()
            .Name("address")
            .Description("The Address of the Facility.");

        descriptor
            .Field(x => x.RequiredData)
            .Type<ListType<RequiredPatientDataType>>()
            .Name("requiredData")
            .Description("The required Patient data for the Facility.");            

        descriptor
            .Field(x => x.Assignments)
            .Type<ListType<RoutineAssignmentPrmType>>()
            .Name("assignments")
            .Description("The list of the Routine Assignments.");

        descriptor
            .Field(x => x.PurposeIds)
            .Type<ListType<UuidType>>()
            .Name("purposeIds")
            .Description("The list of Purpose Unique Identifiers that needs to be excluded.");

        descriptor
            .Field(x => x.ProcedureIds)
            .Type<ListType<UuidType>>()
            .Name("procedureIds")
            .Description("The list of Procedure Unique Identifiers that needs to be excluded.");
    }
}