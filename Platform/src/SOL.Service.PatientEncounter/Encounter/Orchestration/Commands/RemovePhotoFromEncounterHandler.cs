using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class RemovePhotoFromEncounterHandler : CommandHandler<RemovePhotoFromEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public RemovePhotoFromEncounterHandler(ILogger<RemovePhotoFromEncounterHandler> logger
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(RemovePhotoFromEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.DetachPhotos([command.Id]);
        
        await _repository.Update(encounter, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}