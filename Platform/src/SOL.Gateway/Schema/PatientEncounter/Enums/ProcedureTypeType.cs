namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureTypeType : EnumType<Abstractions.Domain.ProcedureType>
{
    protected override void Configure(IEnumTypeDescriptor<Abstractions.Domain.ProcedureType> descriptor)
    {
        descriptor
            .Name("ProcedureType")
            .Description("The Types of a Procedure.");

        descriptor
            .Value(Abstractions.Domain.ProcedureType.Standard)
            .Name("STANDARD")
            .Description("Indicates a standard procedure.");

        descriptor
            .Value(Abstractions.Domain.ProcedureType.Insertion)
            .Name("INSERTION")
            .Description("Indicates the procedure is an Insertion.");

        descriptor
            .Value(Abstractions.Domain.ProcedureType.Removal)
            .Name("REMOVAL")
            .Description("Indicates the procedure is a Removal.");
    }
}