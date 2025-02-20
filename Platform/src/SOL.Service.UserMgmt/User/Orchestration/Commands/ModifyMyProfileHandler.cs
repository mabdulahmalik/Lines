using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Abstractions.Storage;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class ModifyMyProfileHandler : CommandHandler<ModifyMyProfile>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IFileStorage _storage;

    public ModifyMyProfileHandler(ILogger<ModifyMyProfileHandler> logger, IAggregateRepository<Domain.User> repository
        , IOperationContextFactory operationContextFactory, IFileStorage storage)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
        _storage = storage;
    }

    protected override async Task HandleAsync(ModifyMyProfile message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(_operationContext.Value.ActorId, cancellationToken);

        user.Modify(message.FirstName, message.LastName, message.Phone, user.Avatar);

        _repository.Update(user);
        await _repository.Commit(cancellationToken);
    }
}
