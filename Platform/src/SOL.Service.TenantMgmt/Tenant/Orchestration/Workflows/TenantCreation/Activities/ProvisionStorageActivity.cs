using MassTransit;
using SOL.Abstractions.Storage;
using SOL.Messaging.Contracts.TenantMgmt;

namespace SOL.Service.TenantMgmt.Tenant.Orchestration.Workflows.TenantCreation.Activities;

public class ProvisionStorageActivity : IStateMachineActivity<TenantCreationState, EnactTenantCreation>
{
    private readonly IFileStorage _fileStorage;

    public ProvisionStorageActivity(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ProvisionStorageActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<TenantCreationState, EnactTenantCreation> context, IBehavior<TenantCreationState, EnactTenantCreation> next)
    {
        await _fileStorage.Create(context.Message.Key.ToLower());
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<TenantCreationState, EnactTenantCreation, TException> context
        , IBehavior<TenantCreationState, EnactTenantCreation> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}