using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record DeleteRoutine : IMessage
{
    public Guid Id { get; init; }
}