using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class ResendUserInviteCmdType : InputObjectType<ResendUserInvite>
{
    protected override void Configure(IInputObjectTypeDescriptor<ResendUserInvite> descriptor)
    {
        descriptor
            .Name("ResendUserInviteCmd")
            .Description("The command to resend a pending user invitation.");

        descriptor
            .Field(t => t.InviteId)
            .Name("inviteId")
            .Description("The unique identifier of the invitation to be resent.");
    }
}
