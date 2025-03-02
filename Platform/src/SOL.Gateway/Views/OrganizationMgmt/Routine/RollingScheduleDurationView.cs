using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.OrganizationMgmt.Routine;

public record RollingScheduleDurationView
{
    public int Value { get; set; }
    public DurationUnit Unit { get; set; }
}