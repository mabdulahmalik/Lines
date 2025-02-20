using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Purpose.Orchestration.Commands;

public class ArchivePurposeHandler : CommandHandler<ArchivePurpose>
{
    private readonly IAggregateRepository<Domain.Purpose> _repository;

    public ArchivePurposeHandler(ILogger<ArchivePurposeHandler> logger
        , IAggregateRepository<Domain.Purpose> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ArchivePurpose command, CancellationToken stoppageToken)
    {
        await _repository.Archive(new SingleInstanceSpecification<Domain.Purpose>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}