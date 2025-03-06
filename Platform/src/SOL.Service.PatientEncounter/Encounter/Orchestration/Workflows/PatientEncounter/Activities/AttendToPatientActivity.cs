using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class AttendToPatientActivity(IAggregateRepository<Domain.Encounter> repository, IAggregateReader<Purpose.Domain.Purpose> purposeReader)
    : AdvanceTheEncounterActivityBase<AttendToPatient>(repository)
{
    public override async Task Execute(BehaviorContext<PatientEncounterState, AttendToPatient> context, IBehavior<PatientEncounterState, AttendToPatient> next)
    {
        await AdvanceTheEncounter(context.Message.EncounterId, context.CancellationToken);
        var encounter = await repository.Get(context.Message.EncounterId, context.CancellationToken);
        var purpose = await purposeReader.Get(encounter.PurposeId!.Value, context.CancellationToken);

        var attendedToEvt = new PatientBeingAttendedTo
        {
            EncounterId = encounter.Id,
            Purpose = purpose.Name,
            Attendees = encounter.Assignments
                .Select(x => new PatientAttendee
            {
                ClinicianId = x.ClinicianId,
                Position = x.Position
            })
        };
        
        await context.Publish(attendedToEvt, context.CancellationToken);
        await next.Execute(context);
    }
}