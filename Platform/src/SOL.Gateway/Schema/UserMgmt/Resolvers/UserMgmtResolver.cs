using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Application;
using SOL.Gateway.Views.UserMgmt.Invitation;
using SOL.Gateway.Views.UserMgmt.Role;
using SOL.Gateway.Views.UserMgmt.User;
using SOL.IdP;
using SOL.IdP.Keycloak;
using SOL.IdP.Phase2;

namespace SOL.Gateway.Schema.UserMgmt.Resolvers;

public class UserMgmtResolver
{
    public async Task<IEnumerable<RoleView>> Roles(IOperationContextFactory operationContextFactory
        , IKeycloakAdminClient keycloakAdminClient, int? skip, int? take, CancellationToken cancellationToken)
    {
        var optCtx = operationContextFactory.Get();
        var groupRep = await keycloakAdminClient
            .GroupByPathAsync($"/tenants/{optCtx.TenantKey}/Roles", KeycloakConst.RealmId, cancellationToken);
        
        var groups = await keycloakAdminClient
            .ChildrenAllAsync(KeycloakConst.RealmId, groupRep.Id, briefRepresentation: true, first: skip,
                max: take, cancellationToken: cancellationToken);

        return groups.Select(gr => new RoleView
        {
            Id = Guid.Parse(gr.Id),
            Name = gr.Name
        });
    }
    
    public async Task<IEnumerable<InvitationView>> UserInvitations(IOperationContextFactory operationContextFactory
        , IPhase2AdminClient phase2AdminClient, int? skip, int? take, CancellationToken cancellationToken)
    {
        var optCtx = operationContextFactory.Get();
        var orgs = await phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId, search: optCtx.TenantKey,
            cancellationToken: cancellationToken);
        var org = orgs.First();

        var invitations = await phase2AdminClient
            .GetOrganizationInvitationsAsync(KeycloakConst.RealmId, org.Id, first: skip, max: take,
                cancellationToken: cancellationToken);

        return invitations.Select(ir => new InvitationView
        {
            Email = ir.Email,
            Id = Guid.Parse(ir.Id),
            InvitedBy = Guid.Parse(ir.InviterId),
            CreatedAt = DateTime.Parse(ir.CreatedAt),
            Roles = ir.Roles.Select(Guid.Parse).ToList()
        });
    }

    public async Task<IEnumerable<UserView>> Users(IOperationContextFactory operationContextFactory
        , IKeycloakAdminClient keycloakAdminClient, IDbContextFactory<LinesDataStore> dbContextFactory
        , int? skip, int? take, CancellationToken cancellationToken)
    {
        var userViews = new List<UserView>();
        var roles = await Roles(operationContextFactory, keycloakAdminClient
            , null, null, cancellationToken);
        
        foreach (var roleView in roles)
        {
            var members = await keycloakAdminClient.MembersAllAsync(KeycloakConst.RealmId, roleView.Id.ToString()
                    , first: skip, max: take, briefRepresentation: false
                    , cancellationToken: cancellationToken);

            foreach (var usr in members)
            {
                var userId = Guid.Parse(usr.Id);
                if (userViews.All(v => v.Id != userId))
                {
                    usr.Attributes.TryGetValue("picture", out var picture);
                    usr.Attributes.TryGetValue("phone", out var phone);
                        
                    userViews.Add(new UserView
                    {
                        Id = userId,
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        Email = usr.Email,
                        Avatar = picture?.FirstOrDefault(),
                        Phone = phone?.FirstOrDefault(),
                        Active = usr.Enabled,
                        RegisteredAt = DateTime.UnixEpoch.AddMilliseconds(usr.CreatedTimestamp),
                        Identity = "email",
                        IsTraining = false,
                        Preferences = UserView.DefaultPrefs,
                        Roles = [roleView.Id]
                        //LoggedInAt = usr.LoggedInAt, 
                        //PasswordChangedAt = usr.PasswordChangedAt
                    });
                }
                else
                {
                    var userView = userViews.First(v => v.Id == userId);
                    userView.Roles.Add(roleView.Id);
                }   
            }
        }

        if (!userViews.Any())
            return userViews;
        
        await using var dbCtx = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var userIds = userViews.Select(x => x.Id).ToList();
        dbCtx.UserProfileExts.Where(x => userIds.Contains(x.UserId)).ToList().ForEach(up =>
        {
            var userView = userViews.First(u => u.Id == up.UserId);
            userView.IsTraining = up.InTraining;
            userView.Preferences = up.Preferences;
        });
        
        return userViews;
    }

    public async Task<UserView> Me(IOperationContextFactory operationContextFactory
        , IKeycloakAdminClient keycloakAdminClient, IDbContextFactory<LinesDataStore> dbContextFactory
        , CancellationToken cancellationToken)
    {
        var optCtx = operationContextFactory.Get();
        var user = await keycloakAdminClient.UsersGET2Async(KeycloakConst.RealmId, optCtx.ActorId.ToString()
            , cancellationToken: cancellationToken);
        var groups = await keycloakAdminClient.GroupsAll4Async(KeycloakConst.RealmId, optCtx.ActorId.ToString(),
            briefRepresentation: true, cancellationToken: cancellationToken);

        var roleIds = groups.Select(x => Guid.Parse(x.Id)).ToList();
        
        var userView = new UserView
        {
            Id = Guid.Parse(user.Id),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Avatar = user.Attributes.TryGetValue("picture", out var picture) ? picture.FirstOrDefault() : null,
            Phone = user.Attributes.TryGetValue("phone", out var phone) ? phone.FirstOrDefault() : null,
            Active = user.Enabled,
            RegisteredAt = DateTime.UnixEpoch.AddMilliseconds(user.CreatedTimestamp),
            Identity = "email",
            IsTraining = false,
            Preferences = UserView.DefaultPrefs,
            Roles = roleIds
            //LoggedInAt = user.LoggedInAt, 
            //PasswordChangedAt = user.PasswordChangedAt
        };

        await using var dbCtx = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var userProf =
            await dbCtx.UserProfileExts.SingleOrDefaultAsync(x => x.UserId == userView.Id,
                cancellationToken: cancellationToken);
        
        if (userProf != null)
        {
            userView.IsTraining = userProf.InTraining;
            userView.Preferences = userProf.Preferences;
        }
        
        return userView;
    }
}