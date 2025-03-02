using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.TenantMgmt;
using SOL.Service.TenantMgmt.Tenant.Domain.Services;

namespace SOL.Service.TenantMgmt.Tenant.Orchestration.Workflows.TenantCreation.Activities;

public class ProvisionOrganizationActivity : IStateMachineActivity<TenantCreationState, EnactTenantCreation>
{
    private readonly IAggregateRepository<Domain.Tenant> _repository;
    private readonly TenantManager _tenantManager;

    public ProvisionOrganizationActivity(IAggregateRepository<Domain.Tenant> repository, TenantManager tenantManager)
    {
        _repository = repository;
        _tenantManager = tenantManager;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ProvisionOrganizationActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<TenantCreationState, EnactTenantCreation> context, IBehavior<TenantCreationState, EnactTenantCreation> next)
    {
        var tenant = Domain.Tenant.Create(context.Message.Name, context.Message.Key);

        await _repository.Add(tenant);
        await _repository.Commit(context.CancellationToken);
        
        await _tenantManager.EstablishDefaultRoles(tenant.Key, context.CancellationToken);
    
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<TenantCreationState, EnactTenantCreation, TException> context, IBehavior<TenantCreationState, EnactTenantCreation> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}