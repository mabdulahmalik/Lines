using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Service.PatientEncounter.Procedure.View;

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
        builder.ToView("vw_ProcedureFieldOption");
        builder.HasKey(x => x.Id);
    }
}