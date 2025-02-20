using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RemoveHoldFromEncounterCmdType : InputObjectType<RemoveHoldFromEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<RemoveHoldFromEncounter> descriptor)
    {
        descriptor
            .Name("RemoveHoldFromEncounterCmd")
            .Description("The Command that removes the hold from the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");
    }
}


