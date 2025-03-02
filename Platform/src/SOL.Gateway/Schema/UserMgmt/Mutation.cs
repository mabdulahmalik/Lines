using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;
using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway.Schema.UserMgmt;

public class MutationUserExtensions : ObjectTypeExtension<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field("modifyMyPreference")
            .Description("Updates the logged-in user's preferences.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyMyPreferenceCmdType>())
            .ResolveWith<MutationResolver<ModifyMyPreferences>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyMyStatus")
            .Description("Updates the logged-in user's status.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyMyStatusCmdType>())
            .ResolveWith<MutationResolver<ModifyMyStatus>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyMyProfile")
            .Description("Updates a user's profile.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyMyProfileCmdType>())
            .ResolveWith<MutationResolver<ModifyMyProfile>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyMyAvatar")
            .Description("Updates a user's avatar.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyMyAvatarCmdType>())
            .Resolve(async ctx => {
                var commandBus = ctx.Service<ICommandBus>();
                var command = ctx.ArgumentValue<ModifyMyAvatarCmd>("command");
                var operationContext = ctx.Service<IOperationContextFactory>().Get();
                var msgDataRepo = ctx.Service<IMessageDataRepository>();

                var result = command.Avatar != null
                ? await msgDataRepo.PutStream(command.Avatar.OpenReadStream(), TimeSpan.FromHours(6), ctx.RequestAborted)
                : null;

                await commandBus.SendAsync(new ModifyMyAvatar
                {
                    Avatar = result,
                    FileName = command.Avatar?.Name
                }, ctx.RequestAborted);

                return new MutationResponse
                {
                    CorrelationId = operationContext.CorrelationId
                };
            });

        descriptor
            .Field("modifyUser")
            .Description("Updates a user's details.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyUserCmdType>())
            .ResolveWith<MutationResolver<ModifyUser>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("inviteUsers")
            .Description("Invites new users by sending email invitations.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<InviteUsersCmdType>())
            .ResolveWith<MutationResolver<InviteUsers>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("resendUserInvite")
            .Description("Resends a pending user invitation.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ResendUserInviteCmdType>())
            .ResolveWith<MutationResolver<ResendUserInvite>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("cancelUserInvite")
            .Description("Cancels a pending user invitation.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CancelUserInviteCmdType>())
            .ResolveWith<MutationResolver<CancelUserInvite>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("activateUser")
            .Description("Activates a user.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ActivateUserCmdType>())
            .ResolveWith<MutationResolver<ActivateUser>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("deactivateUser")
            .Description("Deactivates a user.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeactivateUserCmdType>())
            .ResolveWith<MutationResolver<DeactivateUser>>(x => x.Mutate(default!, default!, default!, default!));

    }
}
