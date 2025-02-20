using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Messaging;

namespace SOL.Messaging.Infrastructure.Handlers;

public class DomainRelayHandler<TEvent> : DomainEventHandler<TEvent>
    where TEvent : class, IMessage
{
    private readonly IServiceBus _serviceBus;
    private readonly IMapper _mapper;

    public DomainRelayHandler(ILogger<DomainRelayHandler<TEvent>> logger, IServiceBus serviceBus, IMapper mapper) 
        : base(logger)
    {
        _serviceBus = serviceBus;
        _mapper = mapper;
    }

    protected override async Task HandleAsync(TEvent message, CancellationToken stoppageToken)
    {
        var msgType = message.GetType();
        var typeMap = _mapper.ConfigurationProvider.Internal()
            .GetAllTypeMaps().SingleOrDefault(x => x.SourceType == msgType);

        if (typeMap is null)
            return;
        
        var serviceMessage = _mapper.Map(message, msgType, typeMap.DestinationType);
        await _serviceBus.PublishAsync(serviceMessage, stoppageToken);
    }
}