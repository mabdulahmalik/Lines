using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.PatientEncounter.Procedure;

public class ProcedureView : IEntityTypeConfiguration<ProcedureView>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public ProcedureType? Type { get; set; }
    public RequiredPatientData RequiredData { get; set; }
    public ProcedureSetting Settings { get; set; }
    public int Order { get; set; }
    public bool? Archived { get; set; }
    public int? References { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public void Configure(EntityTypeBuilder<ProcedureView> builder)
    {
        builder.ToView("vw_Procedure", LinesDataStore.DbSchema.PatientEncounter);
        builder.HasKey(x => x.Id);
    }
}