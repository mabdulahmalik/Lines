using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Views;

public class RoutineView : IEntityTypeConfiguration<RoutineView>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? PurposeId { get; set; }
    public RoutineSetting Settings { get; set; }
    public int? AssignmentCount { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public void Configure(EntityTypeBuilder<RoutineView> builder)
    {
        builder.ToView("vw_Routine");
        builder.HasKey(x => x.Id);
    }
}