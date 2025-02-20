using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Activities.Views;

namespace SOL.Activities.Persistence.Configurations.Views;

class ActivityViewConfiguration : IEntityTypeConfiguration<ActivityView>
{
    public void Configure(EntityTypeBuilder<ActivityView> builder)
    {
        builder.ToView("Activity");

        builder.HasNoKey();

        builder.Property(x => x.ActivityType);
        builder.Property(x => x.AggregateType);
        builder.Property(x => x.AggregateId);
        builder.Property(x => x.Metadata);
        builder.Property(x => x.UserId);
        builder.Property(x => x.Timestamp);
    }
}