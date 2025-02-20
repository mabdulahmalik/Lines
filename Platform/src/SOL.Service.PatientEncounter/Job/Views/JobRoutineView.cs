using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Job.Views;

public class JobRoutineView : IEntityTypeConfiguration<JobRoutineView>
{
    public Guid JobId { get; set; }
    public Guid EncounterId { get; set; }
    public Guid EncounterProcedureId { get; set; }
    public Guid? RoutineAssignmentId { get; set; }
    public Guid? RoutineId { get; set; }
    
    public void Configure(EntityTypeBuilder<JobRoutineView> builder)
    {
        builder.ToView("vw_JobRoutine");
        builder.HasNoKey();

        builder.Property(x => x.RoutineAssignmentId).HasColumnName("FacilityRoutineId");
    }
}