using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EscalateEncounterCmdType : InputObjectType<EscalateEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<EscalateEncounter> descriptor)
    {
        descriptor
            .Name("EscalateEncounterCmd")
            .Description("The Command that Escalates the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");

        descriptor
            .Field(t => t.Reason)
            .Name("reason")
            .Description("The reason behind the Escalation.");
    }
}