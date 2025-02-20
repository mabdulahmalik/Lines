using SOL.Abstractions.Persistence;

namespace SOL.Service.UserMgmt.Role.View;

public class RoleView
{
    private static List<RoleView> _roles = InitializeRoles();

    public Guid Id { get; set; }
    public string? Name { get; set; }

    public static List<RoleView> GetRoles() => _roles;

    private static List<RoleView> InitializeRoles() =>
    [
        new RoleView
        {
            Id = Guid.Parse("f7d2e9a1-3c84-4b56-97a2-5e1d8c3f9b7d"),
            Name = "Administrator"
        },
        new RoleView
        {
            Id = Guid.Parse("d9b4e2f3-6a77-4e6a-9fbc-8d3a2e8f2a1d"),
            Name = "Director"
        },

        new RoleView
        {
            Id = Guid.Parse("a3f5c9d7-8b42-4e1a-91f6-2d7e8a4c6b3e"),
            Name = "Owner"
        },
        new RoleView
        {
            Id = Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a"),
            Name = "Clinician"
        }
    ];
}

public class RoleQuery : IDomainQuery<RoleView>
{
    public IQueryable<RoleView> Query => RoleView.GetRoles().AsQueryable();
}