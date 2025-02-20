using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EnactJobReschedulePrcType : InputObjectType<EnactJobReschedule>
{
    protected override void Configure(IInputObjectTypeDescriptor<EnactJobReschedule> descriptor)
    {
        descriptor
            .Name("EnactJobReschedulePrc")
            .Description("Process to reschedule a Job.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Job that needs to be rescheduled.");
        
        descriptor
            .Field(t => t.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier of the Facility where the Job is to be done.");

        descriptor
            .Field(t => t.Date)
            .Name("date")
            .Description("The DATE in which the Job should be rescheduled to.");
        
        descriptor
            .Field(t => t.Time)
            .Name("time")
            .Description("The TIME in which the Job should be rescheduled to.");
        
        descriptor
            .Field(t => t.Reason)
            .Name("reason")
            .Description("The Reason for rescheduling the Job.");
    }
}