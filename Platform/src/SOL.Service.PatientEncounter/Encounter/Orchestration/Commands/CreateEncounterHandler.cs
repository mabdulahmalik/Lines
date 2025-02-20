using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class CreateEncounterHandler : CommandHandler<CreateEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public CreateEncounterHandler(ILogger<CreateEncounterHandler> logger
        , IOperationContextFactory operationContextFactory
        , IAggregateRepository<Domain.Encounter> repository) 
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(CreateEncounter command, CancellationToken stoppageToken)
    {
        var encounter = Domain.Encounter.Create(command.JobId, command.FacilityRoomId, command.PurposeId);
            
        if(command.IsStat) {
            encounter.Escalate(command.StatReason);
        }

        if(command.IsOnHold) {
            encounter.PlaceOnHold(_operationContext.Value.ActorId, command.HoldReason);
        }

        await _repository.Add(encounter, stoppageToken);
        await _repository.Commit(stoppageToken);        
    }
}