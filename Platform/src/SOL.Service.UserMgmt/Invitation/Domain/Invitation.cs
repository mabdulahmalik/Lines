using Dawn;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.UserMgmt.Invitation.Domain;

public class Invitation : AggregateRoot
{
    private Invitation(Guid id)
        :base(id)
    { }

    public string Email { get; private set; }
    public Guid InvitedBy { get; private set; }
    public List<Guid> Roles { get; private set; }
    
    internal static Invitation Create(Guid id, Instant createdAt, string email, Guid invitedBy, List<Guid> roles)
    {
        Guard.Argument(email).NotNull().NotEmpty().NotWhiteSpace().MaxLength(256);
        Guard.Argument(roles).NotNull().NotEmpty();
        Guard.Argument(invitedBy).NotDefault();
        
        var retValue = new Invitation(id)
        {
            Email = email,
            Roles = roles,
            InvitedBy = invitedBy,
            CreatedAt = createdAt
        };

        return retValue;
    }
}
