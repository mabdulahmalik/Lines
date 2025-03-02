using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.UserMgmt.User.Domain;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class ModifyMyPreferencesHandler : CommandHandler<ModifyMyPreferences>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public ModifyMyPreferencesHandler(ILogger<ModifyMyPreferencesHandler> logger
        , IOperationContextFactory operationContextFactory, IAggregateRepository<Domain.User> repository)
        : base(logger)
    {
        _operationContext = new(operationContextFactory.Get);
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyMyPreferences message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(_operationContext.Value.ActorId, cancellationToken);
        
        UserPreference userPrefs = default;
        message.Preferences?.ForEach(data => {
            userPrefs |= data;
        });
        
        user.SetPreferences(userPrefs);

        await _repository.Update(user, cancellationToken);
        await _repository.Commit(cancellationToken);
    }
}
