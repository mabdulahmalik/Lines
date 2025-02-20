using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Activities.Aggregates.Activities;

namespace SOL.Service.Facilities.Persistence.Configurations.Tables;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activity");

        builder.Property(x => x.ActivityType);
        builder.Property(x => x.AggregateType);
        builder.Property(x => x.AggregateId);
        builder.Property(x => x.Metadata);
        builder.Property(x => x.UserId);
        builder.Property(x => x.Timestamp);

        builder.HasKey(x => x.Id);
    }
}
