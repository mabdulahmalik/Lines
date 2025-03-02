using HotChocolate.Data.Filters;
using SOL.Gateway.Views.PatientEncounter.MedicalRecord;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MedicalRecordsFilterType : FilterInputType<MedicalRecordView>
{
    protected override void Configure(IFilterInputTypeDescriptor<MedicalRecordView> descriptor)
    {
        descriptor
            .Name("MedicalRecordsFilter")
            .Description("Filters for Patient Medical Records.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Medical Record.");

        descriptor
            .Field(x => x.FacilityId)
            .Name("facilityId")
            .Description("The unique identifier of the Facility associated to the Medical Record.");
        
        descriptor
            .Field(x => x.Number)
            .Name("number")
            .Description("The Medical Record Number (MRN).");

        descriptor
            .Field(x => x.FirstName)
            .Name("firstName")
            .Description("The first name of the Patient.");

        descriptor
            .Field(x => x.LastName)
            .Name("lastName")
            .Description("The last name of the Patient.");

        descriptor
            .Field(x => x.Birthday)
            .Name("birthday")
            .Description("The birthday of the Patient.");

        descriptor
            .Field(x => x.FirstSeenOn)
            .Name("firstSeenOn")
            .Description("The date of the first encounter with the Patient.");
        
        descriptor
            .Field(x => x.LastSeenOn)
            .Name("lastSeenOn")
            .Description("The date of last (most recent) encounter with the Patient.");
        
        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date the Record was created.");        
        
        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date the Record was last modified.");
                
        descriptor
            .Field(x => x.LinesTotal)
            .Name("linesTotal")
            .Description("The Total # of Lines on this Record.");
        
        descriptor
            .Field(x => x.LinesIn)
            .Name("linesIn")
            .Description("The IN/Active # of Lines on this Record.");        
        
        descriptor
            .Field(x => x.Active)
            .Name("active")
            .Description("Whether or not the Record is ACTIVE.");        
    }
}