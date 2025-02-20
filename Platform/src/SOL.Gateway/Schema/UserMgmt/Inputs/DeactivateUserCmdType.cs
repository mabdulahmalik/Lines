using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class DeactivateUserCmdType : InputObjectType<DeactivateUser>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeactivateUser> descriptor)
    {
        descriptor
            .Name("DeactivateUserCmd")
            .Description("Command to deactivate a user.");

        descriptor
            .Field(x => x.UserId)
            .Name("userId")
            .Description("Unique identifier of the user to deactivate.");
    }
}
