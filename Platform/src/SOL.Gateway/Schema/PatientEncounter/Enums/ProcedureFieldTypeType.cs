namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldTypeType : EnumType<Abstractions.Domain.ProcedureFieldType>
{
    protected override void Configure(IEnumTypeDescriptor<Abstractions.Domain.ProcedureFieldType> descriptor)
    {
        descriptor
            .Name("ProcedureFieldType")
            .Description("The type of data that a field can hold.");

        descriptor
            .Value(Abstractions.Domain.ProcedureFieldType.Text)
            .Name("TEXT")
            .Description("A field that holds textual data.");

        descriptor
            .Value(Abstractions.Domain.ProcedureFieldType.Number)
            .Name("NUMBER")
            .Description("A field that holds numeric data.");

        descriptor
            .Value(Abstractions.Domain.ProcedureFieldType.Toggle)
            .Name("TOGGLE")
            .Description("A field that holds a boolean value.");

        descriptor
            .Value(Abstractions.Domain.ProcedureFieldType.List)
            .Name("LIST")
            .Description("A field that holds a list of options.");
    }
}