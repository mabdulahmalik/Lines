using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Facility.Domain;

public class RoutineAssignment : Entity
{
    private readonly List<AssignmentSpec> _specs;

    internal RoutineAssignment(Guid id, Guid routineId, string name, IEnumerable<AssignmentSpec> specs) 
        : base(id)
    {
        Guard.Argument(name).NotNull().NotEmpty();
        Guard.Argument(routineId).NotDefault();

        Name = name.Trim();
        RoutineId = routineId;
        _specs = specs?.ToList() ?? new List<AssignmentSpec>();
    }

    internal RoutineAssignment(Guid routineId, string name, IEnumerable<AssignmentSpec> specs) 
        : this(Guid.NewGuid(), routineId, name, specs) 
    { }

    internal RoutineAssignment(Guid routineId, string name) 
        : this(routineId, name, new List<AssignmentSpec>())
    { }

    public string Name { get; private set; }
    public Guid RoutineId { get; }

    internal Guid this[Guid id]
    {
        get => _specs.Single(x => x.PropertyId == id).PropertyValue;
        set {
            var spec = _specs.FirstOrDefault(x => x.PropertyId == id);

            if (spec is not null)
            {
                spec.SetPropertyValue(value);
            } 
            else
            {
                _specs.Add(new AssignmentSpec(id, value));
            }
        }
    }

    internal void Rename(string name)
    {
        Name = name;
    }
}
