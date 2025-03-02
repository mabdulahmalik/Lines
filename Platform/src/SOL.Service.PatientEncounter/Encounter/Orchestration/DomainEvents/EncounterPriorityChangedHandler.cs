using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration;

public class EncounterPriorityChangedHandler : DomainEventHandler<EncounterPriorityChanged>
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IAggregateRepository<Job.Domain.Job> _jobRepository;

    public EncounterPriorityChangedHandler(ILogger<EncounterPriorityChangedHandler> logger, IOperationContextFactory operationContextFactory
        , IAggregateRepository<Job.Domain.Job> jobRepository) 
        : base(logger)
    {
        _operationContext = new(operationContextFactory.Get);
        _jobRepository = jobRepository;
    }

    protected override async Task HandleAsync(EncounterPriorityChanged message, CancellationToken stoppageToken)
    {
        if (message.Priority == EncounterPriority.Stat)
        {
            var statReason = message.Args as string ?? String.Empty;
            if(!String.IsNullOrWhiteSpace(statReason)) {
                var job = await _jobRepository.Get(message.JobId, stoppageToken);;
                
                job.PinNote(new Note(_operationContext.Value.ActorId, statReason));

                await _jobRepository.Update(job, stoppageToken);
                await _jobRepository.Commit(stoppageToken);
            }            
        }
    }
}