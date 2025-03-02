using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class CreateProcedureCmdType : InputObjectType<CreateProcedure>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateProcedure> descriptor)
    {
        descriptor
            .Name("CreateProcedureCmd")
            .Description("The Command that holds the data needed to create a new Procedure.");

        descriptor
            .Field(t => t.Name)
            .Name("name")
            .Description("The Name/Descriptor of the Procedure to create.");

        descriptor
            .Field(t => t.EnablePerformanceReporting)
            .Name("enablePerformanceReporting")
            .Description("Whether performance reporting is enabled for the Procedure.")
            .DefaultValue(false);

        descriptor
            .Field(t => t.Type)
            .Type<ProcedureTypeType>()
            .Name("type")
            .Description("The Type of Procedure being created.")
            .DefaultValue(Abstractions.Domain.ProcedureType.Standard);
        
        descriptor
            .Field(t => t.RequiredData)
            .Type<ListType<RequiredPatientDataType>>()
            .Name("requiredData")
            .Description("The required Patient Data fields for the Procedure.");
        
        descriptor
            .Field(t => t.Fields)
            .Type<ListType<ProcedureFieldPrmType>>()
            .Name("fields")
            .Description("The fields that are part of the Procedure.");
    }
}