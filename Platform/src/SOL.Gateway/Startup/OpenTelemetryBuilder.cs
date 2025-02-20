using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using MassTransit.Logging;
using MassTransit.Monitoring;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.EntityFrameworkCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SOL.Gateway;

static class OpenTelemetryBuilder
{
    private static readonly string ServiceName = "sol-gateway";
    
    public static ILoggingBuilder AddOpenTelemetryCfg(this ILoggingBuilder builder)
    {
        builder.AddOpenTelemetry(b =>
        {
            b.IncludeScopes = true;
            b.ParseStateValues = true;
            b.IncludeFormattedMessage = true;
            b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName));
        });

        return builder;
    }

    public static IServiceCollection AddOpenTelemetryCfg(this IServiceCollection services, Action<OpenTelemetryOptions> setOpenTelemetryOptions)
    {
        var options = new OpenTelemetryOptions();
        setOpenTelemetryOptions(options);
        
        if (!String.IsNullOrWhiteSpace(options.AppInsightsKey))
        {
            services
                .AddOpenTelemetry()
                .UseAzureMonitor(o =>
                {
                    //o.Credential = new ManagedIdentityCredential();
                    o.ConnectionString = options.AppInsightsKey;
                })
                .ConfigureResource(res => res.AddService(ServiceName))
                .WithTracing(b =>
                {
                    b.AddSource(DiagnosticHeaders.DefaultListenerName); //MassTransit
                    b.AddHotChocolateInstrumentation();
                    b.AddEntityFrameworkCoreInstrumentation(ConfigureEntityFrameworkCoreInstrumentationOptions);
                })
                .WithMetrics(b =>
                {
                    b.AddMeter(InstrumentationOptions.MeterName); // MassTransit
                });

            services
                .Configure<AspNetCoreTraceInstrumentationOptions>(ConfigureAspNetCoreTraceInstrumentationOptions)
                .Configure<HttpClientTraceInstrumentationOptions>(ConfigureHttpClientTraceInstrumentationOptions);
        }
        else if (!String.IsNullOrWhiteSpace(options.OtlpEndpoint))
        {
            services
                .AddOpenTelemetry()
                .ConfigureResource(res => res.AddService(ServiceName))
                .WithTracing(b =>
                {
                    b.AddHttpClientInstrumentation(ConfigureHttpClientTraceInstrumentationOptions);
                    b.AddAspNetCoreInstrumentation(ConfigureAspNetCoreTraceInstrumentationOptions);
                    b.AddEntityFrameworkCoreInstrumentation(ConfigureEntityFrameworkCoreInstrumentationOptions);
                    b.AddHotChocolateInstrumentation();
                    b.AddSource(DiagnosticHeaders.DefaultListenerName); //MassTransit
                    b.AddOtlpExporter(o =>
                    {
                        o.Protocol = OtlpExportProtocol.HttpProtobuf;
                        o.Endpoint = new Uri(options.OtlpEndpoint);
                    });
                });
        }

        return services;
    }
    
    private static void ConfigureAspNetCoreTraceInstrumentationOptions(AspNetCoreTraceInstrumentationOptions options)
    {
        options.RecordException = true;
    }
    
    private static void ConfigureHttpClientTraceInstrumentationOptions(HttpClientTraceInstrumentationOptions options)
    {
        options.RecordException = true;
    }
    
    private static void ConfigureEntityFrameworkCoreInstrumentationOptions(EntityFrameworkInstrumentationOptions options)
    {
        options.SetDbStatementForText = true;
        options.SetDbStatementForStoredProcedure = true;
    }
}

public class OpenTelemetryOptions
{
    public string OtlpEndpoint { get; set; }
    public string AppInsightsKey { get; set; }
}