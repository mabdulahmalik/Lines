using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.Service.UserMgmt.User.View;

public class UserStatusView : IEntityTypeConfiguration<UserStatusView>
{
    public Guid UserId { get; set; }
    public UserAvailability? Status { get; set; }
    public DateTime? ChangedAt { get; set; }
    public string? Message { get; set; }

    public void Configure(EntityTypeBuilder<UserStatusView> builder)
    {
        builder.ToView("vw_UserStatus");
        builder.HasKey(x => x.UserId);
    }
}

public class UserStatusQuery : IDomainQuery<UserStatusView>
{
    private readonly Lazy<UserMgmtDataStore> _dataStore;

    public UserStatusQuery(IDbContextFactory<UserMgmtDataStore> dataStoreFactory)
    {
        _dataStore = new(dataStoreFactory.CreateDbContext);
    }

    public IQueryable<UserStatusView> Query => _dataStore.Value.Set<UserStatusView>();
}