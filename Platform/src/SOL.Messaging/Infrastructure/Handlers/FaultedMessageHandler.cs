using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;

namespace SOL.Messaging.Infrastructure.Handlers;

public class FaultedMessageHandler : IConsumer<Fault>
{
    private readonly IServiceBus _serviceBus;

    public FaultedMessageHandler(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    public async Task Consume(ConsumeContext<Fault> context)
    {
        await HandleAsync(context.Message.Exceptions, context.Message.FaultMessageTypes, context.CancellationToken);
    }

    private async Task HandleAsync(ExceptionInfo[] exceptions, string[] messageTypes, CancellationToken stoppageToken)
    {
        if (exceptions.Any(ex => ex.ExceptionType == "MassTransit.UnknownStateException"))
            return;
        
        var errors = new List<OperationError>();
        Array.ForEach(exceptions, ex => GetExceptions(ex, errors));

        var msg = new OperationErrored {
            MessageName = messageTypes.First().Split(":").Last(),
            Errors = errors.ToArray()
        };

        await _serviceBus.PublishAsync(msg, stoppageToken);
    }

    private void GetExceptions(ExceptionInfo? exception, List<OperationError> retVal)
    {
        if(exception == null) {
            return;
        }

        var rootError = exception.ExceptionType.Split('.').Last();

        retVal.Add(new OperationError { Error = rootError, Message = exception.Message });
        GetExceptions(exception.InnerException, retVal);
    }    
}