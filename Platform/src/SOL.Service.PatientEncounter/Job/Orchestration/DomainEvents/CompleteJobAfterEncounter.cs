using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Job.Orchestration.DomainEvents;

public class CompleteJobAfterEncounter : DomainEventHandler<EncounterProgressed>
{
    private readonly IAggregateRepository<Domain.Job> _repository;

    public CompleteJobAfterEncounter(ILogger<CompleteJobAfterEncounter> logger
        , IAggregateRepository<Domain.Job> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(EncounterProgressed message, CancellationToken stoppageToken)
    {
        if (message.Stage != EncounterStage.Completed)
            return;
        
        var job = await _repository.Get(message.JobId, stoppageToken);
        job.Complete();

        await _repository.Update(job, stoppageToken);
        await _repository.Commit(stoppageToken);        
    }
}