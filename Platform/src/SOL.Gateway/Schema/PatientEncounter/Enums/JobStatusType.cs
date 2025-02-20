using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobStatusType : EnumType<JobStatus>
{
    protected override void Configure(IEnumTypeDescriptor<JobStatus> descriptor)
    {
        descriptor
            .Name("JobStatus")
            .Description("The Status of the job.");

        descriptor
            .Value(JobStatus.Scheduled)
            .Name("SCHEDULED")
            .Description("The job has been scheduled.");

        descriptor
            .Value(JobStatus.Underway)
            .Name("UNDERWAY")
            .Description("The job has an associated Encounter.");

        descriptor
            .Value(JobStatus.Completed)
            .Name("COMPLETED")
            .Description("The job has been completed.");

        descriptor
            .Value(JobStatus.Canceled)
            .Name("CANCELED")
            .Description("The job has been canceled.");
    }
}
