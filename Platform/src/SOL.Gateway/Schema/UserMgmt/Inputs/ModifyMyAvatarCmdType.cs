namespace SOL.Gateway.Schema.UserMgmt;

public class ModifyMyAvatarCmdType : InputObjectType<ModifyMyAvatarCmd>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyMyAvatarCmd> descriptor)
    {
        descriptor
            .Name("ModifyMyAvatarCmd")
            .Description("The command that modifies a user avatar.");

        descriptor
            .Field(x => x.Avatar)
            .Name("avatar")
            .Description("The binary representations of the Avatar.");
    }
}
public record ModifyMyAvatarCmd
{
    public IFile? Avatar { get; set; }
}
