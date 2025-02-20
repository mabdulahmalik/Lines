using AutoMapper;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;

namespace SOL.Messaging.Infrastructure;

public class EventRelayProfile : Profile
{
    public EventRelayProfile()
    {
        CreateMap<AggregateCreated, ObjectCreated>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name));
        
        CreateMap<AggregateModified, ObjectModified>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name));
        
        CreateMap<AggregateSorted, ObjectSorted>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Position, x => x.MapFrom(y => y.PosNew));
        
        CreateMap<AggregateDeleted, ObjectDeleted>()
            .ForMember(x => x.Ids, x => x.MapFrom(y => y.Ids))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name));
        
        CreateMap<AggregateRestored, ObjectRestored>()
            .ForMember(x => x.Ids, x => x.MapFrom(y => y.Ids))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name));        
        
        CreateMap<AggregateArchiveStateChanged, ObjectArchiveStateChanged>()
            .ForMember(x => x.Ids, x => x.MapFrom(y => y.Ids))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Archived, x => x.MapFrom(y => y.Archived));
    }
}