using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class PlaceEncounterOnHoldHandler : CommandHandler<PlaceEncounterOnHold>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public PlaceEncounterOnHoldHandler(ILogger<PlaceEncounterOnHoldHandler> logger
        , IOperationContextFactory operationContextFactory
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(PlaceEncounterOnHold command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.PlaceOnHold(_operationContext.Value.ActorId, command.Reason);

        await _repository.Update(encounter, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}