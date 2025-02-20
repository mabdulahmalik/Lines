using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.MedicalRecord.Orchestration.Commands;

public class DeleteMedicalRecordHandler : CommandHandler<DeleteMedicalRecord>
{
    private readonly IAggregateRepository<Domain.MedicalRecord> _repository;

    public DeleteMedicalRecordHandler(ILogger<DeleteMedicalRecordHandler> logger
        , IAggregateRepository<Domain.MedicalRecord> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DeleteMedicalRecord command, CancellationToken stoppageToken)
    {
        await _repository.Delete(new SingleInstanceSpecification<Domain.MedicalRecord>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}