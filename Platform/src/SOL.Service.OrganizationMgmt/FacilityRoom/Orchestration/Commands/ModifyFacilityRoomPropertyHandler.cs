using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;
using Domain_RoomPropertyOption = SOL.Service.OrganizationMgmt.FacilityType.Domain.RoomPropertyOption;
using FacilityType_Domain_RoomPropertyOption = SOL.Service.OrganizationMgmt.FacilityType.Domain.RoomPropertyOption;
using FacilityTypes_RoomPropertyOption = SOL.Service.OrganizationMgmt.FacilityType.Domain.RoomPropertyOption;
using RoomPropertyOption = SOL.Service.OrganizationMgmt.FacilityType.Domain.RoomPropertyOption;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Orchestration.Commands;

public class ModifyFacilityRoomPropertyHandler : CommandHandler<ModifyFacilityRoomProperty>
{
    private readonly IAggregateRepository<FacilityType.Domain.FacilityType> _repository;

    public ModifyFacilityRoomPropertyHandler(ILogger<ModifyFacilityRoomPropertyHandler> logger
        , IAggregateRepository<FacilityType.Domain.FacilityType> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyFacilityRoomProperty command, CancellationToken stoppageToken)
    {
        var sortOrder = 0;
        var facilityType = await _repository.Get(command.FacilityTypeId, stoppageToken);
        
        facilityType.ModifyProperty(command.Id, command.Name, command.Options
            .Select(x => new FacilityType_Domain_RoomPropertyOption(x.Id.GetValueOrDefault(Guid.NewGuid()), x.Text, ++sortOrder))
            .ToArray());
        
        await _repository.Update(facilityType, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}