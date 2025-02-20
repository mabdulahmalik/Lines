using Dawn;
using SOL.Abstractions.Domain;
using SOL.Service.UserMgmt.User.Domain.Events;

namespace SOL.Service.UserMgmt.User.Domain;

public class User : AggregateRoot
{
    private readonly List<Guid> _roles = [];

    private User(Guid id)
        :base(id)
    { }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public Uri? Avatar { get; private set; }
    public bool InTraining { get; private set; }
    public UserStatus Status { get; private set; }
    public UserPreference Preference { get; private set; }
    public IReadOnlyList<Guid> Roles => _roles.AsReadOnly();
    public bool Active { get; private set; }

    internal static User Create(Guid id, string firstName, string lastName, string email, params Guid[] roles)
    {
        Guard.Argument(firstName).NotNull().NotEmpty();
        Guard.Argument(lastName).NotNull().NotEmpty();
        Guard.Argument(email).NotNull().NotEmpty();
        Guard.Argument(roles).NotNull().NotEmpty();

        var retValue = new User(id)
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Preference = UserPreference.ElapsedTime 
                         | UserPreference.MilitaryTime 
                         | UserPreference.MiddleEndianDate,
            Status = new UserStatus(id, UserAvailability.Offline),
            Active = true
        };

        retValue._roles.AddRange(roles.Distinct());

        retValue.RaiseEventCreated();
        return retValue;
    }

    internal static User MapKeyclockUser(Guid id, string firstName, string lastName, string email, string phone, 
        Uri? avatar, bool inTraining, List<Guid> roles, List<UserStatus> statuses, UserPreference preference, bool active)
    {
        Guard.Argument(firstName).NotNull().NotEmpty();
        Guard.Argument(lastName).NotNull().NotEmpty();
        Guard.Argument(email).NotNull().NotEmpty();
        Guard.Argument(roles).NotNull().NotEmpty();

        var retValue = new User(id)
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Avatar = avatar,
            InTraining = inTraining,
            Preference = preference,
            Active = active
        };

        retValue._roles.AddRange(roles.Distinct());
        retValue.Status = statuses.Last();

        return retValue;
    }

    public void Modify(string firstName, string lastName, string? phone = null, Uri? avatar = null)
    {
        Guard.Argument(firstName, nameof(firstName)).NotNull().NotEmpty();
        Guard.Argument(lastName, nameof(lastName)).NotNull().NotEmpty();

        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Avatar = avatar;

        RaiseEventModified();
    }

    public void AddRole(params Guid[] roleIds)
    {
        Guard.Argument(roleIds, nameof(roleIds)).NotNull().NotEmpty();
        Guard.Argument(roleIds, nameof(roleIds)).DoesNotContain(Guid.Empty);

        foreach (var roleId in roleIds)
        {
            if (!_roles.Contains(roleId))
            {
                _roles.Add(roleId);
            }
        }

        RaiseEventModified();
    }

    public void RemoveRole(params Guid[] roleIds)
    {
        Guard.Argument(roleIds, nameof(roleIds)).NotNull().NotEmpty();
        Guard.Argument(roleIds, nameof(roleIds)).DoesNotContain(Guid.Empty);

        _roles.RemoveAll(roleIds.Contains);
        RaiseEventModified();
    }

    public void ChangeStatus(UserAvailability status, string? message = null)
    {
        Status = new UserStatus(Id, status, message);
        RaiseEvent(new UserStatusChanged(Status));
    }
    
    public void SetPreferences(UserPreference preference)
    {
        Guard.Argument(preference, nameof(preference)).NotDefault();

        Preference = preference;
        RaiseEvent(new UserPreferenceChanged(Id, preference));
    }

    public void EnrollTraining()
    {
        InTraining = true;
        RaiseEventModified();
    }

    public void WithdrawTraining()
    {
        InTraining = false;
        RaiseEventModified();
    }

    public void Activate()
    {
        Active = true;
        RaiseEvent(new UserActivationChanged(Id, true));
    }

    public void Deactivate()
    {
        Active = false;
        RaiseEvent(new UserActivationChanged(Id, false));
    }
}