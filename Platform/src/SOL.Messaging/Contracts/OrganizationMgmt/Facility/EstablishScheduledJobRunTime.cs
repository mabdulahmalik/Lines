using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record EstablishScheduledJobRunTime : IMessage
{
    public Guid FacilityId { get; init; }
    public LocalDate Date { get; init; }
    public Guid JobId { get; init; }
}