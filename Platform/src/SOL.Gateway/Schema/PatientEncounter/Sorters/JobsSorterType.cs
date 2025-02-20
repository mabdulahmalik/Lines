using HotChocolate.Data.Sorting;
using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobsSorterType : SortInputType<JobView>
{
    protected override void Configure(ISortInputTypeDescriptor<JobView> descriptor)
    {
        descriptor
            .Name("JobsSorter")
            .Description("Sorting the Job Query.");
        
        descriptor
            .Field(t => t.Status)
            .Name("status")
            .Description("The current status of the job.");

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the job was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the job was modified.");

        descriptor
            .Field(t => t.StatusChangedAt)
            .Name("statusChangedAt")
            .Description("The date and time the job status was changed at.");

        descriptor
            .Field(t => t.Schedule)
            .Name("scheduled")
            .Description("The DATE the Job is scheduled for.")
            .Type<JobScheduleSorterType>();

        descriptor
            .Field(t => t.Contact)
            .Name("contact")
            .Description("The contact associated with the job.");

        descriptor
            .Field(t => t.OrderingProvider)
            .Name("orderingProvider")
            .Description("The ordering provider associated with the job.");        
    }
}