using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;
using RoomProperty = SOL.Service.OrganizationMgmt.FacilityType.Domain.RoomProperty;
using RoomPropertyOption = SOL.Service.OrganizationMgmt.FacilityType.Domain.RoomPropertyOption;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Orchestration.Commands;

public class CreateFacilityRoomPropertyHandler : CommandHandler<CreateFacilityRoomProperty>
{
    private readonly IAggregateRepository<FacilityType.Domain.FacilityType> _repository;

    public CreateFacilityRoomPropertyHandler(ILogger<CreateFacilityRoomPropertyHandler> logger
        , IAggregateRepository<FacilityType.Domain.FacilityType> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CreateFacilityRoomProperty command, CancellationToken stoppageToken)
    {
        var facilityType = await _repository.Get(command.FacilityTypeId, stoppageToken);
        var roomPropertyOrder = facilityType.Properties.Any()
            ? facilityType.Properties.Max(x => x.Order)
            : 0;

        var roomProperty = new RoomProperty(command.Name, roomPropertyOrder + 1);
        var options =  command.Options.Select((option, i) => new RoomPropertyOption(option, i + 1)).ToArray();
        
        roomProperty.SetOptions(options);
        facilityType.AddProperty(roomProperty);
        
        await _repository.Update(facilityType, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}
