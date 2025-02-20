using HotChocolate.Data.Filters;
using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobsFilterType : FilterInputType<JobView>
{
    protected override void Configure(IFilterInputTypeDescriptor<JobView> descriptor)
    {
        descriptor
            .Name("JobsFilter")
            .Description("Filters the Job Query.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the job.");

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
            .Type<LocationFilterType>()
            .Name("location")
            .Description("The Location of the Job.");        

        descriptor
            .Field(t => t.Status)
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
            .Field(t => t.Schedule)
            .Name("schedule")
            .Description("The DATE the Job is scheduled for.")
            .Type<JobScheduleFilterType>();

        descriptor
            .Field(t => t.Contact)
            .Name("contact")
            .Description("The contact associated with the Job.");

        descriptor
            .Field(t => t.OrderingProvider)
            .Name("orderingProvider")
            .Description("The ordering provider associated with the Job.");
    }
}