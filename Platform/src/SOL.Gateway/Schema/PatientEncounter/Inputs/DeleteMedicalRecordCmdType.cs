using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class DeleteMedicalRecordCmdType : InputObjectType<DeleteMedicalRecord>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeleteMedicalRecord> descriptor)
    {
        descriptor
            .Name("DeleteMedicalRecordCmd")
            .Description("Deletes a Medical Record by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Medical Record.");           
    }
}