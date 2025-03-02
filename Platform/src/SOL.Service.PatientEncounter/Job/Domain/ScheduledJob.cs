using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Job.Domain;

public class ScheduledJob : Entity
{
    public JobStatus Status { get; init; }
    public DateTime? StatusChangedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
    public Guid FacilityId { get; init; }
    public string Room { get; init; }
    public DateOnly ScheduledDate { get; init; }
    
    internal ScheduledJob(Guid id) 
        : base(id) 
    { }
}