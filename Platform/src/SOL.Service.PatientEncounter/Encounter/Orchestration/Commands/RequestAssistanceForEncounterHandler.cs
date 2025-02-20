using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class RequestAssistanceForEncounterHandler : CommandHandler<RequestAssistanceForEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public RequestAssistanceForEncounterHandler(ILogger<RequestAssistanceForEncounterHandler> logger
        , IOperationContextFactory operationContextFactory
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(RequestAssistanceForEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        encounter.RequestAssistance(_operationContext.Value.ActorId, command.Reason);

        _repository.Update(encounter);
        await _repository.Commit(stoppageToken);
    }
}
