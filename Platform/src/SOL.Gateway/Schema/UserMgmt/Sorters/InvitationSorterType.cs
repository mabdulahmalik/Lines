using HotChocolate.Data.Sorting;
using SOL.Service.UserMgmt.Invitation.View;

namespace SOL.Gateway.Schema.UserMgmt;

public class InvitationSorterType : SortInputType<InvitationView>
{
    protected override void Configure(ISortInputTypeDescriptor<InvitationView> descriptor)
    {
        descriptor
            .Name("InvitationSorter")
            .Description("Sorting the Invitation Query.");

        descriptor
            .Field(x => x.Email)
            .Name("email")
            .Description("The email address to which the invitation was sent.");

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time when the invitation was created.");
    }
}