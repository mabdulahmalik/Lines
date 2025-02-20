using HotChocolate.Execution;
using SOL.Abstractions.Messaging;
using SOL.Gateway.Schema;

namespace SOL.Gateway.Application;

public interface ISubscriptionHub
{
    ValueTask<ISourceStream<SubscriptionResponse>> SubscribeToBroadcast(CancellationToken stoppageToken = default);
    ValueTask<ISourceStream<SubscriptionResponse>> SubscribeToDirect(CancellationToken stoppageToken = default);
    ValueTask SendBroadcast(IMessage message, CancellationToken stoppageToken = default);
    ValueTask SendDirect(IMessage message, Guid userId, CancellationToken stoppageToken = default);
}