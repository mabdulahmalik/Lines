using SOL.Gateway.Views.PatientEncounter.Job;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobScheduleType : ObjectType<JobScheduleView>
{
    protected override void Configure(IObjectTypeDescriptor<JobScheduleView> descriptor)
    {
        descriptor
            .Name("JobSchedule")
            .Description("The scheduled date and time the Job should be attended to.");
        
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