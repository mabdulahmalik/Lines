using AutoMapper;
using SOL.Service.PatientEncounter.Line.Domain;
using SE = SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Line;

public class LineEventRelay : Profile
{
    public LineEventRelay()
    {
        CreateMap<LineInfectionStatusChanged, SE.LineInfectionStatusChanged>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LineId))
            .ForMember(x => x.HasInfection, y => y.MapFrom(z => z.HasInfection))
            .ForMember(x => x.InfectedOn, y => y.MapFrom(z => z.InfectedOn));
        
        CreateMap<LinePlacementChanged, SE.LinePlacementChanged>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LineId))
            .ForMember(x => x.Dwelling, x => x.MapFrom(y => y.Dwelling))
            .ForMember(x => x.ExternallyPlaced, y => y.MapFrom(z => z.ExternallyPlaced))
            .ForMember(x => x.PlacedBy, y => y.MapFrom(z => z.PlacedBy))
            .ForMember(x => x.PlacedOn, y => y.MapFrom(z => z.PlacedOn));
        
        CreateMap<LineRemoved, SE.LineRemoved>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.RemovedOn, y => y.MapFrom(z => z.RemovedOn));
    }
}