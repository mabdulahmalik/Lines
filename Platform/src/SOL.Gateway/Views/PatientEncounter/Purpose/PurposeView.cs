using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.PatientEncounter.Purpose;

public class PurposeView : IEntityTypeConfiguration<PurposeView>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Order { get; set; }
    public bool? Archived { get; set; }
    public int? References { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public void Configure(EntityTypeBuilder<PurposeView> builder)
    {
        builder.ToView("vw_Purpose", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);
    }
}