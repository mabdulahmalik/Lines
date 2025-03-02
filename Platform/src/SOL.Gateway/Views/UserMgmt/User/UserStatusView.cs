using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.UserMgmt.User;

public class UserStatusView : IEntityTypeConfiguration<UserStatusView>
{
    public Guid UserId { get; set; }
    public UserAvailability? Status { get; set; }
    public DateTime? ChangedAt { get; set; }
    public string? Message { get; set; }

    public void Configure(EntityTypeBuilder<UserStatusView> builder)
    {
        builder.ToView("vw_UserStatus", LinesDataStore.DbSchema.UserMgmt);
        builder.HasKey(x => x.UserId);
    }
}