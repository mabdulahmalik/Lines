using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.MedicalRecord.Orchestration.Commands;

public class ModifyMedicalRecordHandler : CommandHandler<ModifyMedicalRecord>
{
    private readonly IAggregateRepository<Domain.MedicalRecord> _repository;

    public ModifyMedicalRecordHandler(ILogger<ModifyMedicalRecordHandler> logger, IAggregateRepository<Domain.MedicalRecord> repository) 
        : base(logger)
    {
        _repository = repository;
    }
    protected override async Task HandleAsync(ModifyMedicalRecord message, CancellationToken stoppageToken)
    {
        var medicalRecord = await _repository.Get(message.Id!.Value, stoppageToken);
        medicalRecord.Modify(message.FirstName, message.LastName, message.Birthday);
        
        await _repository.Update(medicalRecord, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}