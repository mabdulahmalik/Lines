using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.ServiceEvents;

public class EncounterCompletedHandler : ServiceEventHandler<EncounterCompleted>
{
    private readonly IAggregateRepository<Domain.User> _repository;
    
    public EncounterCompletedHandler(ILogger<EncounterCompletedHandler> logger
        , IAggregateRepository<Domain.User> repository)
        : base(logger)
    {
        _repository = repository;
    }
    
    protected override async Task HandleAsync(EncounterCompleted message, CancellationToken stoppageToken)
    {
        foreach (var attendee in message.Attendees)
        {
            var user = await _repository.Get(attendee.ClinicianId, stoppageToken);

            if (user.Status.Availability != UserAvailability.Busy || String.IsNullOrWhiteSpace(user.Status.Message) ||
                !user.Status.Message.Contains(message.Purpose)) 
                continue;
            
            user.ChangeStatus(UserAvailability.Free, $"Finished a {message.Purpose}.");
            await _repository.Update(user, stoppageToken);
        }
        
        await _repository.Commit(stoppageToken);
    }
}