using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;
using SOL.Service.PatientEncounter.Line.Domain;

namespace SOL.Service.PatientEncounter.Line.Orchestration;

public class EncounterRoomChangedHandler : DomainEventHandler<EncounterRoomChanged>
{
    private readonly IAggregateRepository<Domain.Line> _repository;

    public EncounterRoomChangedHandler(ILogger<EncounterRoomChangedHandler> logger, IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(EncounterRoomChanged message, CancellationToken stoppageToken)
    {
        var lines = await _repository.List(new SingleLineByEncounterSpecification(message.EncounterId), stoppageToken);
        if(!lines.Any())
            return;
        
        var line = lines.Single();
        line.Modify(line.Name, message.RoomId, line.MedicalRecordId);
        
        await _repository.Update(line, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}