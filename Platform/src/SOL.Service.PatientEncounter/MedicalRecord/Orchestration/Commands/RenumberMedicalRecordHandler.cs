using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.MedicalRecord.Orchestration.Commands;

public class RenumberMedicalRecordHandler : CommandHandler<RenumberMedicalRecord>
{
    private readonly IAggregateRepository<Domain.MedicalRecord> _repository;

    public RenumberMedicalRecordHandler(ILogger<RenumberMedicalRecordHandler> logger
        , IAggregateRepository<Domain.MedicalRecord> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(RenumberMedicalRecord command, CancellationToken stoppageToken)
    {
        var medicalRecord = await _repository.Get(command.Id, stoppageToken);
        medicalRecord.Renumber(command.Number);

        await _repository.Update(medicalRecord, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}