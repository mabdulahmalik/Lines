using Dawn;
using SOL.Abstractions.Application;
using SOL.Service.UserMgmt.Invitation.View;

namespace SOL.Service.UserMgmt.Invitation.Domain.Services;

public class InvitationManager
{
    private readonly Lazy<IOperationContext> _operationContext;

    public InvitationManager(IOperationContextFactory operationContextFactory)
    {
        _operationContext = new(operationContextFactory.Get);   
    }
    
    public async Task Create(IEnumerable<string> emails, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        Guard.Argument(emails).NotNull().NotEmpty();
        Guard.Argument(roleIds).NotNull().NotEmpty();

        var inverterId = _operationContext.Value.ActorId;

        InvitationView.GetInvitations().AddRange(emails.Select(email => new InvitationView
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Email = email,
            InvitedBy = inverterId,
            Roles = roleIds.ToList()
        }));
    }
    
    public async Task Revoke(Guid invitationId, CancellationToken cancellationToken = default)
    {
        Guard.Argument(invitationId).NotDefault();
    }
    
    public async Task Resend(Guid invitationId, CancellationToken cancellationToken = default)
    {
        Guard.Argument(invitationId).NotDefault();

    }
}