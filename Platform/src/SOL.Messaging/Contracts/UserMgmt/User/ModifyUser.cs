using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ModifyUser : IMessage
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool InTraining { get; set; }
    public List<Guid> Roles { get; set; } = [];
}