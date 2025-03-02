using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Commands;

public class PlaceLineInternallyHandler : CommandHandler<PlaceLineInternally>
{
    private readonly IAggregateRepository<Domain.Line> _repository;
    
    public PlaceLineInternallyHandler(ILogger<PlaceLineInternallyHandler> logger, IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }
    
    protected override async Task HandleAsync(PlaceLineInternally message, CancellationToken stoppageToken)
    {
        var line = await _repository.Get(message.Id, stoppageToken);
        line.PlacedInternally();
        
        await _repository.Update(line, stoppageToken);
        await _repository.Commit(stoppageToken);        
    }
}