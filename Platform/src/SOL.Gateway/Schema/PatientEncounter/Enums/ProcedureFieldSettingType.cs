using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ProcedureFieldSettingType : EnumType<ProcedureFieldSetting>
{
    protected override void Configure(IEnumTypeDescriptor<ProcedureFieldSetting> descriptor)
    {
        descriptor
            .Name("ProcedureFieldSetting")
            .Description("The settings for a procedure field.");

        descriptor
            .Value(ProcedureFieldSetting.Required)
            .Name("REQUIRED")
            .Description("Indicates the field is Requires a value.");

        descriptor
            .Value(ProcedureFieldSetting.MultiSelect)
            .Name("MULTI_SELECT")
            .Description("Applicable to Fields of 'List' only. Allows for multiple options to be selected from that list.");
    }
}