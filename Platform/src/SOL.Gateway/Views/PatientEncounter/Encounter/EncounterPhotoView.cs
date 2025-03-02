using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = System.IO.Path;

namespace SOL.Gateway.Views.PatientEncounter.Encounter;

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
        builder.ToView("EncounterPhoto", LinesDataStore.DbSchema.PatientEncounter);
        builder.Ignore(x => x.ObjectName);
        builder.HasKey(x => x.Id);
    }
}