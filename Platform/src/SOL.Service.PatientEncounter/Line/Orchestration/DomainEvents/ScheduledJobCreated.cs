using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Line.Orchestration;

public class ScheduledJobCreated : DomainEventHandler<AggregateCreated>
{
    private readonly IAggregateRepository<Domain.Line> _repository;
    private readonly IAggregateReader<Job.Domain.Job> _jobReader;

    public ScheduledJobCreated(ILogger<ScheduledJobCreated> logger
        , IAggregateRepository<Domain.Line> repository
        , IAggregateReader<Job.Domain.Job> jobReader) 
        : base(logger)
    {
        _repository = repository;
        _jobReader = jobReader;
    }

    protected override async Task HandleAsync(AggregateCreated message, CancellationToken stoppageToken)
    {
        if (message.Name != nameof(Job.Domain.Job))
            return;
        
        var activeJob = await _jobReader.Get(message.Id, stoppageToken);
        if (activeJob.Status != JobStatus.Scheduled || !activeJob.LineId.HasValue)
            return;
        
        var line = await _repository.Get(activeJob.LineId.Value, stoppageToken);
        if (line.FollowUpId.HasValue)
        {
            var existingJob = await _jobReader.Get(line.FollowUpId.Value, stoppageToken);
            if (existingJob.ScheduledDate > activeJob.ScheduledDate) 
            {
                line.FollowUp(activeJob.Id);
                await _repository.Update(line, stoppageToken);
            }
        }
        else
        {
            line.FollowUp(activeJob.Id);
            await _repository.Update(line, stoppageToken);
        }
        
        await _repository.Commit(stoppageToken);
    }
}