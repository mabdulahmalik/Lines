using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Encounter.Views;

public class EncounterPhotoView : IEntityTypeConfiguration<EncounterPhotoView>
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Length { get; set; }
    public string ObjectName => $"{Id}{Path.GetExtension(FileName)}"; 
    
    public void Configure(EntityTypeBuilder<EncounterPhotoView> builder)
    {
        builder.ToView("EncounterPhoto");
        builder.Ignore(x => x.ObjectName);
        builder.HasKey(x => x.Id);
    }
}