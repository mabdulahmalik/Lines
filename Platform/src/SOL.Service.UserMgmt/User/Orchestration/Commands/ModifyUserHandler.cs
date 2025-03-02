using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Abstractions.Storage;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class ModifyUserHandler : CommandHandler<ModifyUser>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IFileStorage _storage;

    public ModifyUserHandler(ILogger<ModifyUserHandler> logger, IAggregateRepository<Domain.User> repository
        , IOperationContextFactory operationContextFactory, IFileStorage storage)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
        _storage = storage;
    }

    protected override async Task HandleAsync(ModifyUser message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(message.Id, cancellationToken);

        user.Modify(message.FirstName, message.LastName, message.Phone, user.Avatar);

        if (message.InTraining)
        {
            user.EnrollTraining();
        }
        else
        {
            user.WithdrawTraining();
        }

        var newRoles = message.Roles.Where(x=> !user.Roles.Contains(x));

        if (newRoles.Any()) 
        {
            user.AddRole(newRoles.ToArray());
        }

        var rolesToRemove = user.Roles.Where(x => !message.Roles.Contains(x));

        if (rolesToRemove.Any())
        { 
            user.RemoveRole(rolesToRemove.ToArray());
        }

        await _repository.Update(user, cancellationToken);
        await _repository.Commit(cancellationToken);
    }
}
