using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.Role.View;

namespace SOL.Service.UserMgmt.Invitation.View;

public class InvitationRoleView
{
    public Guid InvitationId { get; set; }
    public Guid RoleId { get; set; }

    public static List<InvitationRoleView> GetInvitationRoles()
    {
        return InvitationView.GetInvitations()
            .SelectMany(invitation => invitation.Roles.Select(roleId =>
                new InvitationRoleView
                {
                    InvitationId = invitation.Id,
                    RoleId = roleId
                }))
            .ToList();
    }
}

public class InvitationRoleQuery : IDomainQuery<InvitationRoleView>
{
    public InvitationRoleQuery()
    { }

    public IQueryable<InvitationRoleView> Query => InvitationRoleView.GetInvitationRoles().AsQueryable();
}