using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;
using SOL.Gateway.Schema;

namespace SOL.Gateway.Application;

public class SubscriptionHub : ISubscriptionHub
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly ITopicEventReceiver _topicEventReceiver;
    private readonly ITopicEventSender _topicEventSender;
    
    private readonly JsonSerializerSettings _jsonSerializerSettings = new() {
        Formatting = Formatting.None,
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new CamelCaseNamingStrategy(true, false)
        },
        Converters = new List<JsonConverter> {
            new StringEnumConverter(new AllCapsSnakeCaseNamingStrategy())
        }
    };

    public SubscriptionHub(ITopicEventReceiver topicEventReceiver, ITopicEventSender topicEventSender, IOperationContextFactory operationContextFactory)
    {
        _topicEventReceiver = topicEventReceiver;
        _topicEventSender = topicEventSender;
        _operationContext = new(operationContextFactory.Get);
    }

    public async ValueTask<ISourceStream<SubscriptionResponse>> SubscribeToBroadcast(CancellationToken stoppageToken = default)
    {
        var topicName = $"{Subscription.BROADCAST}_{_operationContext.Value.TenantKey}";
        return await _topicEventReceiver.SubscribeAsync<SubscriptionResponse>(topicName, stoppageToken);
    }

    public async ValueTask SendBroadcast(IMessage message, CancellationToken stoppageToken = default)
    {
        var subResp = new SubscriptionResponse {
            CorrelationId = _operationContext.Value.CorrelationId,
            ActorId = _operationContext.Value.ActorId,
            EventName = message.GetType().Name,
            Payload = JsonConvert.SerializeObject(message, _jsonSerializerSettings)
        };

        var topicName = $"{Subscription.BROADCAST}_{_operationContext.Value.TenantKey}";
        await _topicEventSender.SendAsync(topicName, subResp, stoppageToken);
    }    

    public async ValueTask<ISourceStream<SubscriptionResponse>> SubscribeToDirect(CancellationToken stoppageToken = default)
    {
        var topicName = $"{Subscription.DIRECT}_{_operationContext.Value.TenantKey}_{_operationContext.Value.ActorId:N}";
        return await _topicEventReceiver.SubscribeAsync<SubscriptionResponse>(topicName, stoppageToken);
    }

    public async ValueTask SendDirect(IMessage message, Guid userId, CancellationToken stoppageToken = default)
    {
        var subResp = new SubscriptionResponse {
            CorrelationId = _operationContext.Value.CorrelationId,
            ActorId = _operationContext.Value.ActorId,
            EventName = message.GetType().Name,
            Payload = JsonConvert.SerializeObject(message, _jsonSerializerSettings)
        };

        var topicName = $"{Subscription.DIRECT}_{_operationContext.Value.TenantKey}_{userId:N}";
        await _topicEventSender.SendAsync(topicName, subResp, stoppageToken);
    }
}

class AllCapsSnakeCaseNamingStrategy() : SnakeCaseNamingStrategy(true, false)
{
    protected override string ResolvePropertyName(string name)
    {
        var value = base.ResolvePropertyName(name);
        return value.ToUpper();
    }
}