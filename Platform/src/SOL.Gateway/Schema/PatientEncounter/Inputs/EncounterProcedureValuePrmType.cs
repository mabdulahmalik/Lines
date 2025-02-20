using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureValuePrmType : InputObjectType<EncounterProcedureValue>
{
    protected override void Configure(IInputObjectTypeDescriptor<EncounterProcedureValue> descriptor)
    {
        descriptor
            .Name("EncounterProcedureValuePrm")
            .Description("The Parameters for the ProcedureField Value.");

        descriptor
            .Field(t => t.FieldId)
            .Name("fieldId")
            .Description("The unique identifier of the Procedure Field.");

        descriptor
            .Field(t => t.Value)
            .Name("value")
            .Description("The Value for the Procedure Field.");
    }
}