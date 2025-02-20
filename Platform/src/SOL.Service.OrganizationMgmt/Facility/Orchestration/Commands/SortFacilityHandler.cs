using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Facility.Orchestration.Commands;

public class SortFacilityHandler : CommandHandler<SortFacility>
{
    private readonly IAggregateRepository<Domain.Facility> _repository;

    public SortFacilityHandler(ILogger<SortFacilityHandler> logger
        , IAggregateRepository<Domain.Facility> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(SortFacility command, CancellationToken stoppageToken)
    {
        _repository.Sort(command.Id, command.From, command.To);
        await _repository.Commit(stoppageToken);
    }
}