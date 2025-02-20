using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Commands;

public class DeleteRoutineHandler : CommandHandler<DeleteRoutine>
{
    private readonly IAggregateRepository<Domain.Routine> _repository;

    public DeleteRoutineHandler(ILogger<DeleteRoutineHandler> logger
        , IAggregateRepository<Domain.Routine> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DeleteRoutine command, CancellationToken stoppageToken)
    {
        await _repository.Delete(new SingleInstanceSpecification<Domain.Routine>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}