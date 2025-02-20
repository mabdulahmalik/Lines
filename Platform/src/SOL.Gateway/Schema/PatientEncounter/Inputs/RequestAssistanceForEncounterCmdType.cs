using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RequestAssistanceForEncounterCmdType : InputObjectType<RequestAssistanceForEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<RequestAssistanceForEncounter> descriptor)
    {
        descriptor
            .Name("RequestAssistanceForEncounterCmd")
            .Description("Command to request assistance for a specific encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the encounter for which assistance is being requested.");

        descriptor
            .Field(t => t.Reason)
            .Name("reason")
            .Description("The reason for requesting assistance for the encounter.");
    }
}


