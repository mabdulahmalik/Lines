using SOL.Gateway.Schema.UserMgmt.Enums;
using SOL.Gateway.Views.UserMgmt.User;

namespace SOL.Gateway.Schema.UserMgmt.Objects;

public class UserStatusType : ObjectType<UserStatusView>
{
    protected override void Configure(IObjectTypeDescriptor<UserStatusView> descriptor)
    {
        descriptor
            .Name("UserStatus")
            .Description("Represents the Status of a user.");

        descriptor
            .Field(x => x.Status)
            .Type<UserAvailabilityType>()
            .Name("status")
            .Description("The availability status of the user.");
        
        descriptor
            .Field(x => x.Message)
            .Name("message")
            .Description("The status message of the user.");
        
        descriptor
            .Field(x => x.ChangedAt)
            .Name("changedAt")
            .Description("The date and time of when the status was last changed.");
    }
}