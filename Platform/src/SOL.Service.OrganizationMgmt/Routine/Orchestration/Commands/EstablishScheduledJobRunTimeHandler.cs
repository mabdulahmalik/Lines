using Microsoft.Extensions.Logging;
using NodaTime;
using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.OrganizationMgmt.Facility.Domain.Specifications;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Commands;

public class EstablishScheduledJobRunTimeHandler : CommandHandler<EstablishScheduledJobRunTime>
{
    private readonly IAggregateReader<Facility.Domain.Facility> _facilityReader;
    private readonly ICommandScheduler _commandScheduler;
    private readonly IServiceBus _serviceBus;

    public EstablishScheduledJobRunTimeHandler(ILogger<EstablishScheduledJobRunTimeHandler> logger
        , IAggregateReader<Facility.Domain.Facility> facilityReader
        , ICommandScheduler commandScheduler
        , IServiceBus serviceBus) 
        : base(logger)
    {
        _serviceBus = serviceBus;
        _facilityReader = facilityReader;
        _commandScheduler = commandScheduler;
    }

    protected override async Task HandleAsync(EstablishScheduledJobRunTime command, CancellationToken stoppageToken)
    {
        var facility = await _facilityReader.Get(command.FacilityId, stoppageToken);
        var facilitiesInTimeZone = await _facilityReader.List(new FacilitiesInTimeZoneSpecification(facility.TimeZone), stoppageToken);
        var facilityIds = facilitiesInTimeZone.Select(x => x.Id).ToList();
        
        var facilityZone = DateTimeZoneProviders.Tzdb[facility.TimeZone];
        var scheduledTime = command.Date.AtMidnight().InZoneLeniently(facilityZone);
        
        var scheduledJobCmd = new ActivateScheduledJobs {
            RunTime = scheduledTime.ToInstant(),
            TimeZone = facility.TimeZone,
            FacilityIds = facilityIds,
            Date = scheduledTime.Date
        };

        await _commandScheduler.Schedule(scheduledJobCmd, scheduledTime, stoppageToken);
        Logger.LogInformation("Established Scheduled Job RunTime on {Date} for '{FacilityName}' [{FacilityId}].",
            scheduledTime.Date, facility.Name, facility.Id);

        await _serviceBus.PublishAsync(new ScheduledJobRunTimeEstablished {
            RunTime = scheduledTime.ToInstant(),
            TimeZone = facility.TimeZone,
            Date = scheduledTime.Date,
            FacilityIds = facilityIds,
            JobId = command.JobId
        }, stoppageToken);
    }
}