using System.Reflection;
using AutoMapper;
using Azure.Storage.Blobs;
using FluentValidation;
using MassTransit;
using MassTransit.Internals;
using Microsoft.Extensions.DependencyInjection;
using NodaTime.Serialization.SystemTextJson;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Infrastructure;
using SOL.Messaging.Infrastructure.Filters;
using SOL.Messaging.Infrastructure.Handlers;
using SOL.Messaging.Infrastructure.Schedules;
using SOL.Messaging.Validators;

namespace SOL.Messaging;

public static class Startup
{
    public static IServiceCollection AddMessagingSystem(this IServiceCollection services, Action<MessagingSystemOptions> setMessagingSystemOptions)
    {
        var opts = new MessagingSystemOptions();
        setMessagingSystemOptions(opts);
        
        var consumerAssemblies = opts.Consumers.Select(x => x.Value.Assembly)
            .Union(new[] { typeof(Startup).Assembly }).ToArray();
        var consumerTypes = opts.Consumers.SelectMany(x => x.Value.Types).ToArray();

        services
            .AddTransient<IServiceBus, ServiceEventBus>()
            .AddTransient<IDomainBus, DomainEventBus>()
            .AddTransient<ICommandBus, CommandBus>()
            .AddAutoMapper(consumerAssemblies);
        
        services
            .AddTransient<ICommandScheduler, CommandScheduler>()
            .Decorate<ICommandScheduler, SingleScheduleDecorator>()
            .Scan(scanner => scanner
                .FromAssemblyOf<ISingleScheduler>()
                .AddClasses(tf => tf.AssignableTo<ISingleScheduler>())
                .As<ISingleScheduler>().WithTransientLifetime()
            );
        
        services.Scan(scanner => scanner
            .FromAssemblyOf<EnactJobIntakeValidator>()
            .AddClasses(tf => tf.AssignableTo<IValidator>())
            .As<IValidator>().WithTransientLifetime()
        );

        services.AddMassTransit(x => 
        {
            x.AddConsumer<FaultedMessageHandler>();
            x.AddConsumers(t => t.HasInterface<IServiceHandler>(), consumerAssemblies);
            x.AddConsumers(t => t.HasInterface<IServiceHandler>(), consumerTypes);
            x.AddConsumers(t => t.HasInterface<IServiceHandler>(), GetDomainServiceBridges(consumerAssemblies));
            
            x.SetRedisSagaRepositoryProvider(cfg => {
                cfg.DatabaseConfiguration(opts.ConnStrings.RedisCache);
                cfg.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                cfg.Expiry = TimeSpan.FromDays(2);
                cfg.KeyPrefix = "mt-saga-svc";
                cfg.LockSuffix = "-lock";
            });

            if (opts.Sagas != null)
            {
                x.AddSagaStateMachines(opts.Sagas);
                x.AddActivities(opts.Sagas);   
            }

            var messageDataRepository = GetMessagingRepository(opts.ConnStrings.AzureStorage, "mt-claim-check");
            services.AddTransient<IMessageDataRepository>(_ => messageDataRepository);

            if (!String.IsNullOrWhiteSpace(opts.ConnStrings.AzureServiceBus))
            {
                x.AddServiceBusMessageScheduler();
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(opts.ConnStrings.AzureServiceBus);
                    cfg.ConfigureJsonSerializerOptions(o => {
                        o.ConfigureForNodaTime(new NodaJsonSettings());
                        return o;
                    });
                    /*
                    cfg.UseTransaction(x => {
                        x.Timeout = TimeSpan.FromSeconds(60);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });
                    */
                    cfg.UseServiceBusMessageScheduler();
                    cfg.UsePublishFilter(typeof(PublishMessageHeadersFilter<>), context);
                    cfg.UseConsumeFilter(typeof(ConsumeMessageHeadersFilter<>), context);
                    
                    cfg.UseMessageData(messageDataRepository);
                    cfg.UseMessageScope(context);
                    cfg.UseInMemoryOutbox(context);
                    cfg.ConfigureEndpoints(context);
                    
                    /* TODO: Revisit this later (to complete it)
                    opts.Consumers.ToList().ForEach(c => {
                        var commandHandlers = c.Value.Assembly.GetTypes().Union(c.Value.Types)
                            .Where(t => t.HasInterface<IServiceHandler>() && t.BaseType!.Name == "CommandHandler`1")
                            .ToList();
                        
                        if (commandHandlers.Any())
                            cfg.ReceiveEndpoint(c.Key,
                                e => commandHandlers.ForEach(t => e.ConfigureConsumer(context, t)));
                        
                        var eventHandlers = c.Value.Assembly.GetTypes().Union(c.Value.Types)
                            .Where(t => t.HasInterface<IServiceHandler>() && t.BaseType!.Name == "ServiceEventHandler`1")
                            .ToList();

                        if (eventHandlers.Any())
                        {
                            cfg.SubscriptionEndpoint("Self", c.Key,
                                s => eventHandlers.ForEach(t => s.ConfigureConsumer(context, t)));
                        }
                    });
                    
                    cfg.ReceiveEndpoint("faulted-messages", e => {
                        e.ConfigureConsumer<FaultedMessageHandler>(context);
                    });
                    */
                });
            }
            else if(opts.RabbitMqCfg.IsValid)
            {
                x.AddDelayedMessageScheduler();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(opts.RabbitMqCfg.Host, "/", h => {
                        h.Username(opts.RabbitMqCfg.Username);
                        h.Password(opts.RabbitMqCfg.Password);
                    });

                    cfg.ConfigureJsonSerializerOptions(o => {
                        o.ConfigureForNodaTime(new NodaJsonSettings());
                        return o;
                    });
                    /*
                    cfg.UseTransaction(x => {
                        x.Timeout = TimeSpan.FromSeconds(60);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });
                    */
                    cfg.UseDelayedMessageScheduler();
                    cfg.UsePublishFilter(typeof(PublishMessageHeadersFilter<>), context);
                    cfg.UseConsumeFilter(typeof(ConsumeMessageHeadersFilter<>), context);
                    
                    MessageDataDefaults.TimeToLive = TimeSpan.FromDays(2);
                    
                    cfg.UseMessageData(messageDataRepository);
                    cfg.UseMessageScope(context);
                    cfg.UseInMemoryOutbox(context);
                    cfg.ConfigureEndpoints(context);
                });
            }
        });

        services.AddMediator(x => {
            x.AddConsumers(t => t.HasInterface<IDomainHandler>(), typeof(Startup).Assembly);
            x.AddConsumers(t => t.HasInterface<IDomainHandler>(), consumerAssemblies);
            x.AddConsumers(t => t.HasInterface<IDomainHandler>(), consumerTypes);
            x.AddConsumers(t => t.HasInterface<IDomainHandler>(), GetDomainRelayHandlers(consumerAssemblies));
            
            x.SetRedisSagaRepositoryProvider(cfg => {
                cfg.DatabaseConfiguration(opts.ConnStrings.RedisCache);
                cfg.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                cfg.Expiry = TimeSpan.FromDays(5);
                cfg.KeyPrefix = "mt-saga-dmn";
                cfg.LockSuffix = "-lock";
            });
            x.AddSagaStateMachines(consumerAssemblies);
            x.AddActivities(consumerAssemblies);

            x.ConfigureMediator((context, cfg) => 
            {
                cfg.UsePublishFilter(typeof(PublishMessageHeadersFilter<>), context);
                cfg.UseConsumeFilter(typeof(ConsumeMessageHeadersFilter<>), context);
                
                cfg.UseMessageScope(context);
                cfg.UseInMemoryOutbox(context);
            });
        });      
        
        return services;
    }
    
    static IMessageDataRepository GetMessagingRepository(string connectionString, string containerName)
    {
        var serviceClient = new BlobServiceClient(connectionString);
        var containerClient = serviceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
        
        return serviceClient.CreateMessageDataRepository(containerName);
    }

    static Type[] GetDomainRelayHandlers(params Assembly[] assemblies)
    {
        var results = new List<Type>();
        var relayHandler = typeof(DomainRelayHandler<>);

        foreach (var assembly in assemblies)
        {
            var profiles = assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(Profile)) && t.Name.Contains("EventRelay"))
                .ToArray();

            foreach (var profileType in profiles)
            {
                var profile = (IProfileConfiguration)Activator.CreateInstance(profileType)!;
                results.AddRange(profile.TypeMapConfigs
                    .Select(profileTypeMapConfig => profileTypeMapConfig.SourceType)
                    .Where(srcType => srcType.IsAssignableTo(typeof(IMessage)))
                    .Select(msgSrcType => relayHandler.MakeGenericType(msgSrcType)));
            }
        }

        return results.ToArray();
    }
    
    static Type[] GetDomainServiceBridges(params Assembly[] assemblies)
    {
        var messages = new List<Type>();
        var bridgeHandler = typeof(DomainServiceBridge<>);

        foreach (var assembly in assemblies.Where(a => a.GetTypes().Any(t => t.IsConcreteAndAssignableTo<StateMachine>())))
        {
            var stateMachines = assembly.GetTypes()
                .Where(t => t.IsConcreteAndAssignableTo<StateMachine>())
                .ToArray();

            foreach (var sm in stateMachines)
            {
                var contracts = sm.GetProperties()
                    .Where(p => p.PropertyType.IsAssignableTo(typeof(Event)) 
                                && p.PropertyType.GetGenericArguments().First().IsInNamespace("SOL.Messaging.Contracts"))
                    .Select(p => p.PropertyType.GetGenericArguments().First())
                    .ToArray();
                
                messages.AddRange(contracts.Except(messages));
            }
        }

        return messages.Select(ct => bridgeHandler.MakeGenericType(ct)).ToArray();
    }
}

public class MessagingSystemOptions
{
    public Assembly? Sagas { get; set; }
    public Dictionary<string, ConsumerSource> Consumers { get; } = new();
    public ConnectionStrings ConnStrings { get; set; } = new();
    public RabbitMq RabbitMqCfg { get; set; } = new();

    public class RabbitMq {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsValid => !String.IsNullOrWhiteSpace(Host) && !String.IsNullOrWhiteSpace(Username) &&
                               !String.IsNullOrWhiteSpace(Password);
    }
    
    public class ConsumerSource {
        public Assembly Assembly { get; set; }
        public IEnumerable<Type> Types { get; set; } = new List<Type>();
    }
    
    public class ConnectionStrings {
        public string RedisCache { get; set; } = default!;
        public string AzureStorage { get; set; } = default!;
        public string AzureServiceBus { get; set; } = default!;
    }
}