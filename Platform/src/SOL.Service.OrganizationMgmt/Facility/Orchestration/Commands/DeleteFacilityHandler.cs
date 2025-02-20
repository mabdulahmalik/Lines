using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Facility.Orchestration.Commands;

public class DeleteFacilityHandler : CommandHandler<DeleteFacility>
{
    private readonly IAggregateRepository<Domain.Facility> _repository;

    public DeleteFacilityHandler(ILogger<DeleteFacilityHandler> logger
        , IAggregateRepository<Domain.Facility> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DeleteFacility command, CancellationToken stoppageToken)
    {
        await _repository.Delete(new SingleInstanceSpecification<Domain.Facility>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}