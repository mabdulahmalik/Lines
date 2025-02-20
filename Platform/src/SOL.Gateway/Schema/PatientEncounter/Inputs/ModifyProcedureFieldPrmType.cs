using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ModifyProcedureFieldPrmType : InputObjectType<ModifyProcedureField>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyProcedureField> descriptor)
    {
        descriptor
            .Name("ModifyProcedureFieldPrm")
            .Description("The Parameters for a ProcedureField.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Field.");

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
            .Field(t => t.Archived)
            .Name("archived")
            .Description("Whether or not the Field is Archived.");        

        descriptor
            .Field(t => t.Required)
            .Name("required")
            .Description("Whether the Field is required.");

        descriptor
            .Field(t => t.AllowMultiSelect)
            .Name("allowMultiSelect")
            .Description("Whether the Field allows multiple selections (only for 'List' types).");

        descriptor
            .Field(t => t.Options)
            .Type<ListType<ModifyProcedureFieldOptionPrmType>>()
            .Name("options")
            .Description("The options available for fields of type LIST.");
    }
}