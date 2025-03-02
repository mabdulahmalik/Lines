using HotChocolate.Data.Filters;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncountersFilterType : FilterInputType<EncounterView>
{
    protected override void Configure(IFilterInputTypeDescriptor<EncounterView> descriptor)
    {
        descriptor
            .Name("EncountersFilter")
            .Description("Filters the Encounters Query.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the Encounter.");
        
        descriptor
            .Field(t => t.JobId)
            .Name("jobId")
            .Description("The unique identifier for the Job associated with the Encounter.");
        
        descriptor
            .Field(t => t.PurposeId)
            .Name("purposeId")
            .Description("The Purpose of the Encounter.");        
        
        descriptor
            .Field(t => t.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The Medical Record associated with the Encounter.");           
        
        descriptor
            .Field(t => t.Location)
            .Type<LocationFilterType>()
            .Name("location")
            .Description("The Location of the Encounter.");        

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Encounter was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Encounter was modified.");
        
        descriptor
            .Field(t => t.Stage)
            .Name("status")
            .Description("The current Status of the Encounter.");

        descriptor
            .Field(t => t.Priority)
            .Name("priority")
            .Description("The Priority of the Encounter."); 
    }
}