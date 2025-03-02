using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Procedure;

public class ProcedureFieldView : IEntityTypeConfiguration<ProcedureFieldView>
{
    public Guid Id { get; set; }
    public Guid ProcedureId { get; set; }
    public string Name { get; set; }
    public string? Instruction { get; set; }
    public ProcedureFieldType Type { get; set; }
    public ProcedureFieldSetting Settings { get; set; }
    public bool Archived { get; set; }
    public int References { get; set; }
    public int Order { get; set; }
    
    public void Configure(EntityTypeBuilder<ProcedureFieldView> builder)
    {
        builder.ToView("vw_ProcedureField", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasNoKey();
    }
}
