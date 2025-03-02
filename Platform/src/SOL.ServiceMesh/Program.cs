using System.Reflection;
using System.Text;
using MassTransit.Logging;
using MassTransit.Monitoring;
using NodaTime;
using SOL.Abstractions.Application;
using SOL.Messaging;
using SOL.ServiceMesh;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

var connStrings = new ConnectionStrings();
builder.Configuration.Bind("ConnectionStrings", connStrings);

builder.Logging
    .AddOpenTelemetryCfg("service-mesh");

builder.Services
    .AddSingleton<IClock>(SystemClock.Instance)
    .AddTransient<IOperationContextFactory, OperationContextFactory>()
    .AddOpenTelemetryCfg(opt =>
    {
        opt.ServiceName = "service-mesh";
        opt.AppInsightsKey = connStrings.AppInsights;
        opt.OtlpEndpoint = builder.Configuration["OtlpUrl"];
        
        opt.ConfigureTracerProviderBuilder = b =>
        {
            b.AddSource(DiagnosticHeaders.DefaultListenerName); //MassTransit
        };
        opt.ConfigureMeterProviderBuilder = b =>
        {
            b.AddMeter(InstrumentationOptions.MeterName); // MassTransit
        };
    });

builder.Services
    .AddPatientEncounterContext(opts => opts.DatabaseConnectionString = connStrings.TenantDb)
    .AddOrganizationMgmtContext(opts => opts.DatabaseConnectionString = connStrings.TenantDb)
    .AddUserMgmtContext(opts => opts.DatabaseConnectionString = connStrings.TenantDb)
    .AddTenantMgmtContext();

builder.Services
    .AddHealthCheckCfg(connStrings)
    .AddCachingSystem(opts => opts.RedisConnectionString = connStrings.RedisCache)
    .AddStorageSystem(opts => opts.AzureConnectionString = connStrings.AzureStorage)
    .AddActivityTracking(opts => opts.DatabaseConnectionString = connStrings.TenantDb)
    .AddMessagingSystem(opts => {
        opts.Sagas = Assembly.GetExecutingAssembly();
        
        opts.Consumers.Add("service.organizations", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(OrganizationMgmtOptions).Assembly });
        opts.Consumers.Add("service.encounters", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(PatientEncounterOptions).Assembly });
        opts.Consumers.Add("service.users", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(UserMgmtOptions).Assembly });
        
        builder.Configuration.Bind("ConnectionStrings", opts.ConnStrings);
        builder.Configuration.Bind("RabbitMQ", opts.RabbitMqCfg);
    })
    .AddIdPClients(opts => builder.Configuration.Bind("Keycloak", opts));

var app = builder.Build();

app.UseHealthCheckCfg();

app.MapGet("/", async context => {
    var ascii_art = await File.ReadAllTextAsync("ascii_art.txt");
    await context.Response.WriteAsync(ascii_art, Encoding.ASCII);
});

app.Run();