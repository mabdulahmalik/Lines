namespace SOL.Service.PatientEncounter.Line.View;

public record LineProcedureView
{
    public Guid EncounterId { get; set; }
    public Guid EncounterProcedureId { get; set; }
}