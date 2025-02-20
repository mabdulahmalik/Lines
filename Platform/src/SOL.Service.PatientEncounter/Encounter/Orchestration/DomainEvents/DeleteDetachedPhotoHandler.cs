using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Storage;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration;

public class DeleteDetachedPhotoHandler : DomainEventHandler<EncounterPhotosDetached>
{
    private readonly IFileStorage _fileStorage;
    private readonly Lazy<IOperationContext> _operationContext;

    public DeleteDetachedPhotoHandler(ILogger<DeleteDetachedPhotoHandler> logger, IFileStorage fileStorage, IOperationContextFactory operationContextFactory) 
        : base(logger)
    {
        _fileStorage = fileStorage;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(EncounterPhotosDetached message, CancellationToken stoppageToken)
    {
        foreach (var photo in message.Photos)
        {
            var cloudFilePath = $"{_operationContext.Value.TenantKey}/encounters/{message.EncounterId}/{photo.Id}.{photo.ContentType}";
            await _fileStorage.DeleteAsync(cloudFilePath, stoppageToken);            
        }
    }
}