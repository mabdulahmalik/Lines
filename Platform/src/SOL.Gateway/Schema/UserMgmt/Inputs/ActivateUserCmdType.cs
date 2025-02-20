using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class ActivateUserCmdType : InputObjectType<ActivateUser>
{
    protected override void Configure(IInputObjectTypeDescriptor<ActivateUser> descriptor)
    {
        descriptor
            .Name("ActivateUserCmd")
            .Description("Command to activate a user.");

        descriptor
            .Field(x => x.UserId)
            .Name("userId")
            .Description("Unique identifier of the user to activate.");
    }
}
