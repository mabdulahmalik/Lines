using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Domain;

public class FacilityRoom : AggregateRoot
{
    private readonly List<Property> _properties = new();

    private FacilityRoom(Guid id, string name, Guid facilityId) 
        : base(id)
    {
        Name = name;
        FacilityId = facilityId;
    }

    public string Name { get; private set; }
    public Guid FacilityId { get; private set; }
    public IReadOnlyList<Property> Properties => _properties;

    internal static FacilityRoom Create(string name, Guid facilityId, params Property[] properties)
    {
        Guard.Argument(name).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(facilityId).NotDefault();
        
        var retVal = new FacilityRoom(Guid.NewGuid(), name, facilityId);
        
        if(properties.Any()) {
            retVal._properties.AddRange(properties);
        }

        retVal.RaiseEventCreated();
        return retVal;
    }

    public void Rename(string name)
    {
        Guard.Argument(name).NotNull().NotEmpty().NotWhiteSpace();

        Name = name;

        RaiseEventModified();
    }

    public void SetProperties(params Property[] properties)
    {
        Guard.Argument(properties).NotNull().NotEmpty();

        _properties.Clear();
        _properties.AddRange(properties);

        RaiseEventModified();
    }

    public void ClearProperties()
    {
        _properties.Clear();
        RaiseEventModified();
    }
}