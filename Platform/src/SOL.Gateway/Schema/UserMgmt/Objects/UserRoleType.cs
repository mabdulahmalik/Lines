using SOL.Service.UserMgmt.User.View;

namespace SOL.Gateway.Schema.UserMgmt.Objects;

public class UserRoleType : ObjectType<UserRoleView>
{
    protected override void Configure(IObjectTypeDescriptor<UserRoleView> descriptor)
    {
        descriptor
            .Name("UserRole")
            .Description("Represents the Role of a user.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the role.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the role.");
    }
}