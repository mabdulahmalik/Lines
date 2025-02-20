using SOL.Service.PatientEncounter.MedicalRecord.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MedicalRecordObservationType : ObjectType<MedicalRecordObservationView>
{
    protected override void Configure(IObjectTypeDescriptor<MedicalRecordObservationView> descriptor)
    {
        descriptor
            .Name("MedicalRecordObservation")
            .Description("A Patient Observation tied to their Medical Record.");
        
        descriptor
            .Field(x => x.ObjectId)
            .Name("objectId")
            .Description("The unique identifier of the referenced Object.");
        
        descriptor
            .Field(x => x.Timestamp)
            .Name("timestamp")
            .Description("The date and time the Observation was recorded.");
        
        descriptor
            .Field(x => x.Type)
            .Type<MedicalRecordObservationTypeType>()
            .Name("type")
            .Description("The Type of Observation.");

        descriptor
            .Field(x => x.Data)
            .Type<JsonType>()
            .Name("data")
            .Description("The Data for the Observation. This structure is dynamic and will vary based on the Observation Type.");
    }
}