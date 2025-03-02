using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.PatientEncounter.Procedure;

public class ProcedureFieldOptionView : IEntityTypeConfiguration<ProcedureFieldOptionView>
{
    public Guid Id { get; set; }
    public Guid FieldId { get; set; }
    public string Text { get; set; }
    public bool Archived { get; set; }
    public int References { get; set; }
    public int Order { get; set; }
    
    public void Configure(EntityTypeBuilder<ProcedureFieldOptionView> builder)
    {
        builder.ToView("vw_ProcedureFieldOption", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);
    }
}