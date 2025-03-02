using HotChocolate.Data.Filters;
using SOL.Gateway.Views.PatientEncounter.Job;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobScheduleFilterType : FilterInputType<JobScheduleView>
{
    protected override void Configure(IFilterInputTypeDescriptor<JobScheduleView> descriptor)
    {
        descriptor
            .Name("JobScheduleFilter")
            .Description("Filters the Jobs query by a Job's date/time.");
        
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