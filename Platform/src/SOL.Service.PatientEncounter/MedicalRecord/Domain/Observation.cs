using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.MedicalRecord.Domain;

public class Observation : ValueType<Observation>
{
    public Guid ObjectId { get; private set; }
    public ObservationType Type { get; private set; }
    public Instant Timestamp { get; private set; }
    
    internal Observation(Guid objectId, ObservationType type, Instant timestamp)
    {
        Type = type;
        ObjectId = objectId;
        Timestamp = timestamp;
    }
    
    protected override bool EqualsInternal(Observation? other)
    {
        return ObjectId.Equals(other?.ObjectId) 
               && Type.Equals(other?.Type) 
               && Timestamp.Equals(other?.Timestamp);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(ObjectId, Type, Timestamp);
    }
}