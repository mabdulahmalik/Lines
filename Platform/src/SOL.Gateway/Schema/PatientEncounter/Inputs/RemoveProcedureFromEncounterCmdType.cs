using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RemoveProcedureFromEncounterCmdType : InputObjectType<RemoveProcedureFromEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<RemoveProcedureFromEncounter> descriptor)
    {
        descriptor
            .Name("RemoveProcedureFromEncounterCmd")
            .Description("The Command that removes a Procedure from it's Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure Instance (not the same as the ProcedureId).");
    }
}
