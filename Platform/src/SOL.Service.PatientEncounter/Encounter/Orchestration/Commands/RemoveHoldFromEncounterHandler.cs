using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class RemoveHoldFromEncounterHandler : CommandHandler<RemoveHoldFromEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public RemoveHoldFromEncounterHandler(ILogger<RemoveHoldFromEncounterHandler> logger
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(RemoveHoldFromEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.RemoveHold();

        await _repository.Update(encounter, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}
