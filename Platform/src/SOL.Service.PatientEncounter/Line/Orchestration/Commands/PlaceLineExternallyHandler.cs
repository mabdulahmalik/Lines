using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Commands;

public class PlaceLineExternallyHandler : CommandHandler<PlaceLineExternally>
{
    private readonly IAggregateRepository<Domain.Line> _repository;

    public PlaceLineExternallyHandler(ILogger<PlaceLineExternallyHandler> logger, IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(PlaceLineExternally message, CancellationToken stoppageToken)
    {
        var line = await _repository.Get(message.Id, stoppageToken);
        line.PlacedExternally(message.PlacedOn, message.PlacedBy);
        
        _repository.Update(line);
        await _repository.Commit(stoppageToken);        
    }
}