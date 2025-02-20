using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EnactJobIntakePrcType : InputObjectType<EnactJobIntake>
{
    protected override void Configure(IInputObjectTypeDescriptor<EnactJobIntake> descriptor)
    {
        descriptor
            .Name("EnactJobIntakePrc")
            .Description("The Process that intakes a new Job and all requisite dependencies.");
        
        descriptor
            .Field(t => t.PurposeId)
            .Name("purposeId")
            .Description("The unique identifier of the Job Purpose.");

        descriptor
            .Field(t => t.Contact)
            .Name("contact")
            .Description("The Contact information for the job.");
        
        descriptor
            .Field(t => t.OrderingProvider)
            .Name("orderingProvider")
            .Description("The Ordering Provider information for the job.");        
        
        descriptor
            .Field(t => t.PreNote)
            .Name("preNote")
            .Description("A Note to be displayed with the job.");

        descriptor
            .Field(t => t.MedicalRecord)
            .Name("medicalRecord")
            .Type<IntakeMedicalRecordPrmType>()
            .Description("The date and time the job is scheduled to execute.");        
        
        descriptor
            .Field(t => t.Schedule)
            .Name("schedule")
            .Type<IntakeSchedulePrmType>()
            .Description("The date and time the job is scheduled to execute.");

        descriptor
            .Field(t => t.Location)
            .Type<IntakeLocationPrmType>()
            .Name("location")
            .Description("The location data for the Job Intake process.");
        
        descriptor
            .Field(t => t.Line)
            .Type<IntakeLinePrmType>()
            .Name("line")
            .Description("The Line data for the Job Intake process.");
        
        descriptor
            .Field(t => t.Urgency)
            .Type<IntakeUrgencyPrmType>()
            .Name("urgency")
            .Description("The Urgency data for the Job Intake process.");            
    }
}