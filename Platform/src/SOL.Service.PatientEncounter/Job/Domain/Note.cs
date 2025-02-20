using Dawn;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Job.Domain;

public class Note : ValueType<Note>
{
    public Guid Id { get; private set; }
    public Instant CreatedAt { get; private set; }
    public Guid UserId { get; private set; }
    public string Text { get; private set; }
    public JobNoteTreatment Treatment { get; private set; }

    internal Note(Guid id, Instant createdAt, Guid userId, string text, JobNoteTreatment treatment)
    { 
        Guard.Argument(text).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(userId).NotDefault("Invalid UserId provided.");
        
        CreatedAt = createdAt;
        Treatment = treatment;
        UserId = userId;
        Text = text;
        Id = id;
    }

    internal Note(Guid userId, string text, JobNoteTreatment treatment = JobNoteTreatment.None)
        : this(Guid.NewGuid(), SystemClock.Instance.GetCurrentInstant(), userId, text, treatment)
    { }

    internal void SetText(string text)
    {
        Guard.Argument(text).NotNull().NotEmpty().NotWhiteSpace();
        Text = text;
    }

    internal void Pin()
    {
        Treatment = JobNoteTreatment.Pinned;
    }
    
    internal void Unpin()
    {
        Treatment = JobNoteTreatment.None;
    }

    protected override bool EqualsInternal(Note other)
    {
        return Id.Equals(other.Id);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Id);
    }
}