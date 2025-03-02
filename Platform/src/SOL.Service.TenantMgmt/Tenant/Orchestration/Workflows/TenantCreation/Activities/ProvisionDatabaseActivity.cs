using MassTransit;
using SOL.Messaging.Contracts.TenantMgmt;

namespace SOL.Service.TenantMgmt.Tenant.Orchestration.Workflows.TenantCreation.Activities;

public class ProvisionDatabaseActivity : IStateMachineActivity<TenantCreationState, EnactTenantCreation>
{
    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ProvisionDatabaseActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<TenantCreationState, EnactTenantCreation> context, IBehavior<TenantCreationState, EnactTenantCreation> next)
    {
        //TODO: create the tenant database based on the .dacpac file generated by the tenant database project

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<TenantCreationState, EnactTenantCreation, TException> context
        , IBehavior<TenantCreationState, EnactTenantCreation> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}