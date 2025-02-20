using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.OrganizationMgmt.FacilityRoom.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Orchestration.Commands;

public class ModifyFacilityRoomHandler : CommandHandler<ModifyFacilityRoom>
{
    private readonly IAggregateRepository<Domain.FacilityRoom> _repository;

    public ModifyFacilityRoomHandler(ILogger<ModifyFacilityRoomHandler> logger, IAggregateRepository<Domain.FacilityRoom> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyFacilityRoom command, CancellationToken stoppageToken)
    {
        var room = await _repository.Get(command.Id, stoppageToken);
        room.Rename(command.Name);

        if (command.Properties.Any())
        {
            room.SetProperties(command.Properties
                .Select(x => new Property(x.PropertyId, x.OptionId))
                .ToArray());            
        }
        else if (room.Properties.Any())
        {
            room.ClearProperties();
        }

        _repository.Update(room);
        await _repository.Commit(stoppageToken);
    }
}