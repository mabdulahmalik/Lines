using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class RescheduleJobHandler : CommandHandler<RescheduleJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public RescheduleJobHandler(ILogger<RescheduleJobHandler> logger
        , IOperationContextFactory operationContextFactory
        , IAggregateRepository<Domain.Job> repository) 
        : base(logger)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }
    
    protected override async Task HandleAsync(RescheduleJob command, CancellationToken stoppageToken)
    {
        var job = await _repository.Get(command.Id, stoppageToken);
        job.Schedule(command.Date, command.Time, command.Reason, _operationContext.Value.ActorId);
        
        _repository.Update(job);
        await _repository.Commit(stoppageToken);
    }
}