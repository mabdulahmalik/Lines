using SOL.Gateway.Schema.UserMgmt.Enums;
using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class ModifyMyPreferenceCmdType : InputObjectType<ModifyMyPreferences>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyMyPreferences> descriptor)
    {
        descriptor
            .Name("ModifyMyPreferenceCmd")
            .Description("The command that modifies the logged-in user's preference.");

        descriptor
            .Field(t => t.Preferences)
            .Type<ListType<UserPreferenceType>>()
            .Name("preferences")
            .Description("The preferences of the user.");
    }
}