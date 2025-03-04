using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Procedure.Domain;

public class Procedure : AggregateRoot, IArchivable, ISortable
{
    private List<Field> _fields = new();

    private Procedure(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public ProcedureType Type { get; private set; }
    public RequiredPatientData RequiredData { get; private set; }
    public ProcedureSetting Settings { get; private set; }
    public IReadOnlyList<Field> Fields => _fields;
    public bool Archived { get; private set; }
    public int Order { get; private set; }

    internal static Procedure Create(string name, ProcedureType type, IEnumerable<Field> fields
        , RequiredPatientData requiredData, bool enablePerformanceReporting = true)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();
        Guard.Argument(fields).NotEmpty(a => "At least 1 field is required to create a new Procedure.");
        
        var retVal = new Procedure(Guid.NewGuid(), name) {
            Type = type,
            RequiredData = requiredData,
            Settings = enablePerformanceReporting
                ? ProcedureSetting.PerformanceReporting
                : 0
        };
        
        retVal._fields.AddRange(fields);
        retVal.RaiseEventCreated();

        return retVal;
    }

    public void Modify(string name, RequiredPatientData requiredData, bool enablePerformanceReporting)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();
        
        Name = name;
        RequiredData = requiredData;

        if(enablePerformanceReporting) {
            Settings |= ProcedureSetting.PerformanceReporting;
        } else {
            Settings &= ~ProcedureSetting.PerformanceReporting;
        }
        
        RaiseEventModified();
    }

    public void AddFields(params Field[] fields)
    {
        Guard.Argument(fields).NotEmpty();

        _fields.AddRange(fields);

        RaiseEvent(new ProcedureFieldAdded(Id, fields));
    }

    public void RemoveFields(params Guid[] fieldIds)
    {
        Guard.Argument(fieldIds).NotEmpty();

        var removedFields = _fields.Where(x => fieldIds.Contains(x.Id));
        _fields.RemoveAll(f => fieldIds.Contains(f.Id));
        
        RaiseEvent(new ProcedureFieldRemoved(Id, removedFields));
    }

    public void ModifyField(params Field[] fields)
    {
        Guard.Argument(fields).NotEmpty();

        Array.ForEach(fields, a => {
            _fields.RemoveAll(x => x.Id == a.Id);
            _fields.Add(a);
        });

        RaiseEvent(new ProcedureFieldModified(Id, fields));
    }
}