using Dawn;
using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;
using SOL.IdP;
using SOL.IdP.Phase2;
using SOL.Service.UserMgmt.Invitation.Domain.Events;

namespace SOL.Service.UserMgmt.Invitation.Domain.Services;

public class InvitationManager
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IPhase2AdminClient _phase2AdminClient;
    private readonly IDomainBus _domainBus;

    public InvitationManager(IOperationContextFactory operationContextFactory, IPhase2AdminClient phase2AdminClient, IDomainBus domainBus)
    {
        _operationContext = new(operationContextFactory.Get);
        _phase2AdminClient = phase2AdminClient;
        _domainBus = domainBus;
    }

    public async Task Create(IEnumerable<string> emails, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        Guard.Argument(emails).NotNull().NotEmpty();
        Guard.Argument(roleIds).NotNull().NotEmpty();

        var orgs = await _phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId,
            search: _operationContext.Value.TenantKey, cancellationToken: cancellationToken);

        var tasks = new List<Task>();
        var roles = roleIds.Select(x => x.ToString()).ToList();
        var invitedBy = _operationContext.Value.ActorId.ToString();
        
        foreach (var email in emails)
        {
            tasks.Add(_phase2AdminClient
                .AddOrganizationInvitationAsync(new InvitationRequestRepresentation
                {
                    Send = true,
                    Email = email,
                    //Roles = roles,
                    //Attributes = new Dictionary<string, ICollection<string>>
                    //{
                    //    {"roles", roles}
                    //},
                    InviterId = invitedBy,
                    RedirectUri = KeycloakConst.RegistrationUrl
                }, KeycloakConst.RealmId,
                orgs.First().Id, cancellationToken));
        }

        await Task.WhenAll(tasks);

        await _domainBus.DispatchAsync(new UserInvitationsSent(emails), cancellationToken);
    }
    
    public async Task Revoke(Guid invitationId, CancellationToken cancellationToken = default)
    {
        Guard.Argument(invitationId).NotDefault();

        var orgs = await _phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId,
            search: _operationContext.Value.TenantKey, cancellationToken: cancellationToken);

        await _phase2AdminClient.RemoveOrganizationInvitationAsync(KeycloakConst.RealmId
            , orgs.First().Id, invitationId.ToString()
            , cancellationToken);

        await _domainBus.DispatchAsync(new UserInvitationRevoked(invitationId), cancellationToken);
    }
    
    public async Task Resend(Guid invitationId, CancellationToken cancellationToken = default)
    {
        Guard.Argument(invitationId).NotDefault();
        
        var orgs = await _phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId,
            search: _operationContext.Value.TenantKey, cancellationToken: cancellationToken);

        await _phase2AdminClient.ResendOrganizationInvitationAsync(KeycloakConst.RealmId
            , orgs.First().Id, invitationId.ToString()
            , cancellationToken);

        await _domainBus.DispatchAsync(new UserInvitationResent(invitationId), cancellationToken);
    }
}