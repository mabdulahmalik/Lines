using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.EntityFrameworkCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenTelemetryBuilder
{
    public static ILoggingBuilder AddOpenTelemetryCfg(this ILoggingBuilder builder, string serviceName)
    {
        builder.AddOpenTelemetry(b =>
        {
            b.IncludeScopes = true;
            b.ParseStateValues = true;
            b.IncludeFormattedMessage = true;
            b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));
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
                .ConfigureResource(res => res.AddService(options.ServiceName!))
                .WithTracing(b =>
                {
                    b.AddEntityFrameworkCoreInstrumentation(ConfigureEntityFrameworkCoreInstrumentationOptions);
                    options.ConfigureTracerProviderBuilder(b);
                })
                .WithMetrics(b =>
                {
                    options.ConfigureMeterProviderBuilder(b);
                });

            services
                .Configure<AspNetCoreTraceInstrumentationOptions>(ConfigureAspNetCoreTraceInstrumentationOptions)
                .Configure<HttpClientTraceInstrumentationOptions>(ConfigureHttpClientTraceInstrumentationOptions);
        }
        else if (!String.IsNullOrWhiteSpace(options.OtlpEndpoint))
        {
            services
                .AddOpenTelemetry()
                .ConfigureResource(res => res.AddService(options.ServiceName!))
                .WithTracing(b =>
                {
                    b.AddHttpClientInstrumentation(ConfigureHttpClientTraceInstrumentationOptions);
                    b.AddAspNetCoreInstrumentation(ConfigureAspNetCoreTraceInstrumentationOptions);
                    b.AddEntityFrameworkCoreInstrumentation(ConfigureEntityFrameworkCoreInstrumentationOptions);
                    options.ConfigureTracerProviderBuilder(b);
                    
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
    public string? ServiceName { get; set; }
    public string? OtlpEndpoint { get; set; }
    public string? AppInsightsKey { get; set; }
    public Action<TracerProviderBuilder> ConfigureTracerProviderBuilder { get; set; } = _ => { };
    public Action<MeterProviderBuilder> ConfigureMeterProviderBuilder { get; set; } = _ => { };
}