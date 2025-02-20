using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Procedure.View;

public class ProcedureView : IEntityTypeConfiguration<ProcedureView>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public RequiredPatientData RequiredData { get; set; }
    public ProcedureSetting Settings { get; set; }
    public int Order { get; set; }
    public bool? Archived { get; set; }
    public int? References { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public void Configure(EntityTypeBuilder<ProcedureView> builder)
    {
        builder.ToView("vw_Procedure");
        builder.HasKey(x => x.Id);
    }
}