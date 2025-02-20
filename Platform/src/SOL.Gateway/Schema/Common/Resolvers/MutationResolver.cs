using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;

namespace SOL.Gateway.Schema.Common;

public class MutationResolver<TCommand> where TCommand : class
{
    public Task<MutationResponse> Mutate([Service] ICommandBus commandBus
        , [Service] IOperationContextFactory operationContextFactory
        , TCommand command, CancellationToken cancellationToken) 
        => commandBus.SendAsync(command, cancellationToken).ContinueWith(_ 
            => new MutationResponse { CorrelationId = operationContextFactory.Get().CorrelationId }, cancellationToken);
}