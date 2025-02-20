using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain.Specifications;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration;

public class RemoveEncountersFromJobHandler : DomainEventHandler<JobScheduled>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public RemoveEncountersFromJobHandler(ILogger<RemoveEncountersFromJobHandler> logger
        , IAggregateRepository<Domain.Encounter> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(JobScheduled message, CancellationToken stoppageToken)
    {
        await _repository.Delete(new EncountersForJobSpecification(message.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}