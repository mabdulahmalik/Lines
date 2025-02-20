using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Facility.Domain;

public class Facility : AggregateRoot, ISortable, IArchivable
{
    private Tracked<Guid> _purposeIds = new();
    private Tracked<Guid> _procedureIds = new();
    private List<RoutineAssignment> _assignments = new();

    private Facility(Guid id, string name, Guid typeId, string timeZone)
        : base(id)
    {
        Name = name;
        TimeZone = timeZone;
        TypeId = typeId;
    }

    public string Name { get; private set; }
    public Guid TypeId { get; private set; }
    public bool Archived { get; private set; }
    public int Order { get; private set; }
    public string TimeZone { get; private set; }
    public Address Address { get; private set; }
    public RequiredPatientData RequiredData { get; private set; }
    public IReadOnlyList<RoutineAssignment> Routines => _assignments;

    internal static Facility Create(string name, Guid facilityTypeId, string timeZone, Address address, RequiredPatientData requiredData)
    {
        Guard.Argument(name).NotNull().NotEmpty();
        Guard.Argument(facilityTypeId).NotDefault();
        Guard.Argument(timeZone).NotNull().NotEmpty();
        Guard.Argument(address).NotNull();

        var retVal = new Facility(Guid.NewGuid(), name, facilityTypeId, timeZone) {
            RequiredData = requiredData,
            Address = address,
            Archived = false
        };

        retVal.RaiseEventCreated();
        return retVal;
    }

    public void Modify(string name, string timeZone, Address address, RequiredPatientData requiredData)
    {
        Guard.Argument(name).NotNull().NotEmpty();
        Guard.Argument(timeZone).NotNull().NotEmpty();
        Guard.Argument(address).NotNull();

        Name = name;
        TimeZone = timeZone;
        Address = address;
        RequiredData = requiredData;

        RaiseEventModified();
    }

    public void ExcludePurpose(params Guid[] purposeIds)
    {
        Guard.Argument(purposeIds).NotNull();

        _purposeIds.Clear();
        _purposeIds.AddRange(purposeIds);
        
        RaiseEventModified();
    }

    public void ExcludeProcedure(params Guid[] procedureIds)
    {
        Guard.Argument(procedureIds).NotNull();

        _procedureIds.Clear();
        _procedureIds.AddRange(procedureIds);
        
        RaiseEventModified();
    }

    public void AssignRoutine(params RoutineAssignment[] assignments)
    {
        Guard.Argument(assignments).NotNull().NotEmpty();

        _assignments.AddRange(assignments);
        RaiseEvent(new FacilityRoutinesAssigned(Id, assignments));
    }

    public void RemoveRoutine(params Guid[] assignmentIds)
    {
        Guard.Argument(assignmentIds).NotNull().NotEmpty();

        var removedRoutines = _assignments.Where(x => assignmentIds.Contains(x.Id));
        _assignments.RemoveAll(x => assignmentIds.Contains(x.Id));

        RaiseEvent(new FacilityRoutinesRemoved(Id, removedRoutines));
    }

    public void ModifyRoutine(params RoutineAssignment[] assignments)
    {
        Guard.Argument(assignments).NotNull().NotEmpty();

        Array.ForEach(assignments, a => {
            _assignments.RemoveAll(x => x.Id == a.Id);
            _assignments.Add(a);
        });

        RaiseEvent(new FacilityRoutinesModified(Id, assignments));
    }
}