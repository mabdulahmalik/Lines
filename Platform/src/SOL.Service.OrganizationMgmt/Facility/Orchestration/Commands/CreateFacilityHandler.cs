using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.OrganizationMgmt.Facility.Orchestration.Commands;

public class CreateFacilityHandler : CommandHandler<CreateFacility>
{
    private readonly IAggregateRepository<Domain.Facility> _repository;

    public CreateFacilityHandler(ILogger<CreateFacilityHandler> logger, IAggregateRepository<Domain.Facility> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CreateFacility command, CancellationToken stoppageToken)
    {
        RequiredPatientData requiredData = default;
        command.RequiredData.ForEach(data => {
            requiredData |= data;
        });           

        var facilityType = Domain.Facility.Create(command.Name, command.TypeId, command.TimeZone, command.Address, requiredData);

        await _repository.Add(facilityType, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}