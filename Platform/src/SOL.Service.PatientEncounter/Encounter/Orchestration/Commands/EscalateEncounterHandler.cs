using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class EscalateEncounterHandler : CommandHandler<EscalateEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public EscalateEncounterHandler(ILogger<EscalateEncounterHandler> logger
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(EscalateEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.Escalate(command.Reason);

        _repository.Update(encounter);
        await _repository.Commit(stoppageToken);
    }
}
