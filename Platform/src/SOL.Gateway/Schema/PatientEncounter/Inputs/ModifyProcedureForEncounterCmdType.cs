using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ModifyProcedureForEncounterCmdType : InputObjectType<ModifyProcedureForEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyProcedureForEncounter> descriptor)
    {
        descriptor
            .Name("ModifyProcedureForEncounterCmd")
            .Description("The Command that modifies a Procedure for the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure Instance (not the same as the ProcedureId).");

        descriptor
            .Field(t => t.Values)
            .Type<ListType<EncounterProcedureValuePrmType>>()
            .Name("values")
            .Description("The list of Procedure Values.");
    }
}
