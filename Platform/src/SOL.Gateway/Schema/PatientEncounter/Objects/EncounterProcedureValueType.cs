using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterProcedureValueType : ObjectType<EncounterProcedureValueView>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterProcedureValueView> descriptor)
    {
        descriptor
            .Name("EncounterProcedureValue")
            .Description("The Field/Value associated with the Procedure executed for the Encounter.");

        descriptor
            .Field(t => t.FieldId)
            .Name("fieldId")
            .Description("The unique identifier for the ProcedureField.");        
        
        descriptor
            .Field(t => t.Value)
            .Name("value")
            .Description("The value ProcedureField.");        
    }
}