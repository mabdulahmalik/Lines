namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAlertTypeType : EnumType<Abstractions.Domain.EncounterAlertType>
{
    protected override void Configure(IEnumTypeDescriptor<Abstractions.Domain.EncounterAlertType> descriptor)
    {
        descriptor
            .Name("EncounterAlertType")
            .Description("The type of Alert for an Encounter.");

        descriptor.Value(Abstractions.Domain.EncounterAlertType.OnHold)
            .Name("ON_HOLD")
            .Description("The Encounter has been put on Hold.");

        descriptor.Value(Abstractions.Domain.EncounterAlertType.AppointmentScheduled)
            .Name("APPOINTMENT_SCHEDULED")
            .Description("The Encounter has been scheduled for a specific time.");

        descriptor.Value(Abstractions.Domain.EncounterAlertType.AssistanceRequested)
            .Name("ASSISTANCE_REQUESTED")
            .Description("An Attendee has requested help with this Encounter.");
    }
}