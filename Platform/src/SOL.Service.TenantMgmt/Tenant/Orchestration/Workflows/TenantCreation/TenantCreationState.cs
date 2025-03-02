using MassTransit;

namespace SOL.Service.TenantMgmt.Tenant.Orchestration.Workflows.TenantCreation;

public class TenantCreationState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
}