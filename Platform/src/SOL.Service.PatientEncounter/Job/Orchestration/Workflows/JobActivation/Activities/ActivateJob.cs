using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobActivation.Activities;

public class ActivateJob : IStateMachineActivity<JobActivationState, ActivateScheduledJobs>
{
    private readonly ILogger<ActivateJob> _logger;
    private readonly IDomainQuery<JobView> _jobsQuery;
    private readonly IAggregateRepository<Domain.Job> _repository;

    public ActivateJob(ILogger<ActivateJob> logger, IDomainQuery<JobView> jobsQuery, IAggregateRepository<Domain.Job> repository)
    {
        _logger = logger;
        _jobsQuery = jobsQuery;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ActivateJob));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobActivationState, ActivateScheduledJobs> context, IBehavior<JobActivationState, ActivateScheduledJobs> next)
    {
        var sortOrder = 0;
        var jobIds = await _jobsQuery.Query
            .Where(x => x.Status == JobStatus.Scheduled
                        && x.Schedule.Date == context.Message.Date.ToDateOnly()
                        && context.Message.FacilityIds.Contains(x.Location.FacilityId))
            .OrderBy(x => x.Location.FacilityId)
                .ThenBy(x => x.Location.Room)
            .Select(x => x.Id)
            .ToDictionaryAsync(k => k, v => ++sortOrder, context.CancellationToken);
        
        _logger.LogInformation("Activating {Count} scheduled job(s) on {Date} for the following facilities: {FacilityIds}."
            , jobIds.Count, context.Message.Date, String.Join(", ", context.Message.FacilityIds));
        if (!jobIds.Any())
            return;

        var jobs= await _repository.List(new SpecificInstancesSpecification<Domain.Job>(jobIds.Keys), context.CancellationToken);
        
        foreach (var job in jobs) {
            job.Start();
            _repository.Update(job);
        }

        context.Saga.EncounterData = jobs.Select(x => new JobEncounterData {
            JobId = x.Id,
            Order = jobIds[x.Id],
            PurposeId = x.PurposeId,
            FacilityRoomId = x.RoomId,
            MedicalRecordId = x.MedicalRecordId
        }).ToList();
        
        await _repository.Commit(context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobActivationState, ActivateScheduledJobs, TException> context
        , IBehavior<JobActivationState, ActivateScheduledJobs> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}