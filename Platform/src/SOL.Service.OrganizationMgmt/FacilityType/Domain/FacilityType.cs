using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public class FacilityType : AggregateRoot, ISortable
{
    private readonly List<RoomProperty> _properties = new();

    private FacilityType(Guid id, string name, bool active) 
        : base(id)
    {
        Name = name;
        Active = active;
    }

    public string Name { get; private set; }
    public bool Active { get; private set; }
    public int Order { get; private set; }
    public IReadOnlyList<RoomProperty> Properties => _properties;

    internal static FacilityType Create(string name)
    {
        Guard.Argument(name).NotNull().NotEmpty();

        var retVal = new FacilityType(Guid.NewGuid(), name, true);
        retVal.RaiseEventCreated();

        return retVal;
    }

    public void Rename(string name)
    {
        Guard.Argument(name).NotNull().NotEmpty();
        
        Name = name;
        RaiseEventModified();
    }    

    public void Activate()
    {
        Active = true;
        RaiseEvent(new FacilityTypeActivationChanged(Id, true));
    }

    public void Deactivate()
    {
        Active = false;
        RaiseEvent(new FacilityTypeActivationChanged(Id, false));
    }

    public void AddProperty(string name, int sortOrder, params RoomPropertyOption[] options)
    {
        Guard.Argument(name).NotNull().NotEmpty();

        var property = new RoomProperty(name, sortOrder);
        
        if(options.Any())
            property.SetOptions(options);
        
        _properties.Add(property);
        RaiseEvent(new RoomPropertyAdded(Id, property));
    }

    public void ModifyProperty(Guid propertyId, string name, int sortOrder, params RoomPropertyOption[] options)
    {
        Guard.Argument(propertyId).NotDefault();
        Guard.Argument(name).NotNull().NotEmpty();

        var property = _properties.Single(x => x.Id == propertyId);
        property.Rename(name);
        property.Resort(sortOrder);
        
        if(options.Any())
            property.SetOptions(options);
        else
            property.ClearOptions();

        RaiseEvent(new RoomPropertyModified(Id, property));
    }
    
    public void SortProperty(Guid id, int from, int to)
    {
        Guard.Argument(from).NotNegative();
        Guard.Argument(to).NotNegative();
        Guard.Argument(id).NotDefault();

        if(from == to)
            return;

        var property = _properties.Single(x => x.Order == from);
        _properties.Remove(property);
        _properties.Insert(to-1, property);

        var i = 0;
        _properties.ForEach(x => x.Resort(++i));
        
        RaiseEvent(new RoomPropertySorted(Id, id, from, to));
    }

    public void RemoveProperty(Guid propertyId)
    {
        Guard.Argument(propertyId).NotDefault();
        
        var property = _properties.Single(x => x.Id == propertyId);
        _properties.Remove(property);
        
        RaiseEvent(new RoomPropertyRemoved(Id, property));
    }
}