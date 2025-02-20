using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Job.Views;

public class JobNoteView : IEntityTypeConfiguration<JobNoteView>
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } 
    public JobNoteTreatment Treatment { get; set; }
    
    public void Configure(EntityTypeBuilder<JobNoteView> builder)
    {
        builder.ToView("JobNote");
        builder.HasKey(x => x.Id);
    }
}