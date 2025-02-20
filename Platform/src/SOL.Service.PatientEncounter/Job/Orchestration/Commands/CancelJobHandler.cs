using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class CancelJobHandler : CommandHandler<CancelJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public CancelJobHandler(ILogger<CancelJobHandler> logger, IAggregateRepository<Domain.Job> repository
        , IOperationContextFactory operationContextFactory) 
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(CancelJob message, CancellationToken cancellationToken)
    {
        var job = await _repository.Get(message.Id, cancellationToken);
        job.Cancel(_operationContext.Value.ActorId, message.Reason);
        
        _repository.Update(job);
        await _repository.Commit(cancellationToken);
    }
}