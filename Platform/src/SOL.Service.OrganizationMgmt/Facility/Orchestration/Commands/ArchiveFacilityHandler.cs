using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Facility.Orchestration.Commands;

public class ArchiveFacilityHandler : CommandHandler<ArchiveFacility>
{
    private readonly IAggregateRepository<Domain.Facility> _repository;

    public ArchiveFacilityHandler(ILogger<ArchiveFacilityHandler> logger
        , IAggregateRepository<Domain.Facility> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ArchiveFacility command, CancellationToken stoppageToken)
    {
        await _repository.Archive(new SingleInstanceSpecification<Domain.Facility>(command.FacilityId), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}