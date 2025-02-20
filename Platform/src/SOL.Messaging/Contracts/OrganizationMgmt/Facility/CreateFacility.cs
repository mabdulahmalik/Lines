using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record CreateFacility : IMessage
{
    public string Name { get; init; }
    public Guid TypeId { get; init; }
    public string TimeZone { get; init; }
    public Address Address { get; init; }
    public List<RequiredPatientData> RequiredData { get; init; } = new();
}