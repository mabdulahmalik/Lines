using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityType.Orchestration.Commands;

public class SortFacilityTypeHandler : CommandHandler<SortFacilityType>
{
    private readonly IAggregateRepository<Domain.FacilityType> _repository;

    public SortFacilityTypeHandler(ILogger<SortFacilityTypeHandler> logger, IAggregateRepository<Domain.FacilityType> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(SortFacilityType command, CancellationToken stoppageToken)
    {
        _repository.Sort(command.Id, command.From, command.To);
        await _repository.Commit(stoppageToken);
    }
}