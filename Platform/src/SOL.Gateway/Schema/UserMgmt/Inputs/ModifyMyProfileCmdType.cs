using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class ModifyMyProfileCmdType : InputObjectType<ModifyMyProfile>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyMyProfile> descriptor)
    {
        descriptor
            .Name("ModifyMyProfileCmd")
            .Description("The command that modifies a user profile.");

        descriptor
            .Field(t => t.FirstName)
            .Name("firstName")
            .Description("The first name of the user.");

        descriptor
            .Field(t => t.LastName)
            .Name("lastName")
            .Description("The last name of the user.");

        descriptor
            .Field(x => x.Phone)
            .Name("phone")
            .Description("The phone number of the user.");
    }
}
