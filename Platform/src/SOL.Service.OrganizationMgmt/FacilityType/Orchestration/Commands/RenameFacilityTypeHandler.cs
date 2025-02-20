using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityType.Orchestration.Commands;

public class RenameFacilityTypeHandler : CommandHandler<RenameFacilityType>
{
    private readonly IAggregateRepository<Domain.FacilityType> _repository;

    public RenameFacilityTypeHandler(ILogger<RenameFacilityTypeHandler> logger, IAggregateRepository<Domain.FacilityType> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(RenameFacilityType command, CancellationToken stoppageToken)
    {
        var facilityType = await _repository.Get(command.Id, stoppageToken);
        facilityType.Rename(command.Name);
        
        _repository.Update(facilityType);
        await _repository.Commit(stoppageToken);
    }
}