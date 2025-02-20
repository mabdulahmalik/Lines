using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Line.Orchestration.Commands;

public class CreateLineHandler : CommandHandler<CreateLine>
{
    private readonly IAggregateRepository<Domain.Line> _repository;

    public CreateLineHandler(ILogger<CreateLineHandler> logger, IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CreateLine command, CancellationToken stoppageToken)
    {
        var line = Domain.Line.Create(command.RoomId, command.Name, command.Type
            , command.InsertedOn, command.InsertedWith, command.ExternallyPlaced
            , command.ExternallyPlacedBy);
        
        if (command.EncounterIds.Any())
            line.AttachEncounters(command.EncounterIds.ToArray());
        
        await _repository.Add(line, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}