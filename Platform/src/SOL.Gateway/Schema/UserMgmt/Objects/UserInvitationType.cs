using SOL.Gateway.Schema.UserMgmt.Loaders;
using SOL.Gateway.Views.UserMgmt.Invitation;

namespace SOL.Gateway.Schema.UserMgmt.Objects;

public class UserInvitationType : ObjectType<InvitationView>
{
    protected override void Configure(IObjectTypeDescriptor<InvitationView> descriptor)
    {
        descriptor
            .Name("UserInvitation")
            .Description("Represents a pending invitation for a user to join this organization.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the invitation.");
        
        descriptor
            .Field(x => x.InvitedBy)
            .Name("invitedBy")
            .Description("The unique identifier of the user that sent the invitation.");
        
        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time when the invitation was initially created and sent.");

        descriptor
            .Field(x => x.Email)
            .Name("email")
            .Description("The email address to which the invitation was sent.");

        descriptor
            .Field(x => x.Roles)
            .Name("roles")
            .Type<ListType<UuidType>>()
            .Description("The list of roles assigned to the user being invited.");
    }
}
