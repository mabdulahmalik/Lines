using MassTransit;
using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Job.Domain.Services;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobActivation.Activities;

public class ActivateJob : IStateMachineActivity<JobActivationState, ActivateScheduledJobs>
{
    private readonly ILogger<ActivateJob> _logger;
    private readonly JobManager _jobManager;
    private readonly IAggregateRepository<Domain.Job> _repository;

    public ActivateJob(ILogger<ActivateJob> logger, JobManager jobManager, IAggregateRepository<Domain.Job> repository)
    {
        _logger = logger;
        _jobManager = jobManager;
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
        var scheduledJobs = await _jobManager
            .GetScheduledJobs(context.Message.Date.ToDateOnly(), context.Message.FacilityIds,
                context.CancellationToken);
        var jobIds = scheduledJobs.Select(x => x.Id)
            .ToDictionary(k => k, v => ++sortOrder);
        
        _logger.LogInformation("Activating {Count} scheduled job(s) on {Date} for the following facilities: {FacilityIds}."
            , jobIds.Count, context.Message.Date, String.Join(", ", context.Message.FacilityIds));
        if (!jobIds.Any())
            return;

        var jobs= await _repository.List(new SpecificInstancesSpecification<Domain.Job>(jobIds.Keys), context.CancellationToken);
        
        foreach (var job in jobs) {
            job.Start();
            await _repository.Update(job, context.CancellationToken);
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