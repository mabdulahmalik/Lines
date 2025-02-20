using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.Invitation.View;

namespace SOL.Gateway.Schema.UserMgmt.Loaders;

public class InvitationRoleLoader : GroupedDataLoader<Guid, Guid>
{
    private readonly IDomainQuery<InvitationRoleView> _invitationRoleQuery;

    public InvitationRoleLoader(IDomainQuery<InvitationRoleView> userStatusQuery
        , IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _invitationRoleQuery = userStatusQuery;
    }

    protected override async Task<ILookup<Guid, Guid>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var roles = _invitationRoleQuery.Query
            .Where(x => keys.Contains(x.InvitationId))
            .Select(x => new { x.InvitationId, x.RoleId })
            .ToList();

        return roles.ToLookup(x => x.InvitationId, y => y.RoleId);
    }
}