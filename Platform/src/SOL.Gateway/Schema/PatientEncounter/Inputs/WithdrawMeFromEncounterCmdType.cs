using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class WithdrawMeFromEncounterCmdType : InputObjectType<WithdrawMeFromEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<WithdrawMeFromEncounter> descriptor)
    {
        descriptor
            .Name("WithdrawMeFromEncounterCmd")
            .Description("The Command that withdraws the active user from the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");
    }
}