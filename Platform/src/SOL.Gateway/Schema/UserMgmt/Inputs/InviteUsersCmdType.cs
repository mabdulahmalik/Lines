using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class InviteUsersCmdType : InputObjectType<InviteUsers>
{
    protected override void Configure(IInputObjectTypeDescriptor<InviteUsers> descriptor)
    {
        descriptor
            .Name("InviteUsersCmd")
            .Description("The command to invite new users via email.");

        descriptor
            .Field(t => t.Emails)
            .Name("emails")
            .Description("A list of email addresses of users to be invited.");

        descriptor
            .Field(t => t.Roles)
            .Name("roles")
            .Description("The roles assigned to the invited users.");
    }
}
