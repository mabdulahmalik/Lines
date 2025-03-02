using SOL.Abstractions.Domain;
using SOL.Gateway.Schema.Common;
using SOL.Gateway.Views.OrganizationMgmt.Facility;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class FacilityType : ObjectType<FacilityView>
{
    protected override void Configure(IObjectTypeDescriptor<FacilityView> descriptor)
    {
        descriptor
            .Name("Facility")
            .Description("A Facility.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Facility.")
            .IsProjected();

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Facility.");

        descriptor
            .Field(x => x.TypeId)
            .Name("typeId")
            .Description("The unique identifier of the Facility Type.");

        descriptor
            .Field(x => x.TimeZone)
            .Name("timeZone")
            .Description("The time zone of the Facility.");

        descriptor
            .Field(x => x.Address)
            .Type<AddressType>()
            .Name("address")
            .Description("The Address of the Facility.");

        descriptor
            .Field(x => x.Archived)
            .Name("archived")
            .Description("Whether or not the Facility is archived.");

        descriptor
            .Field(x => x.RequiredData)
            .Type<ListType<RequiredPatientDataType>>()
            .Name("requiredData")
            .Description("The required Patient data for the Facility.")
            .Resolve(ctx => {
                var facility = ctx.Parent<FacilityView>();
                return Enum.GetValues<RequiredPatientData>().Where(e => facility.RequiredData.HasFlag(e));
            });

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Facility was Created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Facility was last Modified.");
        
        descriptor
            .Field(x => x.ReferenceCount)
            .Name("referenceCount")
            .Description("The number of Jobs, Encounters, and Lines referencing this Facility.");        

        descriptor
            .Field("procedureIds")
            .Description("The list of Procedure Unique Identifiers that are excluded.")
            .Type<ListType<UuidType>>()
            .Resolve(async ctx => {
                var result = await ctx.DataLoader<FacilityProcedureLoader>()
                    .LoadAsync(ctx.Parent<FacilityView>().Id, ctx.RequestAborted);
                return result.Select(x => x.ProcedureId);
            });

        descriptor
            .Field("purposeIds")
            .Description("The list of Purpose Unique Identifiers that are excluded.")
            .Type<ListType<UuidType>>()
            .Resolve(async ctx => {
                var result = await ctx.DataLoader<FacilityPurposeLoader>()
                    .LoadAsync(ctx.Parent<FacilityView>().Id, ctx.RequestAborted);
                return result.Select(x => x.PurposeId);
            });

        descriptor
            .Field("routineAssignments")
            .Description("The list of Routine Assignments.")
            .Type<ListType<FacilityRoutineType>>()
            .Resolve(async ctx => await ctx.DataLoader<FacilityRoutineLoader>()
                .LoadAsync(ctx.Parent<FacilityView>().Id, ctx.RequestAborted));
    }
}