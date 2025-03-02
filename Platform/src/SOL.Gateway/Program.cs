using MassTransit.Logging;
using MassTransit.Monitoring;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using OpenTelemetry.Trace;
using SOL.Abstractions.Application;
using SOL.Gateway;
using SOL.Gateway.Application;
using SOL.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

var connStrings = new ConnectionStrings();
builder.Configuration.Bind("ConnectionStrings", connStrings);

builder.Logging
    .AddOpenTelemetryCfg("lines-api");

builder.Services
    .AddSingleton<IClock>(SystemClock.Instance)
    .AddTransient<ISubscriptionHub, SubscriptionHub>()
    .AddTransient<IOperationContextFactory, OperationContextFactory>()
    .AddPooledDbContextFactory<LinesDataStore>(dbOpts =>
    {
        dbOpts.UseSqlServer(connStrings.TenantDb, sqlOpts => {
            sqlOpts.UseNodaTime();
            sqlOpts.UseAzureSqlDefaults();
            sqlOpts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
    })
    .AddOpenTelemetryCfg(opt =>
    {
        opt.ServiceName = "lines-api";
        opt.AppInsightsKey = connStrings.AppInsights;
        opt.OtlpEndpoint = builder.Configuration["OtlpUrl"];
        
        opt.ConfigureTracerProviderBuilder = b =>
        {
            b.AddSource(DiagnosticHeaders.DefaultListenerName); //MassTransit
            b.AddHotChocolateInstrumentation();
        };
        opt.ConfigureMeterProviderBuilder = b =>
        {
            b.AddMeter(InstrumentationOptions.MeterName); // MassTransit
        };
    });

builder.Services
    .AddHttpContextAccessor()
    .AddHealthCheckCfg(connStrings)
    .AddGraphQLCfg(builder.Environment, opts => opts.RedisConnectionString = connStrings.RedisCache)
    .AddMessagingSystem(opts => {
        opts.Consumers.Add("api.lines", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(Program).Assembly, Types = EventRelayRegistry.EventHandlers });
        
        builder.Configuration.Bind("ConnectionStrings", opts.ConnStrings);
        builder.Configuration.Bind("RabbitMQ", opts.RabbitMqCfg);
    })
    .AddCachingSystem(opts => opts.RedisConnectionString = connStrings.RedisCache)
    .AddStorageSystem(opts => opts.AzureConnectionString = connStrings.AzureStorage)
    .AddActivityTracking(opts => opts.DatabaseConnectionString = connStrings.TenantDb)
    .AddIdPClients(opts => builder.Configuration.Bind("Keycloak", opts));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<DataBootstrap>();

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});
app.UseStaticFiles(new StaticFileOptions()
{
    HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.DoNotCompress,
    OnPrepareResponse = (context) =>
    {
        if (context.File.Name.EndsWith(".js") ||
            context.File.Name.EndsWith(".html"))
        {
            context.Context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromDays(0)
                };
        }
    }
});
app.UseRouting();
app.UseWebSockets();
app.UseGraphQLCfg();
app.UseHealthCheckCfg();
app.MapFallbackToFile("index.html");

await app.RunWithGraphQLCommandsAsync(args);