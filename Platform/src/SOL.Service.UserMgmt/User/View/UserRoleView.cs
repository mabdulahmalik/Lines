using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.Role.View;

namespace SOL.Service.UserMgmt.User.View;

public class UserRoleView
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }

    public static List<UserRoleView> GetUserRoles()
    {
        var roles = RoleView.GetRoles().ToDictionary(r => r.Id);
        var users = UserView.GetUsers();

        return users
            .SelectMany(user => user.Roles
                .Where(roleId => roles.ContainsKey(roleId))
                .Select(roleId => new UserRoleView
                {
                    UserId = user.Id,
                    Id = roleId,
                    Name = roles[roleId].Name
                }))
            .ToList();
    }
}

public class UserRoleQuery : IDomainQuery<UserRoleView>
{
    public UserRoleQuery()
    { }

    public IQueryable<UserRoleView> Query => UserRoleView.GetUserRoles().AsQueryable();
}