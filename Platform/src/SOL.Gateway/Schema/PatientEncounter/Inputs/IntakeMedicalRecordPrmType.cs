using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class IntakeMedicalRecordPrmType : InputObjectType<IntakeMedicalRecord>
{
    protected override void Configure(IInputObjectTypeDescriptor<IntakeMedicalRecord> descriptor)
    {
        descriptor
            .Name("IntakeMedicalRecordPrm")
            .Description("The Medical Record info to attach to a Job.");
        
        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Medical Record.");
        
        descriptor
            .Field(t => t.Number)
            .Name("number")
            .Description("The NUMBER (should be unique) for the Medical Record.");           
    }
}