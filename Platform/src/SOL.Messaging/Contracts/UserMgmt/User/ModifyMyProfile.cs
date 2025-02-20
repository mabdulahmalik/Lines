using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.UserMgmt;

public record ModifyMyProfile : IMessage
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}
