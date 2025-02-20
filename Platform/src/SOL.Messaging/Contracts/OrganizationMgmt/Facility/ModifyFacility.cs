using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.OrganizationMgmt;

public record ModifyFacility : IMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string TimeZone { get; init; }
    public Address Address { get; init; }
    public List<RequiredPatientData> RequiredData { get; init; } = new();
    public List<RoutineAssignment> Assignments { get; init; } = new();
    public List<Guid> PurposeIds { get; init; } = new();
    public List<Guid> ProcedureIds { get; init; } = new();
}

public record RoutineAssignment
{
    public Guid? Id { get; init; }
    public string Name { get; init; }
    public Guid RoutineId { get; init; }
    public List<RoutineAssignmentSpec> Specs { get; init; } = new();
}

public record RoutineAssignmentSpec
{
    public Guid PropertyId { get; init; }
    public Guid PropertyValue { get; init; }
}