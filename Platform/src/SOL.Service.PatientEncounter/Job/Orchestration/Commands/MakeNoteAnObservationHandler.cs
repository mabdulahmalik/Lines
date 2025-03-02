using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.MedicalRecord.Domain;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class MakeNoteAnObservationHandler : CommandHandler<MakeNoteAnObservation>
{
    private readonly IAggregateRepository<MedicalRecord.Domain.MedicalRecord> _repository;
    private readonly IAggregateRepository<Domain.Job> _jobReader;

    public MakeNoteAnObservationHandler(ILogger<MakeNoteAnObservationHandler> logger
        , IAggregateRepository<MedicalRecord.Domain.MedicalRecord> repository
        , IAggregateRepository<Domain.Job> jobReader) 
        : base(logger)
    {
        _repository = repository;
        _jobReader = jobReader;
    }

    protected override async Task HandleAsync(MakeNoteAnObservation command, CancellationToken stoppageToken)
    {
        var job = await _jobReader.Get(command.JobId, stoppageToken);
        var medicalRecord = await _repository.Get(command.MedicalRecordId, stoppageToken);
        
        var note = job.Notes.Single(x => x.Id == command.Id);
        var observation = new Observation(note.Id, ObservationType.Note, note.CreatedAt);

        medicalRecord.AddObservations(observation);
        
        await _repository.Update(medicalRecord, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}