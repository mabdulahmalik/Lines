namespace SOL.Gateway.Views.PatientEncounter.Line;

public record LineFollowUpView
{
    public DateOnly? Date { get; set; }
    public Guid? PurposeId { get; set; }
    public Guid? JobId { get; set; }
}