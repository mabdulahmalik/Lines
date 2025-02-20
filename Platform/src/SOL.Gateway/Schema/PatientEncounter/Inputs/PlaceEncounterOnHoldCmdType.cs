using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class PlaceEncounterOnHoldCmdType : InputObjectType<PlaceEncounterOnHold>
{
    protected override void Configure(IInputObjectTypeDescriptor<PlaceEncounterOnHold> descriptor)
    {
        descriptor
            .Name("PlaceEncounterOnHoldCmd")
            .Description("Command that holds the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the encounter.");

        descriptor
            .Field(t => t.Reason)
            .Name("reason")
            .Description("The Reason for encounter being Hold.");
    }
}


