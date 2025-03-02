using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureSettingType : EnumType<ProcedureSetting>
{
    protected override void Configure(IEnumTypeDescriptor<ProcedureSetting> descriptor)
    {
        descriptor
            .Name("ProcedureSetting")
            .Description("The settings for a Procedure.");

        descriptor
            .Value(ProcedureSetting.PerformanceReporting)
            .Name("PERFORMANCE_REPORTING")
            .Description("The procedure should be included in Performance Reporting.");
    }
}