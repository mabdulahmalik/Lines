using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class MutationFacilitiesExtension : ObjectTypeExtension<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field("createFacilityType")
            .Description("Creates a new Facility Type.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CreateFacilityTypeCmdType>())
            .ResolveWith<MutationResolver<CreateFacilityType>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyFacilityType")
            .Description("Modify a Facility Type.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyFacilityTypeCmdType>())
            .ResolveWith<MutationResolver<ModifyFacilityType>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("sortFacilityType")
            .Description("Changes the Sort Order of a Facility Type.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<SortFacilityTypeCmdType>())
            .ResolveWith<MutationResolver<SortFacilityType>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("activateFacilityType")
            .Description("Activates a Facility Type.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ActivateFacilityTypeCmdType>())
            .ResolveWith<MutationResolver<ActivateFacilityType>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("deactivateFacilityType")
            .Description("Deactivates a Facility Type.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeactivateFacilityTypeCmdType>())
            .ResolveWith<MutationResolver<DeactivateFacilityType>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("createRoutine")
            .Description("Creates a new Routine.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CreateRoutineCmdType>())
            .ResolveWith<MutationResolver<CreateRoutine>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyRoutine")
            .Description("Modifies a Routine.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyRoutineCmdType>())
            .ResolveWith<MutationResolver<ModifyRoutine>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("deleteRoutine")
            .Description("Deletes a Routine.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeleteRoutineCmdType>())
            .ResolveWith<MutationResolver<DeleteRoutine>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("activateRoutine")
            .Description("Activates a Routine.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ActivateRoutineCmdType>())
            .ResolveWith<MutationResolver<ActivateRoutine>>(x => x.Mutate(default!, default!, default!, default!));        

        descriptor
            .Field("deactivateRoutine")
            .Description("Deactivates a Routine.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeactivateRoutineCmdType>())
            .ResolveWith<MutationResolver<DeactivateRoutine>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("createFacility")
            .Description("Creates a new Facility.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CreateFacilityCmdType>())
            .ResolveWith<MutationResolver<CreateFacility>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyFacility")
            .Description("Modifies a facility.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyFacilityCmdType>())
            .ResolveWith<MutationResolver<ModifyFacility>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("sortFacility")
            .Description("Changes the Sort Order of a Facility.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<SortFacilityCmdType>())
            .ResolveWith<MutationResolver<SortFacility>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("deleteFacility")
            .Description("Deletes a Facility.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeleteFacilityCmdType>())
            .ResolveWith<MutationResolver<DeleteFacility>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("archiveFacility")
            .Description("Archives a Facility.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ArchiveFacilityCmdType>())
            .ResolveWith<MutationResolver<ArchiveFacility>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("unarchiveFacility")
            .Description("Unarchives a Facility.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<UnarchiveFacilityCmdType>())
            .ResolveWith<MutationResolver<UnarchiveFacility>>(x => x.Mutate(default!, default!, default!, default!));        

        descriptor
            .Field("createFacilityRoom")
            .Description("Creates a new Room.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CreateFacilityRoomCmdType>())
            .ResolveWith<MutationResolver<CreateFacilityRoom>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyFacilityRoom")
            .Description("Modifies a Room.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyFacilityRoomCmdType>())
            .ResolveWith<MutationResolver<ModifyFacilityRoom>>(x => x.Mutate(default!, default!, default!, default!));       
    }
}