using SOL.Abstractions.Domain;
using SOL.Gateway.Schema.UserMgmt.Enums;
using SOL.Gateway.Schema.UserMgmt.Loaders;
using SOL.Gateway.Views.UserMgmt.User;

namespace SOL.Gateway.Schema.UserMgmt.Objects;

public class UserType : ObjectType<UserView>
{
    protected override void Configure(IObjectTypeDescriptor<UserView> descriptor)
    {
        descriptor
            .Name("User")
            .Description("Represents a user of the product.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the user.")
            .IsProjected();

        descriptor
            .Field(x => x.FirstName)
            .Name("firstName")
            .Description("The first name of the user.");

        descriptor
            .Field(x => x.LastName)
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
            .Field(x => x.Avatar)
            .Name("avatar")
            .Description("The avatar image URL of the user.");
        
        descriptor
            .Field(x => x.Active)
            .Name("active")
            .Description("Indicates whether the user is Active.");

        descriptor
            .Field(x => x.LoggedInAt)
            .Name("loggedInAt")
            .Description("The date and time of when the user last logged in.");
        
        descriptor
            .Field(x => x.PasswordChangedAt)
            .Name("passwordChangedAt")
            .Description("The date and time of when the user last changed their password.");
        
        descriptor
            .Field(x => x.RegisteredAt)
            .Name("registeredAt")
            .Description("The date and time of when the user created their account.");

        descriptor
            .Field(x => x.IsTraining)
            .Name("inTraining")
            .Description("Indicates whether the user is enrolled in training.");
        
        descriptor
            .Field(x => x.Identity)
            .Name("identity")
            .Description("The authentication identity of the user.");
        
        descriptor
            .Field(x => x.Preferences)
            .Type<ListType<UserPreferenceType>>()
            .Name("preferences")
            .Description("The preferences of the user.")
            .Resolve(ctx => {
                var user = ctx.Parent<UserView>();
                return Enum.GetValues<UserPreference>().Where(e => user.Preferences.HasFlag(e));
            });

        descriptor
            .Field("status")
            .Type<UserStatusType>()
            .Description("The current status of the user.")
            .Resolve(async ctx =>
                await ctx.DataLoader<UserStatusLoader>().LoadAsync(ctx.Parent<UserView>().Id, ctx.RequestAborted));
        
        descriptor
            .Field(x => x.Roles)
            .Name("roles")
            .Type<ListType<UuidType>>()
            .Description("The roles assigned to the user.");
    }
}