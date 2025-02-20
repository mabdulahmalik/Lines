using System.Text;
using NodaTime;
using SOL.Abstractions.Application;
using SOL.Gateway;
using SOL.Gateway.Application;
using SOL.Messaging;
using SOL.Service.OrganizationMgmt.Facility.View;
using SOL.Service.PatientEncounter.Encounter.Views;
using SOL.Service.UserMgmt.User.View;
using SOL.Workflow.JobIntake;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

var connStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Logging
    .AddOpenTelemetryCfg();

builder.Services
    .AddOpenTelemetryCfg(opt =>
    {
        opt.OtlpEndpoint = builder.Configuration["OtlpUrl"];
        opt.AppInsightsKey = connStrings["AppInsights"];
    })
    .AddSingleton<IClock>(SystemClock.Instance)
    .AddTransient<ISubscriptionHub, SubscriptionHub>()
    .AddTransient<IOperationContextFactory, OperationContextFactory>()
    .AddStackExchangeRedisCache(options => {
        options.Configuration = connStrings["RedisCache"];
    });

builder.Services
    .AddHttpContextAccessor()
    .AddGraphQLConfig(builder.Environment, opts => opts.RedisConnectionString = connStrings["RedisCache"])
    .AddMessagingSystem(opts => {
        opts.Sagas = typeof(JobIntakeStateMachine).Assembly;
        
        opts.Consumers.Add("api.gateway", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(Program).Assembly, Types = EventRelayRegistry.EventHandlers });
        opts.Consumers.Add("encounter.service", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(EncounterView).Assembly });
        opts.Consumers.Add("organization.service", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(FacilityProcedureView).Assembly });
        opts.Consumers.Add("user.service", new MessagingSystemOptions.ConsumerSource { Assembly = typeof(UserView).Assembly });

        opts.RedisConnectionString = connStrings["RedisCache"];
        opts.AzureConnectionString = connStrings["AzureStorage"];
        
        builder.Configuration.Bind("RabbitMQ", opts.RabbitMqCfg);
    })
    .AddCachingSystem()
    .AddStorageSystem(opts => opts.AzureConnectionString = connStrings["AzureStorage"])
    .AddActivityTracking(opts => opts.DatabaseConnectionString = connStrings["TenantDb"]);

builder.Services
    .AddPatientEncounterContext(opts => opts.DatabaseConnectionString = connStrings["TenantDb"])
    .AddOrganizationMgmtContext(opts => opts.DatabaseConnectionString = connStrings["TenantDb"])
    .AddUserMgmtContext(opts => opts.DatabaseConnectionString = connStrings["TenantDb"]);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

app.UseRouting();
app.UseGraphQLConfig();

app.MapGet("/", async context => {
    string ascii_art = await File.ReadAllTextAsync("ascii_art.txt");
    await context.Response.WriteAsync(ascii_art, Encoding.ASCII);
});

await app.RunWithGraphQLCommandsAsync(args);