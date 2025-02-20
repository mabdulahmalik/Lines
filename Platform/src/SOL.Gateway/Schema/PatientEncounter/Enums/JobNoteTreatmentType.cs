using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobNoteTreatmentType : EnumType<JobNoteTreatment>
{
    protected override void Configure(IEnumTypeDescriptor<JobNoteTreatment> descriptor)
    {
        descriptor
            .Name("JobNoteTreatment")
            .Description("The type of treatment for a note.");

        descriptor
            .Value(JobNoteTreatment.None)
            .Name("NONE")
            .Description("Standard note, no treatment.");

        descriptor
            .Value(JobNoteTreatment.Pinned)
            .Name("PINNED")
            .Description("Indicates this Note should be pinned to the top of the Notes list and displayed on the Job card.");
        
        descriptor
            .Value(JobNoteTreatment.ReasonHold)
            .Name("REASON_HOLD")
            .Description("Indicates this Note is the reason the Encounter was put on HOLD.");
        
        descriptor
            .Value(JobNoteTreatment.ReasonStat)
            .Name("REASON_STAT")
            .Description("Indicates this Note is the reason the Encounter was ESCALATED to STAT.");
        
        descriptor
            .Value(JobNoteTreatment.ReasonSos)
            .Name("REASON_SOS")
            .Description("Indicates this Note is the reason the clinician Requested Assistance for this Encounter.");        
    }
}
