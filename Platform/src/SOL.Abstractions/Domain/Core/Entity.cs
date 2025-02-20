namespace SOL.Abstractions.Domain;

public abstract class Entity : ValueType<Entity>
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected override bool EqualsInternal(Entity other)
    {
        return Id.Equals(other.Id);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Id);
    }
}