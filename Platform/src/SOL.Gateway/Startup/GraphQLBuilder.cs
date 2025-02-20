using System.Reflection;
using HotChocolate.Data.Filters;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.NodaTime;
using SOL.Gateway.Conventions;
using SOL.Gateway.Interceptors;
using SOL.Gateway.Schema;
using StackExchange.Redis;

namespace SOL.Gateway;

static class GraphQLBuilder
{
    public static IServiceCollection AddGraphQLConfig(this IServiceCollection services, IWebHostEnvironment env, Action<GraphQLOptions> setOptions)
    {
        var options = new GraphQLOptions();
        setOptions(options);

        services
            .AddGraphQLServer()
            .InitializeOnStartup()
            .ModifyOptions(o =>
            {
                o.DefaultBindingBehavior = BindingBehavior.Explicit;
                o.RemoveUnreachableTypes = true;
            })
            .ModifyPagingOptions(o =>
            {
                o.IncludeTotalCount = true;
                o.DefaultPageSize = 250;
                o.MaxPageSize = 1000;
            })
            .ModifyCostOptions(o => o.EnforceCostLimits = false)
            .ModifyRequestOptions(o => o.IncludeExceptionDetails = env.IsDevelopment())
            .AddHttpRequestInterceptor<HttpRequestInterceptor>()
            .AddSocketSessionInterceptor<SocketSessionInterceptor>()
            .AddConvention<IFilterConvention, EnumFilterConvention>()
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .AddInstrumentation()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddRedisSubscriptions(sp => ConnectionMultiplexer.Connect(options.RedisConnectionString))
            .LoadTypeExtensions()
            .LoadAssignableTypes<InputObjectType>()
            .LoadAssignableTypes<ObjectType>()
            .LoadAssignableTypes<EnumType>()
            .LoadNodaTimeTypes()
            .AddType<UploadType>();
        
        return services;
    }

    public static void UseGraphQLConfig(this WebApplication app)
    {
        app.UseWebSockets();
        app.MapGraphQL();
    }

    private static IRequestExecutorBuilder LoadTypeExtensions(this IRequestExecutorBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes().Where(ty 
            => ty.IsAssignableTo(typeof(ObjectTypeExtension))).ToList();
        
        types.ForEach(ty => builder.AddTypeExtension(ty));

        return builder;
    }
    
    private static IRequestExecutorBuilder LoadAssignableTypes<T>(this IRequestExecutorBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes().Where(ty 
            => ty.IsAssignableTo(typeof(T))).ToList();
        
        types.ForEach(ty => builder.AddType(ty));

        return builder;
    }
    
    private static IRequestExecutorBuilder LoadNodaTimeTypes(this IRequestExecutorBuilder builder)
    {
        builder
            .AddType<InstantType>()
            .AddType<LocalDateType>()
            .AddType<LocalTimeType>()
            .AddType<DurationType>()
            .AddType<IsoDayOfWeekType>();

        return builder;
    }
}

public class GraphQLOptions
{
    public string RedisConnectionString { get; set; }
}