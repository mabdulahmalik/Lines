using Microsoft.Extensions.DependencyInjection.Extensions;
using SOL.Reporting;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddOpenTelemetryCfg("lines-reporting");

builder.Services.AddOpenTelemetryCfg(opt =>
{
    opt.ServiceName = "lines-reporting";
    opt.AppInsightsKey = builder.Configuration.GetConnectionString("AppInsights");
    opt.OtlpEndpoint = builder.Configuration["OtlpUrl"];
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(), //ConfigurationHelper.ResolveConfiguration(sp.GetService<IWebHostEnvironment>()),
        HostAppId = "Net5RestServiceWithCors",
        Storage = new FileStorage(),
        ReportSourceResolver = new UriReportSourceResolver(
            Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports"))
    });

builder.Services.AddCors(corsOption => corsOption.AddPolicy(
    "ReportingRestPolicy",
    corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }
));

builder.Services.AddHealthCheckCfg(builder.Configuration.GetConnectionString("CustomerDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("ReportingRestPolicy");

app.UseRouting();

app.UseAuthorization();

app.UseHealthCheckCfg();

app.MapControllers();

app.Run();
