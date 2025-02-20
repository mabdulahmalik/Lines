using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Job.Domain;

[TrackableActivity(nameof(JobScheduled))]
public record JobScheduled : IMessage
{
    public Guid Id { get; }
    public string? Reason { get; }
    public Guid? ScheduledBy { get; }
    public LocalDate ScheduledDate { get; }
    public LocalTime? ScheduledTime { get; }

    internal JobScheduled(Job job, LocalDate scheduledDate, LocalTime? scheduledTime = null, string? reason = null, Guid? scheduledBy = null)
    {
        Id = job.Id;
        Reason = reason;
        ScheduledBy = scheduledBy;
        ScheduledDate = scheduledDate;
        ScheduledTime = scheduledTime;
    }
}