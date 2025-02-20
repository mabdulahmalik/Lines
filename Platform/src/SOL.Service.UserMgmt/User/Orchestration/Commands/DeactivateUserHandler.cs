using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class DeactivateUserHandler : CommandHandler<DeactivateUser>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public DeactivateUserHandler(ILogger<DeactivateUserHandler> logger, IAggregateRepository<Domain.User> repository
        , IOperationContextFactory operationContextFactory)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(DeactivateUser message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(message.UserId, cancellationToken);

        user.Deactivate();

        _repository.Update(user);
        await _repository.Commit(cancellationToken);
    }
}
