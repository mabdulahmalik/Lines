using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOL.Abstractions.Domain;

namespace SOL.Gateway.Views.UserMgmt.User;

public class UserView
{
    // -- Keycloak Defaults
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public DateTime? RegisteredAt { get; set; }
    public List<Guid> Roles { get; set; }
    
    // - CustomerDb
    public bool? IsTraining { get; set; }
    public bool Active { get; set; }
    public UserPreference Preferences { get; set; }
    
    // - Keycloak Customer Attributes
    public string Identity { get; set; }
    public DateTime? LoggedInAt { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    
    public static UserPreference DefaultPrefs =>
        UserPreference.ElapsedTime | UserPreference.MilitaryTime | UserPreference.MiddleEndianDate;
}

public class UserProfileExt : IEntityTypeConfiguration<UserProfileExt>
{
    public Guid UserId { get; set; }
    public bool InTraining { get; set; }
    public UserPreference Preferences { get; set; }
    
    public void Configure(EntityTypeBuilder<UserProfileExt> builder)
    {
        builder.ToTable("UserProfileExt", LinesDataStore.DbSchema.UserMgmt);

        builder.HasKey(x => x.UserId);
        builder.Property(x => x.InTraining);
        builder.Property(x => x.Preferences);
    }
}