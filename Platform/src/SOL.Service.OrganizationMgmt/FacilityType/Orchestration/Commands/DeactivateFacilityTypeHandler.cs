using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityType.Orchestration.Commands;

public class DeactivateFacilityTypeHandler : CommandHandler<DeactivateFacilityType>
{
    private readonly IAggregateRepository<Domain.FacilityType> _repository;

    public DeactivateFacilityTypeHandler(ILogger<DeactivateFacilityTypeHandler> logger, IAggregateRepository<Domain.FacilityType> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(DeactivateFacilityType command, CancellationToken stoppageToken)
    {
        var facilityType = await _repository.Get(command.FacilityTypeId, stoppageToken);
        facilityType.Deactivate();
        
        _repository.Update(facilityType);
        await _repository.Commit(stoppageToken);
    }
}