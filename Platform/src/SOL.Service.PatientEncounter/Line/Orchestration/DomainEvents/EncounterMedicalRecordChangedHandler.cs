using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;
using SOL.Service.PatientEncounter.Line.Domain;

namespace SOL.Service.PatientEncounter.Line.Orchestration;

public class EncounterMedicalRecordChangedHandler : DomainEventHandler<EncounterMedicalRecordChanged>
{
    private readonly IAggregateRepository<Domain.Line> _repository;

    public EncounterMedicalRecordChangedHandler(ILogger<EncounterMedicalRecordChangedHandler> logger
        , IAggregateRepository<Domain.Line> repository) 
        : base(logger)
    {
        _repository = repository;
    }
    
    protected override async Task HandleAsync(EncounterMedicalRecordChanged message, CancellationToken stoppageToken)
    {
        var lines = await _repository.List(new SingleLineByEncounterSpecification(message.Id), stoppageToken);
        if(!lines.Any())
            return;
        
        var line = lines.Single();
        line.Modify(line.Name, line.FacilityRoomId, message.MedicalRecordId);
        
        await _repository.Update(line, stoppageToken);
        await _repository.Commit(stoppageToken);            
    }
}