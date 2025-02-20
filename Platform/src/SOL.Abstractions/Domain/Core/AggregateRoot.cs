using NodaTime;
using SOL.Abstractions.Messaging;

namespace SOL.Abstractions.Domain;

public abstract class AggregateRoot : Entity
{
    private Tracked<IMessage> _changes = new();

    protected AggregateRoot(Guid id) 
        : base(id)
    { }

    public Instant CreatedAt { get; protected set; }
    public Instant? ModifiedAt { get; protected set; }
    public IReadOnlyList<TimeStamped<IMessage>> Changes => _changes;

    protected void RaiseEvent(IMessage domainEvent)
    {
        _changes.RemoveAll(msg => msg.Value.GetType() == domainEvent.GetType());
        _changes.Add(domainEvent);        
    }
    
    protected void RaiseEventCreated() => RaiseEvent(new AggregateCreated { Id = Id, Name = GetType().Name });
    protected void RaiseEventModified() => RaiseEvent(new AggregateModified { Id = Id, Name = GetType().Name });
}