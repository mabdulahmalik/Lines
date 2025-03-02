namespace SOL.Gateway.Views.PatientEncounter.Line;

public record LineProcedureView
{
    public Guid EncounterId { get; set; }
    public Guid EncounterProcedureId { get; set; }
}