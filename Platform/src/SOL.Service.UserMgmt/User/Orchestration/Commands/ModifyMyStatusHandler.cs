using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.UserMgmt.User.Domain;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class ModifyMyStatusHandler : CommandHandler<ModifyMyStatus>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public ModifyMyStatusHandler(ILogger<ModifyMyStatusHandler> logger
        , IOperationContextFactory operationContextFactory, IAggregateRepository<Domain.User>  repository)
        : base(logger)
    {
        _operationContext = new(operationContextFactory.Get);
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyMyStatus message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(_operationContext.Value.ActorId, cancellationToken);
        user.ChangeStatus(message.Status, message.Message);

        await _repository.Update(user, cancellationToken);
        await _repository.Commit(cancellationToken);
    }
}
