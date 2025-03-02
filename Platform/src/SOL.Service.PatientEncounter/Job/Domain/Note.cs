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
    public bool IsPinned { get; private set; }

    internal Note(Guid id, Instant createdAt, Guid userId, string text, bool isPinned)
    { 
        Guard.Argument(text).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(userId).NotDefault("Invalid UserId provided.");
        
        CreatedAt = createdAt;
        IsPinned = isPinned;
        UserId = userId;
        Text = text;
        Id = id;
    }

    internal Note(Guid userId, string text)
        : this(Guid.NewGuid(), SystemClock.Instance.GetCurrentInstant(), userId, text, false)
    { }

    internal void SetText(string text)
    {
        Guard.Argument(text).NotNull().NotEmpty().NotWhiteSpace();
        Text = text;
    }

    internal void Pin()
    {
        IsPinned = true;
    }
    
    internal void Unpin()
    {
        IsPinned = false;
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