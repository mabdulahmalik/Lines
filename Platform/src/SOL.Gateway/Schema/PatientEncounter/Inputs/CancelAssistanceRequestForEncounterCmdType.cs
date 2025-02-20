using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class CancelAssistanceRequestForEncounterCmdType : InputObjectType<CancelAssistanceRequestForEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<CancelAssistanceRequestForEncounter> descriptor)
    {
        descriptor
            .Name("CancelAssistanceRequestForEncounterCmd")
            .Description("Command to cancel assistance request for a specific encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the encounter for which assistance request is being canceled.");
    }
}


