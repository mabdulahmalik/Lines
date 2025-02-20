using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.Service.UserMgmt.User.View;

namespace SOL.Service.UserMgmt.User;

public class UserRepository : IAggregateRepository<Domain.User>
{
    private readonly List<Action> _webApiCalls = new();
    private readonly Lazy<UserMgmtDataStore> _dataStore;
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IDomainBus _domainBus;

    private readonly IList<Domain.User> _trackedUsers = [];

    public UserRepository(IDbContextFactory<UserMgmtDataStore> dataStoreFactory
        , IOperationContextFactory operationContextFactory,
          IDomainBus domainBus)
    {
        _dataStore = new(dataStoreFactory.CreateDbContext);
        _operationContext = new(operationContextFactory.Get);
        _domainBus = domainBus;
    }

    public async Task<Domain.User> Get(Guid id, CancellationToken stoppageToken = default)
    {
        //Get user from KC API
        var apiResponse = UserView.GetUsers().Single(x => x.Id == id);

        var statuses = await _dataStore.Value.UserStatuses
            .Where(x => x.UserId == id).ToListAsync(stoppageToken);

        var profileExt = await _dataStore.Value.UserProfileExts
            .FirstOrDefaultAsync(x => x.UserId == id, stoppageToken);

        var avatar = !String.IsNullOrWhiteSpace(apiResponse.Avatar)
            ? new Uri(apiResponse.Avatar, UriKind.Absolute)
            : null;

        var user = Domain.User.MapKeyclockUser(id, 
            apiResponse.FirstName, 
            apiResponse.LastName, 
            apiResponse.Email, 
            apiResponse.Phone,
            avatar,
            profileExt.InTraining,
            apiResponse.Roles, 
            statuses, 
            profileExt?.Preferences ?? UserPreference.ElapsedTime 
                | UserPreference.MilitaryTime 
                | UserPreference.MiddleEndianDate,
            apiResponse.Active
        );

        _trackedUsers.Add(user);

        return user;
    }
    
    public Task<bool> Exists(Guid id, CancellationToken stoppageToken = default)
    {
        // TODO: call KC API to check if user exists
        throw new NotImplementedException();
    }

    public void Update(Domain.User aggregateRoot)
    {
        var userStatus = _dataStore.Value.UserStatuses
            .OrderByDescending(x => x.ChangedAt)
            .First(x => x.UserId == aggregateRoot.Id);
        
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

        _webApiCalls.Add(() =>
        {
            // Update user in KC API
            var apiResponse = UserView.GetUsers().Single(x => x.Id == aggregateRoot.Id);
            apiResponse.FirstName = aggregateRoot.FirstName;
            apiResponse.LastName = aggregateRoot.LastName;
            apiResponse.Avatar = aggregateRoot.Avatar?.ToString();
            apiResponse.IsTraining = aggregateRoot.InTraining;
            apiResponse.Phone = aggregateRoot.Phone;
            apiResponse.Roles = aggregateRoot.Roles.ToList();
            apiResponse.Active = aggregateRoot.Active;
        });
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
    
    public void Sort(Guid id, int prevPosition, int curPosition)
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