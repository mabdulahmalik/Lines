using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.MedicalRecord.View;

public class MedicalRecordView : IEntityTypeConfiguration<MedicalRecordView>
{
    public Guid Id { get; set; }
    public Guid? FacilityId { get; set; }
    public string? Number { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? Birthday { get; set; }
    public DateOnly? FirstSeenOn { get; set; }
    public DateOnly? LastSeenOn { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? LinesTotal { get; set; }
    public int? LinesIn { get; set; }
    public bool? Active { get; set; }
    
    public void Configure(EntityTypeBuilder<MedicalRecordView> builder)
    {
        builder.ToView("vw_MedicalRecord");
        builder.HasKey(e => e.Id);
    }
}