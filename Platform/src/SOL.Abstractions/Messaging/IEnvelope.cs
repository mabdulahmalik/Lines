using NodaTime;

namespace SOL.Abstractions.Messaging;

public interface IEnvelope<TMessage> where TMessage : IMessage
{
        Guid OperationId { get; }
        string CorrelationId { get; }
        Instant TimeStamp { get; }
        string MessageName { get; }
        Guid OwnerId { get; }
        
        TMessage Payload { get; }
}

public interface IMessage { }