using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Abstractions.Storage;
using SOL.Messaging.Contracts.UserMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.Commands;

public class ModifyMyAvatarHandler : CommandHandler<ModifyMyAvatar>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IFileStorage _storage;

    public ModifyMyAvatarHandler(ILogger<ModifyMyAvatarHandler> logger, IAggregateRepository<Domain.User> repository
        , IOperationContextFactory operationContextFactory, IFileStorage storage)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
        _storage = storage;
    }

    protected override async Task HandleAsync(ModifyMyAvatar message, CancellationToken cancellationToken)
    {
        var user = await _repository.Get(_operationContext.Value.ActorId, cancellationToken);
                
        var url = await UploadAvatar(message, cancellationToken);

        var avatarUri = url != null ? new Uri(url, UriKind.Absolute) : null;

        user.Modify(user.FirstName, user.LastName, user.Phone, avatarUri);

        await _repository.Update(user, cancellationToken);
        await _repository.Commit(cancellationToken);
    }

    private async Task<string?> UploadAvatar(ModifyMyAvatar avatarData, CancellationToken cancellationToken)
    {
        if (!avatarData.Avatar.HasValue) return null;

        var userId = _operationContext.Value.ActorId;

        var stream = await avatarData.Avatar.Value;
        var contentType = Path.GetExtension(avatarData.FileName).Trim('.').ToLower();

        var basePath = $"{_operationContext.Value.TenantKey}";
        var cloudFilePath = $"avatars/{userId}/{Guid.NewGuid()}.{contentType}";

        await _storage.WriteAsync($"{basePath}/{cloudFilePath}", stream, null, cancellationToken);

        return _storage.GetAbsoluteUrl(cloudFilePath);
    }
}