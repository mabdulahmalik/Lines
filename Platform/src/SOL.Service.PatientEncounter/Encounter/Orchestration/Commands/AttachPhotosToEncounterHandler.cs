using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Abstractions.Storage;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class AttachPhotosToEncounterHandler : CommandHandler<AttachPhotosToEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IFileStorage _storage;

    public AttachPhotosToEncounterHandler(ILogger<AttachPhotosToEncounterHandler> logger
        , IAggregateRepository<Domain.Encounter> repository, IFileStorage storage
        , IOperationContextFactory operationContextFactory)
        : base(logger)
    {
        _operationContext = new(operationContextFactory.Get);
        _repository = repository;
        _storage = storage;
    }

    protected override async Task HandleAsync(AttachPhotosToEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        var photos = new List<Photo>();

        foreach (var photoData in command.Photos)
        {
            var stream = await photoData.Photo.Value;
            var contentLength = stream.Length;
            var contentType = Path.GetExtension(photoData.FileName).Trim('.').ToLower();

            var photo = new Photo(photoData.FileName, contentType, contentLength);

            photos.Add(photo);

            var cloudFilePath = $"{_operationContext.Value.TenantKey}/encounters/{encounter.Id}/{photo.Id}.{contentType}";

            await _storage.WriteAsync(cloudFilePath, stream, new Dictionary<string, string>
            {
                { "EncounterId", encounter.Id.ToString() },
                { "PhotoId", photo.Id.ToString() },
                { "FileName", photoData.FileName }
            }, stoppageToken);
        }

        encounter.AttachPhotos(photos.ToArray());

        await _repository.Update(encounter, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}