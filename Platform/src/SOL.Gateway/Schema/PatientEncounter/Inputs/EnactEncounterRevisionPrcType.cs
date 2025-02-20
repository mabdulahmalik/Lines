using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EnactEncounterRevisionPrcType : InputObjectType<EnactEncounterRevision>
{
    protected override void Configure(IInputObjectTypeDescriptor<EnactEncounterRevision> descriptor)
    {
        descriptor
            .Name("EnactEncounterRevisionPrc")
            .Description("The Process that modifies an Encounter, and it's Job, with the supplied details.");        
        
        descriptor
            .Field(x => x.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job.");
        
        descriptor
            .Field(x => x.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");
        
        descriptor
            .Field(x => x.Contact)
            .Name("contact")
            .Description("The Contact info on the Job.");
        
        descriptor
            .Field(x => x.OrderingProvider)
            .Name("orderingProvider")
            .Description("The Ordering Provider on the Job.");
        
        descriptor
            .Field(x => x.Location)
            .Type<IntakeLocationPrmType>()
            .Name("location")
            .Description("The Location of the Encounter.");            
        
        descriptor
            .Field(x => x.MedicalRecord)
            .Type<EncounterMedicalRecordPrm>()
            .Name("medicalRecord")
            .Description("The Medical Record of the Encounter.");
    }
}

public class EncounterMedicalRecordPrm : InputObjectType<ModifyMedicalRecord>
{
    protected override void Configure(IInputObjectTypeDescriptor<ModifyMedicalRecord> descriptor)
    {
        descriptor
            .Name("EncounterMedicalRecordPrm")
            .Description("The Parameters for creating/re-assigning a Medical Record.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Medical Record.");
        
        descriptor
            .Field(x => x.Number)
            .Name("number")
            .Description("The Medical Record Number (MRN).");        

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