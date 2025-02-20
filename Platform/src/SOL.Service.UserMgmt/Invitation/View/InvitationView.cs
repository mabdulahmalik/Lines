using SOL.Abstractions.Persistence;

namespace SOL.Service.UserMgmt.Invitation.View;

public class InvitationView
{
    private static List<InvitationView> _invitations = InitializeInvitations();

    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Email { get; set; }
    public Guid? InvitedBy { get; set; }
    public List<Guid>? Roles { get; set; }

    public static List<InvitationView> GetInvitations() => _invitations;

    public static List<InvitationView> InitializeInvitations() => new List<InvitationView>
    {
        new InvitationView
        {
            Id = Guid.Parse("7a46f0b1-7399-4029-acb2-3fb280e909c9"),
            InvitedBy = Guid.Parse("462b8b01-ecd7-4140-9e03-420a672ef35a"),
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            Email = "steve.rodgers@marvel.com",
            Roles = [Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a"), Guid.Parse("a3f5c9d7-8b42-4e1a-91f6-2d7e8a4c6b3e")]
        },
        new InvitationView
        {
            Id = Guid.Parse("aa11475e-fc8c-4dcf-bbc1-b510b8749b4e"),
            InvitedBy = Guid.Parse("462b8b01-ecd7-4140-9e03-420a672ef35a"),
            CreatedAt = DateTime.UtcNow.AddHours(-6),
            Email = "sam.wilson@@marvel.com",
            Roles = [Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a")]
        },
        new InvitationView
        {
            Id = Guid.Parse("390b6e17-9bb3-4761-8b6d-20c74070fc44"),
            InvitedBy = Guid.Parse("617ac863-0b6e-4ddf-89e2-37a630cac723"),
            CreatedAt = DateTime.UtcNow.AddHours(-12),
            Email = "wanda.maximoff@marvel.com",
            Roles = [Guid.Parse("3f1a8c5d-2b64-4b89-8f36-5e9d4c7e1e3a")]
        }
    };
}

public class InvitationQuery : IDomainQuery<InvitationView>
{
    public IQueryable<InvitationView> Query => InvitationView.GetInvitations().AsQueryable();
}