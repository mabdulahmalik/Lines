using SOL.Gateway.Schema.UserMgmt.Objects;
using SOL.Gateway.Schema.UserMgmt.Resolvers;

namespace SOL.Gateway.Schema.UserMgmt;

public class QueryUsersExtensions : ObjectTypeExtension<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("me")
            .Description("Gets the logged in user's information.")
            .Type<UserType>()
            .ResolveWith<UserMgmtResolver>(x => x.Me(default!, default!, default!, default!));
        
        descriptor
            .Field("users")
            .Description("Get all Users.")
            .UseOffsetPaging<UserType>()
            .UseProjection()        
            .ResolveWith<UserMgmtResolver>(x => x.Users(default!, default!, default!, default!, default!, default!));

        descriptor
            .Field("userInvitations")
            .Description("Gets all Invitations.")
            .UseOffsetPaging<UserInvitationType>()
            .UseProjection()
            .ResolveWith<UserMgmtResolver>(x => x.UserInvitations(default!, default!, default!, default!, default!));

        descriptor
            .Field("roles")
            .Description("Get all application Roles.")
            .UseOffsetPaging<RoleType>()
            .UseProjection()
            .ResolveWith<UserMgmtResolver>(x => x.Roles(default!, default!, default!, default!, default!));
    }
}