using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobType : ObjectType<JobView>
{
    protected override void Configure(IObjectTypeDescriptor<JobView> descriptor)
    {
        descriptor
            .Name("Job")
            .Description("Initial work order placed that serves as the basis for an Encounter.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the job.")
            .IsProjected();

        descriptor
            .Field(t => t.PurposeId)
            .Name("purposeId")
            .Description("The unique identifier of the Purpose for the Job.");
        
        descriptor
            .Field(t => t.LineId)
            .Name("lineId")
            .Description("The unique identifier of the existing Line identified for the Job.");
        
        descriptor
            .Field(t => t.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The unique identifier of Medical Record identified for the Job.");
        
        descriptor
            .Field(t => t.Location)
            .Type<LocationType>()
            .Name("location")
            .Description("The Location of the Job.");        

        descriptor
            .Field(t => t.Status)
            .Type<JobStatusType>()
            .Name("status")
            .Description("The current status of the Job.");

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Job was CREATED.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Job was MODIFIED.");

        descriptor
            .Field(t => t.StatusChangedAt)
            .Name("statusChangedAt")
            .Description("The date and time the Job status was changed at.");

        descriptor
            .Field(t => t.Contact)
            .Name("contact")
            .Description("The contact associated with the Job.");

        descriptor
            .Field(t => t.OrderingProvider)
            .Name("orderingProvider")
            .Description("The ordering provider associated with the Job.");
        
        descriptor
            .Field("notes")
            .Description("Any/all Notes associated with the Job.")
            .Type<ListType<JobNoteType>>()
            .Resolve(async ctx => await ctx.DataLoader<JobNoteLoader>()
                .LoadAsync(ctx.Parent<JobView>().Id, ctx.RequestAborted));

        descriptor
            .Field(t => t.Schedule)
            .Name("schedule")
            .Description("The date and time the Job is scheduled for.")
            .Type<JobScheduleType>();
        
        descriptor
            .Field("origin")
            .Description("The Encounter and Procedure used to trigger this Follow Up Job.")
            .Type<JobRoutineType>()
            .Resolve(async ctx => await ctx.DataLoader<JobRoutineLoader>()
                .LoadAsync(ctx.Parent<JobView>().Id, ctx.RequestAborted));          
    }
}