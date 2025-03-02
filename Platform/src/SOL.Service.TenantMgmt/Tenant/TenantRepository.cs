using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.IdP;
using SOL.IdP.Phase2;

namespace SOL.Service.TenantMgmt.Tenant;

public class TenantRepository : IAggregateRepository<Domain.Tenant>
{
    private readonly List<Action> _webApiCalls = new();
    private readonly List<Domain.Tenant> _trackedTenants = new();
    
    private readonly IPhase2AdminClient _phase2AdminClient;
    private readonly IDomainBus _domainBus;

    public TenantRepository(IPhase2AdminClient phase2AdminClient, IDomainBus domainBus)
    {
        _phase2AdminClient = phase2AdminClient;
        _domainBus = domainBus;
    }

    public async Task<Domain.Tenant> Get(Guid id, CancellationToken stoppageToken = default)
    {
        var orgRep =
            await _phase2AdminClient.GetOrganizationByIdAsync(KeycloakConst.RealmId, id.ToString(), stoppageToken);

        return Domain.Tenant.Map(orgRep);
    }
    
    public async Task<bool> Exists(Guid id, CancellationToken stoppageToken = default)
    {
        var orgRep =
            await _phase2AdminClient.GetOrganizationByIdAsync(KeycloakConst.RealmId, id.ToString(), stoppageToken);

        return orgRep != null;
    }

    public Task Add(Domain.Tenant aggregateRoot, CancellationToken stoppageToken = default)
    {
        _webApiCalls.Add(async void () =>
        {
            var orgRep = new OrganizationRepresentation
            {
                Name = aggregateRoot.Key,
                DisplayName = aggregateRoot.Name
            };

            await _phase2AdminClient.CreateOrganizationAsync(orgRep, KeycloakConst.RealmId, stoppageToken);
            var orgs = await _phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId, orgRep.Name,
                cancellationToken: stoppageToken);
        
            _trackedTenants.Add(Domain.Tenant.Map(orgs.First())); 
        });
        
        return Task.CompletedTask;
    }

    public Task Update(Domain.Tenant aggregateRoot, CancellationToken stoppageToken = default)
    {
        _webApiCalls.Add(async void () =>
        {
            var orgRep = new OrganizationRepresentation
            {
                DisplayName = aggregateRoot.Name
            };

            await _phase2AdminClient.UpdateOrganizationAsync(orgRep, KeycloakConst.RealmId, aggregateRoot.Id.ToString(),
                stoppageToken);

            _trackedTenants.Add(aggregateRoot);
        });
        
        return Task.CompletedTask;
    }
    
    public async Task Commit(CancellationToken stoppageToken = default)
    {
        _webApiCalls.ForEach(call => call());

        var domainEvents = _trackedTenants
            .SelectMany(x => x.Changes)
            .OrderBy(x => x.TimeStamp)
            .ToList();
        
        foreach (var change in domainEvents.Select(x => x.Value))
            await _domainBus.DispatchAsync(change, stoppageToken);

        _trackedTenants.Clear();
        _webApiCalls.Clear();
    }

    public void Dispose()
    {
        _webApiCalls.Clear();
        _trackedTenants.Clear();
    }
    
    public Task<Domain.Tenant> Get(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default) 
        => throw new NotSupportedException();
    
    public Task<bool> Exists(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task<IReadOnlyList<Domain.Tenant>> List(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();
    
    public Task Sort(Guid id, int prevPosition, int curPosition, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();
    
    public Task Delete(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Restore(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Archive(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Unarchive(ISpecification<Domain.Tenant> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();
}