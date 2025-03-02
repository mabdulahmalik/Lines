namespace SOL.Gateway.Views.UserMgmt.Invitation;

public class InvitationView
{
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Email { get; set; }
    public Guid? InvitedBy { get; set; }
    public List<Guid>? Roles { get; set; }
}