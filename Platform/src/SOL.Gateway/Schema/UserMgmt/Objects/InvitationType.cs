using SOL.Gateway.Schema.UserMgmt.Loaders;
using SOL.Service.UserMgmt.Invitation.View;

namespace SOL.Gateway.Schema.UserMgmt.Objects;

public class InvitationType : ObjectType<InvitationView>
{
    protected override void Configure(IObjectTypeDescriptor<InvitationView> descriptor)
    {
        descriptor
            .Name("Invitation")
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
            .Field("roles")
            .Type<ListType<UuidType>>()
            .Description("The list of roles assigned to the user being invited.")
            .Resolve(async ctx =>
                await ctx.DataLoader<InvitationRoleLoader>().LoadAsync(ctx.Parent<InvitationView>().Id, ctx.RequestAborted));
    }
}
