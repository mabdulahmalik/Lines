using AutoMapper;
using SOL.Service.PatientEncounter.MedicalRecord.Domain;
using SE = SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.MedicalRecord;

using MedicalRecordObservationsAdded = MedicalRecordObservationsAdded;
using MedicalRecordObservationsRemoved = MedicalRecordObservationsRemoved;

public class MedicalRecordEventRelay : Profile
{
    public MedicalRecordEventRelay()
    {
        CreateMap<MedicalRecordObservationsAdded, SE.MedicalRecordObservationsAdded>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.MedicalRecordId))
            .ForMember(x => x.Observations,
                y => y.MapFrom(z => z.Observations
                    .Select(obs => new SE.MedicalRecordObservation { ObjectId = obs.ObjectId, Timestamp = obs.Timestamp, Type = obs.Type })));
        
        CreateMap<MedicalRecordObservationsRemoved, SE.MedicalRecordObservationsRemoved>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.MedicalRecordId))
            .ForMember(x => x.Observations,
                y => y.MapFrom(z => z.Observations
                    .Select(obs => new SE.MedicalRecordObservation { ObjectId = obs.ObjectId, Timestamp = obs.Timestamp, Type = obs.Type })));
    }
}