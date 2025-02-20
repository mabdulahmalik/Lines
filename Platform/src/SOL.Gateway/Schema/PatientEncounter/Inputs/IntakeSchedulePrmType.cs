using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class IntakeSchedulePrmType : InputObjectType<IntakeSchedule>
{
    protected override void Configure(IInputObjectTypeDescriptor<IntakeSchedule> descriptor)
    {
        descriptor
            .Name("IntakeSchedulePrm")
            .Description("The date and time the job is scheduled to be attended to.");
        
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