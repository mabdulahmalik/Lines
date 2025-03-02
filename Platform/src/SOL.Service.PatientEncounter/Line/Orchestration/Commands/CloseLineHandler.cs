using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Commands;

public class CloseLineHandler : CommandHandler<CloseLine>
{
    private readonly IAggregateRepository<Domain.Line> _repository;

    public CloseLineHandler(ILogger<CloseLineHandler> logger, IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CloseLine message, CancellationToken stoppageToken)
    {
        var line = await _repository.Get(message.Id, stoppageToken);
        line.Close(message.RemovedOn);
        
        await _repository.Update(line, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}