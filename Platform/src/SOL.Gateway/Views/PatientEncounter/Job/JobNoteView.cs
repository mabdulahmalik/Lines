using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Job;

public class JobNoteView : IEntityTypeConfiguration<JobNoteView>
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } 
    public bool IsPinned { get; set; }
    
    public void Configure(EntityTypeBuilder<JobNoteView> builder)
    {
        builder.ToView("JobNote", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);
    }
}