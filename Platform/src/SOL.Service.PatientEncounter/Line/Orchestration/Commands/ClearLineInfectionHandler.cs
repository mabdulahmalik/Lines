using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Commands;

public class ClearLineInfectionHandler : CommandHandler<ClearLineInfection>
{
    private readonly IAggregateRepository<Domain.Line> _repository;
    
    public ClearLineInfectionHandler(ILogger<ClearLineInfectionHandler> logger, IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ClearLineInfection command, CancellationToken stoppageToken)
    {
        var line = await _repository.Get(command.Id, stoppageToken);
        line.ClearInfection();
        
        await _repository.Update(line, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}