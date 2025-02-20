using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class CancelUserInviteCmdType : InputObjectType<CancelUserInvite>
{
    protected override void Configure(IInputObjectTypeDescriptor<CancelUserInvite> descriptor)
    {
        descriptor
            .Name("CancelUserInviteCmd")
            .Description("The command to cancel a pending user invitation.");

        descriptor
            .Field(t => t.InviteId)
            .Name("inviteId")
            .Description("The unique identifier of the invitation to be canceled.");
    }
}
