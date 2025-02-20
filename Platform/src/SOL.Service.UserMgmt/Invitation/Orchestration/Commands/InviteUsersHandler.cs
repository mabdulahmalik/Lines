using Microsoft.Extensions.Logging;
using NodaTime;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.UserMgmt.Invitation.Domain.Services;
using SOL.Service.UserMgmt.User.Orchestration.Commands;

namespace SOL.Service.UserMgmt.Invitation.Orchestration.Commands;

public class InviteUsersHandler : CommandHandler<InviteUsers>
{
    private readonly InvitationManager _invitationManager;
    private readonly Lazy<IOperationContext> _operationContext;

    public InviteUsersHandler(ILogger<InviteUsersHandler> logger, InvitationManager invitationManager
        , IOperationContextFactory operationContextFactory)
        : base(logger)
    {
        _invitationManager = invitationManager;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(InviteUsers message, CancellationToken cancellationToken)
    {
        await _invitationManager.Create(message.Emails, message.Roles, cancellationToken);
    }
}
