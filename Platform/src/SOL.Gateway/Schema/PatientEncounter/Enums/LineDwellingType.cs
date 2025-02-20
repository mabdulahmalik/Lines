using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineDwellingType : EnumType<LineDwelling>
{
    protected override void Configure(IEnumTypeDescriptor<LineDwelling> descriptor)
    {
        descriptor
            .Name("LineDwelling")
            .Description("The dwelling of the line.");

        descriptor
            .Value(LineDwelling.Undetermined)
            .Name("UNDETERMINED")
            .Description("Indicates the Line dwelling status is undetermined.");        
        
        descriptor
            .Value(LineDwelling.In)
            .Name("IN")
            .Description("Indicates the Line is dwelling IN the patient.");

        descriptor
            .Value(LineDwelling.Out)
            .Name("OUT")
            .Description("Indicates the Line is no longer dwelling in the patient.");
    }
}