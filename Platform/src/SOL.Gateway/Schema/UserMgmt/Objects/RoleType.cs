using SOL.Service.UserMgmt.Role.View;

namespace SOL.Gateway.Schema.UserMgmt.Objects;

public class RoleType : ObjectType<RoleView>
{
    protected override void Configure(IObjectTypeDescriptor<RoleView> descriptor)
    {
        descriptor
            .Name("Role")
            .Description("Represents the application Role.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Role.")
            .IsProjected();

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Role.");
    }
}