using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ModifyProcedureCmdType : InputObjectType<ModifyProcedure>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyProcedure> descriptor)
    {
        descriptor
            .Name("ModifyProcedureCmd")
            .Description("The Command to modify the definition of a Procedure.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.");

        descriptor
            .Field(t => t.Name)
            .Name("name")
            .Description("The Name/Descriptor of the Procedure to update.");

        descriptor
            .Field(t => t.EnablePerformanceReporting)
            .Name("enablePerformanceReporting")
            .Description("Whether Performance Reporting is enabled for the Procedure.");

        descriptor
            .Field(t => t.IsInsertion)
            .Name("isInsertion")
            .Description("Whether the Procedure is an Insertion.");

        descriptor
            .Field(t => t.IsRemoval)
            .Name("isRemoval")
            .Description("Whether the Procedure is a Removal.");

        descriptor
            .Field(t => t.RequiredData)
            .Type<ListType<RequiredPatientDataType>>()
            .Name("requiredData")
            .Description("The required Patient data fields for the Procedure.");

        descriptor
            .Field(t => t.Fields)
            .Type<ListType<ModifyProcedureFieldPrmType>>()
            .Name("fields")
            .Description("The fields that are part of the Procedure.");
    }
}