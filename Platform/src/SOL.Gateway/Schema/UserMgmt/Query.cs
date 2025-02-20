using SOL.Abstractions.Application;
using SOL.Gateway.Schema.Common;
using SOL.Gateway.Schema.UserMgmt.Filters;
using SOL.Gateway.Schema.UserMgmt.Loaders;
using SOL.Gateway.Schema.UserMgmt.Objects;
using SOL.Gateway.Schema.UserMgmt.Sorters;
using SOL.Service.UserMgmt.Invitation.View;
using SOL.Service.UserMgmt.Role.View;
using SOL.Service.UserMgmt.User.View;

namespace SOL.Gateway.Schema.UserMgmt;

public class QueryJobsExtensions : ObjectTypeExtension<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("me")
            .Description("Gets the logged in user's information.")
            .Type<UserType>()
            .Resolve(async ctx =>
            {
                var opCtx = ctx.Service<IOperationContextFactory>().Get();
                return await ctx.DataLoader<LoggedInUserLoader>()
                    .LoadAsync(opCtx.ActorId, ctx.RequestAborted);
            });
        
        descriptor
            .Field("users")
            .Description("Get all Users.")
            .UseOffsetPaging<UserType>()
            .UseProjection()
            .UseFiltering<UsersFilterType>()
            .UseSorting<UsersSorterType>()            
            .ResolveWith<QueryResolver<UserView>>(x => x.Results(default!));

        descriptor
            .Field("invitations")
            .Description("Gets all Invitations.")
            .UseOffsetPaging<InvitationType>()
            .UseProjection()
            .UseFiltering<InvitationFilterType>()
            .UseSorting<InvitationSorterType>()
            .ResolveWith<QueryResolver<InvitationView>>(x => x.Results(default!));

        descriptor
            .Field("roles")
            .Description("Get all application Roles.")
            .UseOffsetPaging<RoleType>()
            .UseProjection()
            .ResolveWith<QueryResolver<RoleView>>(x => x.Results(default!));
    }
}