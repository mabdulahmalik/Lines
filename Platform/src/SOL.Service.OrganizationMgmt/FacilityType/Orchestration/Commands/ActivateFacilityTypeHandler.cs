using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityType.Orchestration.Commands;

public class ActivateFacilityTypeHandler : CommandHandler<ActivateFacilityType>
{
    private readonly IAggregateRepository<Domain.FacilityType> _repository;

    public ActivateFacilityTypeHandler(ILogger<ActivateFacilityTypeHandler> logger, IAggregateRepository<Domain.FacilityType> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ActivateFacilityType command, CancellationToken stoppageToken)
    {
        var facilityType = await _repository.Get(command.FacilityTypeId, stoppageToken);
        facilityType.Activate();

        await _repository.Update(facilityType, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}
