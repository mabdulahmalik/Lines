using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SOL.Gateway.Views.Activity;

public class ActivityView : IEntityTypeConfiguration<ActivityView>
{
    public Guid? Id { get; set; }
    public string? ActivityType { get; set; }
    public string? AggregateType { get; set; }
    public Guid? AggregateId { get; set; }
    public string? Metadata { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? Timestamp { get; set; }
    
    public void Configure(EntityTypeBuilder<ActivityView> builder)
    {
        builder.ToView("Activity", "dbo");
        builder.HasNoKey();
    }
}