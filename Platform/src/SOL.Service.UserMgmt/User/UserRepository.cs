using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.IdP;
using SOL.IdP.Keycloak;
using SOL.Service.UserMgmt.User.Domain;
using System.Threading;

namespace SOL.Service.UserMgmt.User;

public class UserRepository : IAggregateRepository<Domain.User>
{
    private readonly List<Action> _webApiCalls = new();
    private readonly Lazy<UserMgmtDataStore> _dataStore;
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IDomainBus _domainBus;
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    private readonly IList<Domain.User> _trackedUsers = [];

    public UserRepository(IKeycloakAdminClient keycloakAdminClient, IDbContextFactory<UserMgmtDataStore> dataStoreFactory
        , IOperationContextFactory operationContextFactory,
          IDomainBus domainBus)
    {
        _keycloakAdminClient = keycloakAdminClient;
        _dataStore = new(dataStoreFactory.CreateDbContext);
        _operationContext = new(operationContextFactory.Get);
        _domainBus = domainBus;
    }

    public async Task<Domain.User> Get(Guid id, CancellationToken stoppageToken = default)
    {
        var userRep = await _keycloakAdminClient.UsersGET2Async(KeycloakConst.RealmId, id.ToString(), cancellationToken: stoppageToken);
        if (userRep == null) return null;

        var groups = await _keycloakAdminClient.GroupsAll4Async(KeycloakConst.RealmId, id.ToString(), briefRepresentation: true, cancellationToken: stoppageToken);
        var roleIds = groups.Select(x => Guid.Parse(x.Id)).ToList();
        userRep.Attributes.TryGetValue("picture", out var picture);
        userRep.Attributes.TryGetValue("phone", out var phone);

        var userProfile = await _dataStore.Value.UserProfileExts.SingleOrDefaultAsync(x => x.UserId == id, stoppageToken);

        var user = Domain.User.MapKeyclockUser(
            id: Guid.Parse(userRep.Id),
            firstName: userRep.FirstName,
            lastName: userRep.LastName,
            email: userRep.Email,
            phone: phone?.FirstOrDefault() ?? string.Empty,
            avatar: picture?.FirstOrDefault() != null ? new Uri(picture.First()) : null,
            inTraining: userProfile?.InTraining ?? false,
            roles: roleIds,
            statuses: new List<UserStatus> { new UserStatus(id, UserAvailability.Offline) },
            preference: userProfile?.Preferences ?? UserPreference.ElapsedTime,
            active: userRep.Enabled
        );

        _trackedUsers.Add(user);

        return user;
    }
    
    public Task<bool> Exists(Guid id, CancellationToken stoppageToken = default)
    {
        // TODO: call KC API to check if user exists
        throw new NotImplementedException();
    }

    public Task Update(Domain.User aggregateRoot, CancellationToken stoppageToken = default)
    {
        var userStatus = _dataStore.Value.UserStatuses
            .OrderByDescending(x => x.ChangedAt)
            .FirstOrDefault(x => x.UserId == aggregateRoot.Id);
        
        if (userStatus != aggregateRoot.Status) {
            _dataStore.Value.UserStatuses.Add(aggregateRoot.Status);
        }

        var usrProExt = _dataStore.Value.UserProfileExts
            .SingleOrDefault(x => x.UserId == aggregateRoot.Id);

        if (usrProExt == null)
        {
            var userProfileExt = new UserProfileExt {
                UserId = aggregateRoot.Id,
                InTraining = aggregateRoot.InTraining,
                Preferences = aggregateRoot.Preference
            };
            _dataStore.Value.UserProfileExts.Add(userProfileExt);
        }
        else
        {
            usrProExt.InTraining = aggregateRoot.InTraining;
            usrProExt.Preferences = aggregateRoot.Preference;
        }

        _webApiCalls.Add(async () =>
        {
            var keycloakUser = await _keycloakAdminClient.UsersGET2Async(KeycloakConst.RealmId, aggregateRoot.Id.ToString());
            if (keycloakUser == null) return;

            keycloakUser.FirstName = aggregateRoot.FirstName;
            keycloakUser.LastName = aggregateRoot.LastName;
            keycloakUser.Email = aggregateRoot.Email;
            keycloakUser.Enabled = aggregateRoot.Active;

            keycloakUser.Attributes["phone"] = new List<string> { aggregateRoot.Phone };
            keycloakUser.Attributes["picture"] = aggregateRoot.Avatar?.ToString() != null ? new List<string> { aggregateRoot.Avatar.ToString() } : new List<string>();

            await _keycloakAdminClient.UsersPUTAsync(KeycloakConst.RealmId, aggregateRoot.Id.ToString(), keycloakUser);
        });
        
        return Task.CompletedTask;
    }
    
    public async Task Commit(CancellationToken stoppageToken = default)
    {
        if (!_dataStore.IsValueCreated)
            return;

        var domainEvents = _trackedUsers
            .SelectMany(x => x.Changes)
            .OrderBy(x => x.TimeStamp)
            .ToList();

        _webApiCalls.ForEach(call => call());

        await _dataStore.Value.SaveChangesAsync(stoppageToken);

        foreach (var change in domainEvents.Select(x => x.Value))
            await _domainBus.DispatchAsync(change, stoppageToken);

        _trackedUsers.Clear();
    }
    
    public void Dispose()
    {
        if (_dataStore.IsValueCreated) {
            _dataStore.Value.Dispose();
        }
    }
    
    public Task<Domain.User> Get(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default) 
        => throw new NotSupportedException();
    
    public Task<bool> Exists(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task<IReadOnlyList<Domain.User>> List(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();
    
    public Task Sort(Guid id, int prevPosition, int curPosition, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Add(Domain.User aggregateRoot, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();
    
    public Task Delete(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Restore(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Archive(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();

    public Task Unarchive(ISpecification<Domain.User> spec, CancellationToken stoppageToken = default)
        => throw new NotSupportedException();
}