using SOL.Gateway.Application;

namespace SOL.Gateway.Schema;

public class Subscription : ObjectType
{
    public static readonly string DIRECT = "direct";
    public static readonly string BROADCAST = "broadcast";

    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field(DIRECT)
            .Description("Messages sent through this subscription are intended only for the active subscriber.")
            .Type<SubscriptionResponseType>()
            .Resolve(ctx => ctx.GetEventMessage<SubscriptionResponse>())
            .Subscribe(async ctx => {
                var subPub = ctx.Service<ISubscriptionHub>();
                return await subPub.SubscribeToDirect(ctx.RequestAborted);
            });

        descriptor
            .Field(BROADCAST)
            .Description("Messages sent through this subscription are intended for all subscribers.")
            .Type<SubscriptionResponseType>()
            .Resolve(ctx => ctx.GetEventMessage<SubscriptionResponse>())
            .Subscribe(async ctx => {
                var subPub = ctx.Service<ISubscriptionHub>();
                return await subPub.SubscribeToBroadcast(ctx.RequestAborted);
            });            
    }
}

public class SubscriptionResponseType : ObjectType<SubscriptionResponse>
{
    protected override void Configure(IObjectTypeDescriptor<SubscriptionResponse> descriptor)
    {
        descriptor
            .Name("SubscriptionResponse")
            .Description("The response from a subscription.");

        descriptor
            .Field(t => t.ActorId)
            .Name("actorId")
            .Description("The identifier of the actor that sent the message.");

        descriptor
            .Field(t => t.CorrelationId)
            .Name("correlationId")
            .Description("The identifier of the operation being performed.");

        descriptor
            .Field(t => t.EventName)
            .Name("eventName")
            .Description("The name of the event being sent.");

        descriptor
            .Field(t => t.Payload)
            .Name("payload")
            .Description("The data being sent with the event.");
    }
}

public class SubscriptionResponse
{
    public Guid ActorId { get; init; }
    public Guid CorrelationId { get; init; }
    public string EventName { get; init; }
    public string Payload { get; init; }
}