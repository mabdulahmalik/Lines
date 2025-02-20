using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ModifyMedicalRecordCmdType : InputObjectType<ModifyMedicalRecord>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyMedicalRecord> descriptor)
    {
        descriptor
            .Name("ModifyMedicalRecordCmd")
            .Description("The Command that modifies a Medical Record.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Medical Record.");

        descriptor
            .Field(t => t.FirstName)
            .Name("firstName")
            .Description("The First Name for the Medical Record.");

        descriptor
            .Field(t => t.LastName)
            .Name("lastName")
            .Description("The Last Name for the Medical Record.");

        descriptor
            .Field(t => t.Birthday)
            .Name("birthday")
            .Description("The Birthday for the Medical Record.");
    }
}