using MassTransit;
using SOL.Messaging.Contracts.TenantMgmt;
using SOL.Service.TenantMgmt.Tenant.Orchestration.Workflows.TenantCreation.Activities;

namespace SOL.Service.TenantMgmt.Tenant.Orchestration.Workflows.TenantCreation;

public class TenantCreationStateMachine : MassTransitStateMachine<TenantCreationState>
{
    public Event<EnactTenantCreation> CreationStarted { get; set; }
    
    public TenantCreationStateMachine()
    {
        InstanceState(x => x.CurrentState);
        Event(() => CreationStarted, c => c.CorrelateById(z => z.CorrelationId!.Value));
        
        Initially(
            When(CreationStarted)
                .Activity(act => act.OfType<ProvisionOrganizationActivity>())
                .Activity(act => act.OfType<ProvisionDatabaseActivity>())    
                .Activity(act => act.OfType<ProvisionStorageActivity>())
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}