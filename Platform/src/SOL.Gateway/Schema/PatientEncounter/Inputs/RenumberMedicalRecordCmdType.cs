using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RenumberMedicalRecordCmdType : InputObjectType<RenumberMedicalRecord>
{
    protected override void Configure(IInputObjectTypeDescriptor<RenumberMedicalRecord> descriptor)
    {
        descriptor
            .Name("RenumberMedicalRecordCmd")
            .Description("Changes the Number of a Medical Record by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Medical Record.");      
        
        descriptor
            .Field(x => x.Number)
            .Name("number")
            .Description("The new Number for the Medical Record.");      
    }
}