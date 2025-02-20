using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.UserMgmt.Invitation.Domain.Services;
using SOL.Service.UserMgmt.User.Orchestration.Commands;

namespace SOL.Service.UserMgmt.Invitation.Orchestration.Commands;

public class CancelUserInviteHandler : CommandHandler<CancelUserInvite>
{
    private readonly InvitationManager _invitationManager;
    private readonly Lazy<IOperationContext> _operationContext;

    public CancelUserInviteHandler(ILogger<CancelUserInviteHandler> logger, InvitationManager invitationManager
        , IOperationContextFactory operationContextFactory)
        : base(logger)
    {
        _invitationManager = invitationManager;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(CancelUserInvite message, CancellationToken cancellationToken)
    {
        await _invitationManager.Revoke(message.InviteId, cancellationToken);
    }
}