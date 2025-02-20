using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class DeescalateEncounterCmdType : InputObjectType<DeescalateEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeescalateEncounter> descriptor)
    {
        descriptor
            .Name("DeescalateEncounterCmd")
            .Description("The Command that Deescalates the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");
    }
}