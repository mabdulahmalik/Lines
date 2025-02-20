using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class DeleteJobHandler : CommandHandler<DeleteJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;

    public DeleteJobHandler(ILogger<DeleteJobHandler> logger, IAggregateRepository<Domain.Job> repository) 
        : base(logger)
    {
        _repository = repository;
    }
    
    protected override async Task HandleAsync(DeleteJob message, CancellationToken cancellationToken)
    {
        await _repository.Delete(new SingleInstanceSpecification<Domain.Job>(message.Id), cancellationToken);
        await _repository.Commit(cancellationToken);
    }
}