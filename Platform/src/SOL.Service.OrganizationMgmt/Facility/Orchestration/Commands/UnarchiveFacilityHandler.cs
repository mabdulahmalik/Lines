using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Facility.Orchestration.Commands;

public class UnarchiveFacilityHandler : CommandHandler<UnarchiveFacility>
{
    private readonly IAggregateRepository<Domain.Facility> _repository;

    public UnarchiveFacilityHandler(ILogger<UnarchiveFacilityHandler> logger
        , IAggregateRepository<Domain.Facility> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(UnarchiveFacility command, CancellationToken stoppageToken)
    {
        await _repository.Unarchive(new SingleInstanceSpecification<Domain.Facility>(command.FacilityId), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}