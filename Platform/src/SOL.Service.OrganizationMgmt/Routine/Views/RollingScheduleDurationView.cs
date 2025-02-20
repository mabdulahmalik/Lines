using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Views;

public record RollingScheduleDurationView
{
    public int Value { get; set; }
    public DurationUnit Unit { get; set; }
}