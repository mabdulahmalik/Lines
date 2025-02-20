using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class IntakeUrgencyPrmType : InputObjectType<IntakeUrgency>
{
    protected override void Configure(IInputObjectTypeDescriptor<IntakeUrgency> descriptor)
    {
        descriptor
            .Name("IntakeUrgencyPrm")
            .Description("Holds the Urgency data for the Job Intake process.");        
        
        descriptor
            .Field(t => t.IsStat)
            .Name("isStat")
            .Description("Whether the job has STAT urgency.");
        
        descriptor
            .Field(t => t.StatReason)
            .Name("statReason")
            .Description("The reason behind the STAT urgency.");        

        descriptor
            .Field(t => t.IsOnHold)
            .Name("isOnHold")
            .Description("Whether the job is on HOLD.");
        
        descriptor
            .Field(t => t.HoldReason)
            .Name("holdReason")
            .Description("The reason behind the HOLD.");        
    }
}