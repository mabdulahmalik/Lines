using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Contracts.Common;

public record OperationErrored : IMessage
{
    public string MessageName { get; init; }
    public OperationError[] Errors { get; init; }
}

public record OperationError
{
    public string Error { get; init; }
    public string Message { get; init; }
}