using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration;

public class EncounterAlertedAddedHandler : DomainEventHandler<EncounterAlertedAdded>
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IAggregateRepository<Job.Domain.Job> _jobRepository;

    public EncounterAlertedAddedHandler(
        ILogger<EncounterAlertedAddedHandler> logger,
        IOperationContextFactory operationContextFactory,
        IAggregateRepository<Job.Domain.Job> jobRepository)
        : base(logger)
    {
        _operationContext = new(operationContextFactory.Get);
        _jobRepository = jobRepository;
    }

    protected override async Task HandleAsync(EncounterAlertedAdded message, CancellationToken stoppageToken)
    {
        if(message.Alert.Type == EncounterAlertType.OnHold || message.Alert.Type == EncounterAlertType.AssistanceRequested)
        {
            if(!String.IsNullOrWhiteSpace(message.Alert.Text)) {
                var noteTreatment = message.Alert.Type switch {
                    EncounterAlertType.OnHold => JobNoteTreatment.ReasonHold,
                    EncounterAlertType.AssistanceRequested => JobNoteTreatment.ReasonSos
                };
                
                var job = await _jobRepository.Get(message.JobId, stoppageToken);
                
                job.AddNotes(new Note(Guid.NewGuid(), message.Alert.AlertedAt
                    , _operationContext.Value.ActorId, message.Alert.Text, noteTreatment));

                _jobRepository.Update(job);
                await _jobRepository.Commit(stoppageToken);
            }
        }
    }
}