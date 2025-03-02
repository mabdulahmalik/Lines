using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Job.Domain;
using SOL.Service.PatientEncounter.MedicalRecord.Domain;

namespace SOL.Service.PatientEncounter.MedicalRecord.Orchestration;

public class RemoveObservationNote : DomainEventHandler<JobNotesRemoved>
{
    private readonly IAggregateRepository<Domain.MedicalRecord> _repository;

    public RemoveObservationNote(ILogger<RemoveObservationNote> logger
        , IAggregateRepository<Domain.MedicalRecord> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(JobNotesRemoved message, CancellationToken stoppageToken)
    {
        var medicalRecord =
            await _repository.Get(
                new SingleUsingObservationIdSpecification(message.NoteIds.First(), ObservationType.Note),
                stoppageToken);

        var observations = medicalRecord.Observations
            .Where(o => message.NoteIds.Contains(o.ObjectId) && o.Type == ObservationType.Note)
            .ToArray();

        medicalRecord.RemoveObservations(observations);
        
        await _repository.Update(medicalRecord, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}