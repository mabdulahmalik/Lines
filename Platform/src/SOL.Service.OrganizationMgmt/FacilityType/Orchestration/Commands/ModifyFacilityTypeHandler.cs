using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityType.Orchestration.Commands;

public class ModifyFacilityTypeHandler : CommandHandler<ModifyFacilityType>
{
    private readonly IAggregateRepository<Domain.FacilityType> _repository;
    
    public ModifyFacilityTypeHandler(ILogger<ModifyFacilityTypeHandler> logger, IAggregateRepository<Domain.FacilityType> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyFacilityType command, CancellationToken stoppageToken)
    {
        var facilityType = await _repository.Get(command.Id, stoppageToken);
        facilityType.Rename(command.Name);
        
        if (command.Active)
            facilityType.Activate();
        else
            facilityType.Deactivate();
        
        var existingProperties = facilityType.Properties.Select(x => x.Id).ToList();
        var deletableProperties =
            existingProperties.Except(command.Properties.Select(x => x.Id.GetValueOrDefault())).ToList();
        
        deletableProperties.ForEach(pid => facilityType.RemoveProperty(pid));
        
        var propertyOrder = 0;
        command.Properties.ForEach(roomProp =>
        {
            if (roomProp.Id.HasValue)
            {
                var optionOrder = 0;
                facilityType.ModifyProperty(roomProp.Id.Value, roomProp.Name, ++propertyOrder, roomProp.Options
                    .Select(x => new Domain.RoomPropertyOption(x.Id.GetValueOrDefault(Guid.NewGuid()), x.Text, ++optionOrder))
                    .ToArray());
            }
            else
            {
                facilityType.AddProperty(roomProp.Name, ++propertyOrder, roomProp.Options
                    .Select((x, i) =>
                        new Domain.RoomPropertyOption(x.Id.GetValueOrDefault(Guid.NewGuid()), x.Text, i + 1))
                    .ToArray());
            }
        });
        
        await _repository.Update(facilityType, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}