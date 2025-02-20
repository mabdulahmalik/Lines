using MassTransit;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.PatientEncounter;

public record AttachPhotosToEncounter : IMessage
{
    public Guid EncounterId { get; init; }
    public List<PhotoData> Photos { get; init; } = new();

    public record PhotoData
    {
        public string FileName { get; init; }
        public MessageData<Stream> Photo { get; init; }
    }
}
