using Microsoft.Extensions.DependencyInjection.Extensions;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.UseCors("ReportingRestPolicy");

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //...
});

app.Run();
