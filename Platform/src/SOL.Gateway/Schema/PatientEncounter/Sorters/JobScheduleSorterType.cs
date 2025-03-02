using HotChocolate.Data.Sorting;
using SOL.Gateway.Views.PatientEncounter.Job;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobScheduleSorterType : SortInputType<JobScheduleView>
{
    protected override void Configure(ISortInputTypeDescriptor<JobScheduleView> descriptor)
    {
        descriptor
            .Name("JobScheduleSorter")
            .Description("Sorting the Jobs Query by a Job's date/time.");
        
        descriptor
            .Field(t => t.Date)
            .Name("date")
            .Description("The DATE in which the Job should be executed.");
        
        descriptor
            .Field(t => t.Time)
            .Name("time")
            .Description("The TIME in which the Job should be executed.");     
    }
}