using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.MedicalRecord;

public class MedicalRecordObservationView : IEntityTypeConfiguration<MedicalRecordObservationView>
{
    public Guid MedicalRecordId { get; set; }
    public Guid ObjectId { get; set; }
    public ObservationType Type { get; set; }
    public DateTime Timestamp { get; set; }    
    public string Data { get; set; }
    
    public void Configure(EntityTypeBuilder<MedicalRecordObservationView> builder)
    {
        builder.ToView("vw_MedicalRecordObservation", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(e => new { e.MedicalRecordId, e.ObjectId, e.Type });
    }
}