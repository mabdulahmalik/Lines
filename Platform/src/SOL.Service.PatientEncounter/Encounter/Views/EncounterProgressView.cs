using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Encounter.Views;

public class EncounterProgressView : IEntityTypeConfiguration<EncounterProgressView>
{
    public Guid EncounterId { get; set; }
    public DateTime Waiting { get; set; }
    public int? WaitingDuration { get; set; }
    public DateTime? Assigned { get; set; }
    public int? AssignedDuration { get; set; }
    public DateTime? Attending { get; set; }
    public int? AttendingDuration { get; set; }
    public DateTime? Charting { get; set; }
    public int? ChartingDuration { get; set; }
    public DateTime? Completed { get; set; } 
    
    public void Configure(EntityTypeBuilder<EncounterProgressView> builder)
    {
        builder.ToView("vw_EncounterProgress");
        builder.HasKey(x => x.EncounterId);
        
        builder.Property(x => x.WaitingDuration).HasColumnName("Waiting_Duration");
        builder.Property(x => x.AssignedDuration).HasColumnName("Assigned_Duration");
        builder.Property(x => x.AttendingDuration).HasColumnName("Attending_Duration");
        builder.Property(x => x.ChartingDuration).HasColumnName("Charting_Duration");
    }
}