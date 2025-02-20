using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class ActivateUserHandler : CommandHandler<ActivateUser>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public ActivateUserHandler(ILogger<ActivateUserHandler> logger, IAggregateRepository<Domain.User> repository
        , IOperationContextFactory operationContextFactory)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(ActivateUser message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(message.UserId, cancellationToken);

        user.Activate();

        _repository.Update(user);
        await _repository.Commit(cancellationToken);
    }
}
