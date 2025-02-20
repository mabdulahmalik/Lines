using HotChocolate.Data.Filters;
using SOL.Service.UserMgmt.User.View;

namespace SOL.Gateway.Schema.UserMgmt.Filters;

public class UsersFilterType : FilterInputType<UserView>
{
    protected override void Configure(IFilterInputTypeDescriptor<UserView> descriptor)
    {
        descriptor
            .Name("UsersFilter")
            .Description("Filters for the Users Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the user.");

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
    }
}