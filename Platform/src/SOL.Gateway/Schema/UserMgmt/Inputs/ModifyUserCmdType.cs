using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class ModifyUserCmdType : InputObjectType<ModifyUser>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyUser> descriptor)
    {
        descriptor
            .Name("ModifyUserCmd")
            .Description("The command that modifies a user details.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the user.");

        descriptor
            .Field(t => t.FirstName)
            .Name("firstName")
            .Description("The first name of the user.");

        descriptor
            .Field(t => t.LastName)
            .Name("lastName")
            .Description("The last name of the user.");

        descriptor
            .Field(x => x.Email)
            .Name("email")
            .Description("The email address of the user.");
        
        descriptor
            .Field(x => x.Phone)
            .Name("phone")
            .Description("The phone number of the user.");

        descriptor
            .Field(x => x.InTraining)
            .Name("inTraining")
            .Description("Indicates whether the user is enrolled in training.");

        descriptor
            .Field(x => x.Roles)
            .Name("roles")
            .Description("A list of unique role identifiers assigned to the user.");
    }
}
