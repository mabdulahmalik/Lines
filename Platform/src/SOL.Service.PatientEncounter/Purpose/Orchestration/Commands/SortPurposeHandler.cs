using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Purpose.Orchestration.Commands;

public class SortPurposeHandler : CommandHandler<SortPurpose>
{
    private readonly IAggregateRepository<Domain.Purpose> _repository;

    public SortPurposeHandler(ILogger<SortPurposeHandler> logger
        , IAggregateRepository<Domain.Purpose> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(SortPurpose command, CancellationToken stoppageToken)
    {
        _repository.Sort(command.Id, command.From, command.To);
        await _repository.Commit(stoppageToken);
    }
}