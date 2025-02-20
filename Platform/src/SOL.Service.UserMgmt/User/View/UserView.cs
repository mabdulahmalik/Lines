using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.Service.UserMgmt.User.View;

public class UserView
{
    private static List<UserView> _users = InitializeUsers();

    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public DateTime? LoggedInAt { get; set; }
    public DateTime? RegisteredAt { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    public bool? IsTraining { get; set; }
    public List<Guid> Roles { get; set; }
    public string Identity { get; set; }
    public UserPreference Preferences { get; set; }
    public bool Active { get; set; }

    public static List<UserView> GetUsers() => _users;

    private static List<UserView> InitializeUsers() =>
    [
        new UserView
        {
            Id = Guid.Parse("462b8b01-ecd7-4140-9e03-420a672ef35a"),
            FirstName = "Tony",
            LastName = "Stark",
            Email = "ironman@marvel.com",
            Avatar = null,
            LoggedInAt = DateTime.UtcNow.AddDays(-1),
            RegisteredAt = DateTime.UtcNow.AddDays(-3),
            PasswordChangedAt = DateTime.UtcNow.AddDays(-2),
            Phone = "123.456.7890",
            Identity = "email",
            IsTraining = true,
            Active = true,
            Preferences = DefaultPrefs,
            Roles = [Guid.Parse("f7d2e9a1-3c84-4b56-97a2-5e1d8c3f9b7d"), Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a")]
        },
        new UserView
        {
            Id = Guid.Parse("617ac863-0b6e-4ddf-89e2-37a630cac723"),
            FirstName = "James",
            LastName = "Rhodes",
            Email = "warmachine@marvel.com",
            Avatar = "https://i.pravatar.cc/150?img=3",
            LoggedInAt = DateTime.UtcNow.AddDays(-3),
            RegisteredAt = DateTime.UtcNow.AddDays(-5),
            PasswordChangedAt = DateTime.UtcNow.AddDays(-2),
            Phone = "555.777.4320",
            Identity = "google",
            IsTraining = false,
            Active = false,
            Preferences = DefaultPrefs,
            Roles = [Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a")]
        },
        new UserView
        {
            Id = Guid.Parse("4d54f03a-e3ab-4d1f-a6c0-393a5f1e1252"),
            FirstName = "Virginia",
            LastName = "Potts",
            Email = "pepperpotts@marvel.com",
            Avatar = "https://i.pravatar.cc/150?img=1",
            LoggedInAt = DateTime.UtcNow.AddDays(-2),
            RegisteredAt = DateTime.UtcNow.AddDays(-3),
            PasswordChangedAt = DateTime.UtcNow.AddDays(-2),
            Phone = "333.555.7777",
            Identity = "microsoft",
            IsTraining = true,
            Active = true,
            Preferences = DefaultPrefs,
            Roles = [Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a")]
        }
    ];

    private static UserPreference DefaultPrefs =>
        UserPreference.ElapsedTime | UserPreference.MilitaryTime | UserPreference.MiddleEndianDate;
}

public class UserQuery : IDomainQuery<UserView>
{
    public IQueryable<UserView> Query => UserView.GetUsers().AsQueryable();
}