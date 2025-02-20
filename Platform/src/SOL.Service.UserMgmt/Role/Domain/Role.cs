using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.UserMgmt.Role.Domain;

public class Role : AggregateRoot
{
    internal static Role Create(string name)
    {
        Guard.Argument(name).NotNull().NotEmpty();

        var retValue = new Role(Guid.NewGuid()) { Name = name };

        retValue.RaiseEventCreated();

        return retValue;
    }

    private Role(Guid id) 
        : base(id)
    {
    }

    public string Name { get; private set; }
}
