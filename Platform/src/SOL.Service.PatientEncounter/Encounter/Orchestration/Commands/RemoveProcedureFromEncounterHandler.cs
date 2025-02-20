using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class RemoveProcedureFromEncounterHandler : CommandHandler<RemoveProcedureFromEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public RemoveProcedureFromEncounterHandler(ILogger<RemoveProcedureFromEncounterHandler> logger
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(RemoveProcedureFromEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.UndoProcedures([command.Id]);

        _repository.Update(encounter);
        await _repository.Commit(stoppageToken);
    }
}