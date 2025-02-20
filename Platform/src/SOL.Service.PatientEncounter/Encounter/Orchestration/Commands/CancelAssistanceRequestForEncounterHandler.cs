using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class CancelAssistanceRequestForEncounterHandler : CommandHandler<CancelAssistanceRequestForEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public CancelAssistanceRequestForEncounterHandler(ILogger<CancelAssistanceRequestForEncounterHandler> logger
        , IOperationContextFactory operationContextFactory
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(CancelAssistanceRequestForEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.CancelAssistance();

        _repository.Update(encounter);
        await _repository.Commit(stoppageToken);
    }
}