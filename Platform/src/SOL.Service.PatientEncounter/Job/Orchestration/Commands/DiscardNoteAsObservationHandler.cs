using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class DiscardNoteAsObservationHandler : CommandHandler<DiscardNoteAsObservation>
{
    private readonly IAggregateRepository<MedicalRecord.Domain.MedicalRecord> _repository;

    public DiscardNoteAsObservationHandler(ILogger<DiscardNoteAsObservationHandler> logger
        , IAggregateRepository<MedicalRecord.Domain.MedicalRecord> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DiscardNoteAsObservation command, CancellationToken stoppageToken)
    {
        var medicalRecord = await _repository.Get(command.MedicalRecordId, stoppageToken);
        var observation = medicalRecord.Observations
            .Single(x => x.ObjectId == command.Id && x.Type == ObservationType.Note);
        
        medicalRecord.RemoveObservations(observation);
        
        _repository.Update(medicalRecord);
        await _repository.Commit(stoppageToken);
    }
}