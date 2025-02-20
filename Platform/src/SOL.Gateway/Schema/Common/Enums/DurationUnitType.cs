using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.Common;

public class DurationUnitType : EnumType<DurationUnit>
{
    protected override void Configure(IEnumTypeDescriptor<DurationUnit> descriptor)
    {
        descriptor
            .Name("DurationUnit")
            .Description("Represents the unit of time used to measure duration.");

        descriptor
            .Value(DurationUnit.Minutes)
            .Name("MINUTES")
            .Description("Duration is measured in minutes.");

        descriptor
            .Value(DurationUnit.Hours)
            .Name("HOURS")
            .Description("Duration is measured in hours.");

        descriptor
            .Value(DurationUnit.Days)
            .Name("DAYS")
            .Description("Duration is measured in days.");
    }
}