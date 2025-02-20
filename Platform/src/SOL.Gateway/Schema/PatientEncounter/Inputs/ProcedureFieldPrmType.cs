using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldPrmType : InputObjectType<ProcedureField>
{
    protected override void Configure(IInputObjectTypeDescriptor<ProcedureField> descriptor)
    {
        descriptor
            .Name("ProcedureFieldPrm")
            .Description("The Parameters of a Procedure Field.");

        descriptor
            .Field(t => t.Name)
            .Name("name")
            .Description("The Name/Label of the Field.");

        descriptor
            .Field(t => t.Instruction)
            .Name("instruction")
            .Description("Instructions for the Field.");

        descriptor
            .Field(t => t.Type)
            .Type<ProcedureFieldTypeType>()
            .Name("type")
            .Description("The Type of data the Field will hold.");

        descriptor
            .Field(t => t.Required)
            .Name("required")
            .Description("Whether the field is Required.")
            .DefaultValue(false);

        descriptor
            .Field(t => t.AllowMultiSelect)
            .Name("allowMultiSelect")
            .Description("Whether the field allows multiple selections (for 'List' types only).")
            .DefaultValue(false);

        descriptor
            .Field(t => t.Options)
            .Type<ListType<StringType>>()
            .Name("options")
            .Description("The options available for fields of type LIST.");
    }
}