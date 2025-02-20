using SOL.Gateway.Schema.UserMgmt.Enums;
using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class ModifyMyStatusCmdType : InputObjectType<ModifyMyStatus>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyMyStatus> descriptor)
    {
        descriptor
            .Name("ModifyMyStatusCmd")
            .Description("The command that modifies the logged-in user's status.");

        descriptor
            .Field(t => t.Status)
            .Type<UserAvailabilityType>()
            .Name("status")
            .Description("The new status of the user.");

        descriptor
            .Field(t => t.Message)
            .Name("message")
            .Description("An optional message describing the status change.");
    }
}
