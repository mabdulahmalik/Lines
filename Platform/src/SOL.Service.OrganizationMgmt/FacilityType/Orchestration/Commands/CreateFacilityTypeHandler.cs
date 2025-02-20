using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.FacilityType.Orchestration.Commands;

public class CreateFacilityTypeHandler : CommandHandler<CreateFacilityType>
{
    private readonly IAggregateRepository<Domain.FacilityType> _repository;

    public CreateFacilityTypeHandler(ILogger<CreateFacilityTypeHandler> logger, IAggregateRepository<Domain.FacilityType> repository)   
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CreateFacilityType command, CancellationToken stoppageToken)
    {
        var facilityType = Domain.FacilityType.Create(command.Name);
        await _repository.Add(facilityType, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}