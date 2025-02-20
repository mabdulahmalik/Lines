using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EnactListRevisionPrcType : InputObjectType<EnactLineRevision>
{
    protected override void Configure(IInputObjectTypeDescriptor<EnactLineRevision> descriptor)
    {
        descriptor
            .Name("EnactLineRevisionPrc")
            .Description("The Process that modifies a Line with the supplied details.");        
        
        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");
        
        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Line.");
        
        descriptor
            .Field(x => x.Location)
            .Type<IntakeLocationPrmType>()
            .Name("location")
            .Description("The Location of the Line.");            
        
        descriptor
            .Field(x => x.MedicalRecord)
            .Type<EncounterMedicalRecordPrm>()
            .Name("medicalRecord")
            .Description("The Medical Record of the Line.");
    }
}