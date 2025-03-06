using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.UserMgmt.User.Orchestration.ServiceEvents;

public class PatientBeingAttendedToHandler : ServiceEventHandler<PatientBeingAttendedTo>
{
    private readonly IAggregateRepository<Domain.User> _repository;

    public PatientBeingAttendedToHandler(ILogger<PatientBeingAttendedToHandler> logger
        , IAggregateRepository<Domain.User> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(PatientBeingAttendedTo message, CancellationToken stoppageToken)
    {
        foreach (var attendee in message.Attendees)
        {
            var user = await _repository.Get(attendee.ClinicianId, stoppageToken);
            var statusMsg = attendee.Position switch
            {
                EncounterAssigneePosition.Primary => $"Leading a {message.Purpose}.",
                EncounterAssigneePosition.Assistant => $"Assisting with a {message.Purpose}.",
                EncounterAssigneePosition.Trainee => $"Training on a {message.Purpose}.",
                _ => throw new ArgumentOutOfRangeException()
            };

            if (attendee.Position == EncounterAssigneePosition.Primary &&
                message.Attendees.All(x => x.ClinicianId == attendee.ClinicianId))
            {
                statusMsg = $"Attending to a {message.Purpose}.";
            }
            
            user.ChangeStatus(UserAvailability.Busy, statusMsg);
            
            await _repository.Update(user, stoppageToken);
        }
        
        await _repository.Commit(stoppageToken);
    }
}