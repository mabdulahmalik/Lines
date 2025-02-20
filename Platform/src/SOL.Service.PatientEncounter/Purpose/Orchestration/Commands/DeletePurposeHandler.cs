using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Purpose.Orchestration.Commands;

public class DeletePurposeHandler : CommandHandler<DeletePurpose>
{
    private readonly IAggregateRepository<Domain.Purpose> _repository;

    public DeletePurposeHandler(ILogger<DeletePurposeHandler> logger
        , IAggregateRepository<Domain.Purpose> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DeletePurpose command, CancellationToken stoppageToken)
    {
        await _repository.Delete(new SingleInstanceSpecification<Domain.Purpose>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}