using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Job.Domain;

[TrackableActivity(nameof(JobStatusChanged))]
public record JobStatusChanged : IMessage
{
    public Guid JobId { get; }
    public JobStatus Status { get; }

    internal JobStatusChanged(Job job)
    {
        JobId = job.Id;
        Status = job.Status;        
    }
}