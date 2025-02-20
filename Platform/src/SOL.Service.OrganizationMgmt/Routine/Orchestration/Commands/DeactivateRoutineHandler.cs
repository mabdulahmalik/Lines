using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Commands;

public class DeactivateRoutineHandler : CommandHandler<DeactivateRoutine>
{
    private readonly IAggregateRepository<Domain.Routine> _repository;

    public DeactivateRoutineHandler(ILogger<DeactivateRoutineHandler> logger
        , IAggregateRepository<Domain.Routine> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DeactivateRoutine command, CancellationToken stoppageToken)
    {
        var routine = await _repository.Get(command.RoutineId, stoppageToken);
        routine.Deactivate();
        
        _repository.Update(routine);
        await _repository.Commit(stoppageToken);
    }    
}