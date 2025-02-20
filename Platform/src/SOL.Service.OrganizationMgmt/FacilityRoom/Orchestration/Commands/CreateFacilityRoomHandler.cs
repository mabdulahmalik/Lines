using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.OrganizationMgmt.FacilityRoom.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Orchestration.Commands;

public class CreateFacilityRoomHandler : CommandHandler<CreateFacilityRoom>
{
    private readonly IAggregateRepository<Domain.FacilityRoom> _repository;

    public CreateFacilityRoomHandler(ILogger<CreateFacilityRoomHandler> logger, IAggregateRepository<Domain.FacilityRoom> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CreateFacilityRoom command, CancellationToken stoppageToken)
    {
        var properties = command.Properties.Select(x => new Property(x.PropertyId, x.OptionId)).ToArray();
        var room = Domain.FacilityRoom.Create(command.Name, command.FacilityId, properties);

        await _repository.Add(room, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}
