using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public class RoomProperty : Entity
{
    private readonly List<RoomPropertyOption> _options = new();

    public RoomProperty(Guid id, string name, int order) 
        : base(id)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();

        Name = name.Trim();
        Order = order;
    }

    public RoomProperty(string name, int order) 
        : this(Guid.NewGuid(), name, order)
    { }

    public string Name { get; private set; }
    public int Order { get; private set; }
    public IReadOnlyList<RoomPropertyOption> Options => _options;

    public void Rename(string name)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();
        Name = name;
    }
    
    public void Resort(int order)
    {
        Guard.Argument(order).NotNegative();
        Order = order;
    }

    public void SetOptions(params RoomPropertyOption[] options)
    {
        Guard.Argument(options).NotNull().NotEmpty();
        
        _options.Clear();
        _options.AddRange(options);
    }

    public void ClearOptions()
    {
        _options.Clear();
    }
}