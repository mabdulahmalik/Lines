using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterType : ObjectType<EncounterView>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterView> descriptor)
    {
        descriptor
            .Name("Encounter")
            .Description("Core unit of work performed by one or more clinicians, describes an Encounter with a Patient.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the Encounter.")
            .IsProjected();
        
        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier for the Job this Encounter belongs to.");
        
        descriptor
            .Field(t => t.PurposeId)
            .Name("purposeId")
            .Description("The Purpose of the Encounter.");
        
        descriptor
            .Field(t => t.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The Medical Record associated with the Encounter.");           
        
        descriptor
            .Field(t => t.Location)
            .Type<LocationType>()
            .Name("location")
            .Description("The Location of the Encounter.");         

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Encounter was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Encounter was modified.");

        descriptor
            .Field(t => t.Priority)
            .Type<EncounterPriorityType>()
            .Name("priority")
            .Description("The priority of the Encounter."); 

        descriptor
            .Field(t => t.Stage)
            .Type<EncounterStageType>()
            .Name("stage")
            .Description("The current Stage of the Encounter.");        
        
        descriptor
            .Field("assignments")
            .Description("The assigned Clinicians on the Encounter.")
            .Type<ListType<EncounterAssignmentType>>()
            .Resolve(async ctx => await ctx.DataLoader<EncounterAssignmentLoader>()
                .LoadAsync(ctx.Parent<EncounterView>().Id, ctx.RequestAborted));

        descriptor
            .Field("procedures")
            .Description("The executed Procedures during this Encounter.")
            .Type<ListType<EncounterProcedureType>>()
            .Resolve(async ctx => await ctx.DataLoader<EncounterProcedureLoader>()
                .LoadAsync(ctx.Parent<EncounterView>().Id, ctx.RequestAborted));            

        descriptor
            .Field("photos")
            .Description("The Photos associated with the Encounter.")
            .Type<ListType<EncounterPhotoType>>()
            .Resolve(async ctx => await ctx.DataLoader<EncounterPhotoLoader>()
                .LoadAsync(ctx.Parent<EncounterView>().Id, ctx.RequestAborted));
        
        descriptor
            .Field("lines")
            .Description("The Lines associated with the Encounter.")
            .Type<ListType<UuidType>>()
            .Resolve(async ctx => await ctx.DataLoader<LineEncounterLoader>()
                .LoadAsync(ctx.Parent<EncounterView>().Id, ctx.RequestAborted));
        
        descriptor
            .Field("progress")
            .Description("The progression of the Encounter through the Workflow.")
            .Type<ListType<EncounterProgressStageType>>()
            .Resolve(async ctx => await ctx.DataLoader<EncounterProgressLoader>()
                .LoadAsync(ctx.Parent<EncounterView>().Id, ctx.RequestAborted));
        
        descriptor
            .Field("alerts")
            .Description("The Alerts that need to be surfaced for this Encounter.")
            .Type<ListType<EncounterAlertType>>()
            .Resolve(async ctx => await ctx.DataLoader<EncounterAlertLoader>()
                .LoadAsync(ctx.Parent<EncounterView>().Id, ctx.RequestAborted));        
    }
}