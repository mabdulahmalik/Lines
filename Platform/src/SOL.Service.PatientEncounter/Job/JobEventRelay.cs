using AutoMapper;
using SOL.Service.PatientEncounter.Job.Domain;
using SE = SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Job;

public class JobEventRelay : Profile
{
    public JobEventRelay()
    {
        CreateMap<JobScheduled, SE.JobRescheduled>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Reason, x => x.MapFrom(y => y.Reason))
            .ForMember(x => x.ScheduledBy, x => x.MapFrom(y => y.ScheduledBy))
            .ForMember(x => x.ScheduledDate, x => x.MapFrom(y => y.ScheduledDate))
            .ForMember(x => x.ScheduledTime, x => x.MapFrom(y => y.ScheduledTime));
        
        CreateMap<Note, SE.JobNote>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Text, x => x.MapFrom(y => y.Text))
            .ForMember(x => x.CreatedBy, x => x.MapFrom(y => y.UserId))
            .ForMember(x => x.CreatedAt, x => x.MapFrom(y => y.CreatedAt))
            .ForMember(x => x.Treatment, x => x.MapFrom(y => y.Treatment));
        
        CreateMap<JobNotesAdded, SE.JobNotesAdded>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.JobId))
            .ForMember(x => x.Notes, y => y.MapFrom(z=> z.Notes));
        
        CreateMap<JobNotesModified, SE.JobNotesModified>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.JobId))
            .ForMember(x => x.Notes, y => y.MapFrom(z=> z.Notes));
        
        CreateMap<JobNotesRemoved, SE.JobNotesRemoved>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.JobId))
            .ForMember(x => x.NoteIds, y => y.MapFrom(z=> z.NoteIds));
        
        CreateMap<JobStatusChanged, SE.JobStatusChanged>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.JobId))
            .ForMember(x => x.Status, y => y.MapFrom(z => z.Status));        
    }
}