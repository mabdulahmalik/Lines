using AutoMapper;
using SOL.Abstractions.Storage;
using SOL.Service.PatientEncounter.Encounter.Domain;
using SE = SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter;

public class EncounterEventRelay : Profile
{
    public EncounterEventRelay()
    {
        CreateMap<CliniciansAssigned, SE.EncounterCliniciansAssigned>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Assignments, y => y.MapFrom(m => m.Assignments.Select(assignment =>
                new SE.EncounterAssignment {
                    ClinicianId = assignment.ClinicianId,
                    AssignedAt = assignment.AssignedAt,
                    Position = assignment.Position
                }).ToList()));
        
        CreateMap<CliniciansWithdrawn, SE.EncounterCliniciansWithdrawn>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.ClinicianIds, y => y.MapFrom(z => z.Assignments.Select(a => a.ClinicianId).ToList()));
        
        CreateMap<EncounterPhotosAttached, SE.EncounterPhotosAttached>()
            .ConvertUsing<EncounterPhotoConverter>();

        CreateMap<EncounterPhotosDetached, SE.EncounterPhotosDetached>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.PhotoIds, y => y.MapFrom(z => z.Photos.Select(a => a.Id).ToList()));
        
        CreateMap<EncounterPriorityChanged, SE.EncounterPriorityChanged>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Priority, y => y.MapFrom(z => z.Priority));
        
        CreateMap<EncounterProceduresApplied, SE.EncounterProceduresApplied>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Procedures, y => y.MapFrom(z => z.Procedures
                .Select(x => new SE.EncounterProcedure { 
                    Id = x.Id,
                    ProcedureId = x.ProcedureId,
                    PerformedBy = x.PerformedBy,
                    PerformedAt = x.PerformedAt,
                    Values = x.Values.Select(v => new SE.EncounterProcedureValue {
                        FieldId =  v.FieldId,
                        Value = v.Value
                    })
                })));
        
        CreateMap<EncounterProcedureModified, SE.EncounterProceduresModified>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Procedures, x => x.MapFrom(y => new List<SE.EncounterProcedure> {
                new SE.EncounterProcedure {
                    Id = y.Procedure.Id,
                    ProcedureId = y.Procedure.ProcedureId,
                    PerformedBy = y.Procedure.PerformedBy,
                    PerformedAt = y.Procedure.PerformedAt,
                    Values = y.Procedure.Values.Select(v => new SE.EncounterProcedureValue {
                        FieldId = v.FieldId,
                        Value = v.Value
                    }).ToList()
                }
            }));
        
        CreateMap<EncounterProceduresUndone, SE.EncounterProceduresUndone>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Ids, y => y.MapFrom(z => z.Procedures.Select(a => a.Id).ToList()));        
        
        CreateMap<EncounterProgressed, SE.EncounterProgressed>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Stage, y => y.MapFrom(z => z.Stage))
            .ForMember(x => x.Duration, y => y.MapFrom(z => z.Duration))
            .ForMember(x => x.EnteredAt, y => NodaTime.SystemClock.Instance.GetCurrentInstant());
        
        CreateMap<EncounterAlertedAdded, SE.EncounterAlertAdded>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.AlertedAt, y => y.MapFrom(z => z.Alert.AlertedAt))
            .ForMember(x => x.Type, y => y.MapFrom(z => z.Alert.Type))
            .ForMember(x => x.Text, y => y.MapFrom(z => z.Alert.Text))
            .ForMember(x => x.AlertedBy, y => y.MapFrom(z=> z.Alert.AlertedBy));        

        CreateMap<EncounterAlertedRemoved, SE.EncounterAlertRemoved>()
            .ForMember(x => x.EncounterId, x => x.MapFrom(y => y.EncounterId))
            .ForMember(x => x.Type, y => y.MapFrom(z => z.Type));        
    }
}

public class EncounterPhotoConverter : ITypeConverter<EncounterPhotosAttached, SE.EncounterPhotosAttached>
{
    private readonly IFileStorage _fileStorage;

    public EncounterPhotoConverter(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }
        
    public SE.EncounterPhotosAttached Convert(EncounterPhotosAttached source, SE.EncounterPhotosAttached destination,
        ResolutionContext context)
        => new SE.EncounterPhotosAttached {
            Id = source.EncounterId,
            Photos = source.Photos.Select(photo => new SE.EncounterPhoto {
                Url = _fileStorage.GetAbsoluteUrl($"encounters/{source.EncounterId}/{photo.Id}.{photo.ContentType}"),
                ContentType = photo.ContentType,
                CreatedAt = photo.CreatedAt,
                FileName = photo.FileName,
                Length = photo.Length,
                Id = photo.Id
            }).ToList()
        };
}