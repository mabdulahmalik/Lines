using HotChocolate.Data.Filters;
using SOL.Service.UserMgmt.Invitation.View;

namespace SOL.Gateway.Schema.UserMgmt.Filters;

public class InvitationFilterType : FilterInputType<InvitationView>
{
    protected override void Configure(IFilterInputTypeDescriptor<InvitationView> descriptor)
    {
        descriptor
            .Name("InvitationFilter")
            .Description("Filters for the Invitation Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the invitation.");
        
        descriptor
            .Field(x => x.InvitedBy)
            .Name("invitedBy")
            .Description("The unique identifier of the user that sent the invitation.");

        descriptor
            .Field(x => x.Email)
            .Name("email")
            .Description("The email address to which the invitation was sent.");

        descriptor
            .Field(x => x.Roles)
            .Name("roles")
            .Description("The list of roles assigned to the user being invited.");

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time when the invitation was created.");
    }
}
