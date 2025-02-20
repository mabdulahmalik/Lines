using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Orchestration.Commands;

public class SortFacilityRoomPropertyHandler : CommandHandler<SortFacilityRoomProperty>
{
    private readonly IAggregateRepository<FacilityType.Domain.FacilityType> _repository;
    
    public SortFacilityRoomPropertyHandler(ILogger<SortFacilityRoomPropertyHandler> logger
        , IAggregateRepository<FacilityType.Domain.FacilityType> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(SortFacilityRoomProperty command, CancellationToken stoppageToken)
    {
        var facilityType = await _repository.Get(command.FacilityTypeId, stoppageToken);
        facilityType.SortProperty(command.Id, command.From, command.To);
        
        _repository.Update(facilityType);
        await _repository.Commit(stoppageToken);
    }
}