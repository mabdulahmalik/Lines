using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.UserMgmt.Invitation.Domain.Services;

namespace SOL.Service.UserMgmt.Invitation.Orchestration.Commands;

public class ResendlUserInviteHandler : CommandHandler<ResendUserInvite>
{
    private readonly InvitationManager _invitationManager;
    private readonly Lazy<IOperationContext> _operationContext;

    public ResendlUserInviteHandler(ILogger<ResendlUserInviteHandler> logger, InvitationManager invitationManager
        , IOperationContextFactory operationContextFactory)
        : base(logger)
    {
        _invitationManager = invitationManager;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(ResendUserInvite message, CancellationToken cancellationToken)
    {
        await _invitationManager.Resend(message.InviteId, cancellationToken);
    }
}