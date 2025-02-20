using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Service.UserMgmt.User;

public class UserProfileExt : IEntityTypeConfiguration<UserProfileExt>
{
    public Guid UserId { get; set; }
    public bool InTraining { get; set; }
    public UserPreference Preferences { get; set; }
    
    public void Configure(EntityTypeBuilder<UserProfileExt> builder)
    {
        builder.ToTable("UserProfileExt");

        builder.HasKey(x => x.UserId);
        builder.Property(x => x.InTraining);
        builder.Property(x => x.Preferences);
    }
}

class UserStatusPersistence : IEntityTypeConfiguration<Domain.UserStatus>
{
    public void Configure(EntityTypeBuilder<Domain.UserStatus> builder)
    {
        builder.ToTable("UserStatus");

        builder.HasKey(x => new { x.UserId, x.ChangedAt });
        builder.Property(x => x.Availability);
        builder.Property(x => x.ChangedAt);
        builder.Property(x => x.Message);
    }
}