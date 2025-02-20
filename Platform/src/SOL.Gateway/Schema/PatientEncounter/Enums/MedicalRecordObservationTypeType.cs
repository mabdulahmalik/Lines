using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MedicalRecordObservationTypeType : EnumType<ObservationType>
{
    protected override void Configure(IEnumTypeDescriptor<ObservationType> descriptor)
    {
        descriptor
            .Name("MedicalRecordObservationType")
            .Description("The Type of Observation.");
        
        descriptor
            .Value(ObservationType.Note)
            .Name("NOTE")
            .Description("The Observation is a Note.");
        
        descriptor
            .Value(ObservationType.Photo)
            .Name("PHOTO")
            .Description("The Observation is a Photo.");        
    }
}