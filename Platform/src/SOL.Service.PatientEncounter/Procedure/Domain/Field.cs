using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Procedure.Domain;

public class Field : Entity
{
    public string Name { get; private set; }
    public string? Instruction { get; private set; }
    public ProcedureFieldType Type { get; private set; }
    public ProcedureFieldSetting Settings { get; private set; }
    public bool Archived { get; private set; }
    public IReadOnlyList<FieldOption>? Options => _options;

    private List<FieldOption>? _options;
    
    internal Field(string name, ProcedureFieldType type, string? instruction = null) 
        : this(Guid.NewGuid(), name, type, instruction) 
    { }

    internal Field(Guid id, string name, ProcedureFieldType type, string? instruction = null) 
        : base(id)
    {
        Name = name;
        Type = type;
        Instruction = instruction;

        if(type == ProcedureFieldType.List) {
            _options = new();
        }
    }

    public void Rename(string name)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();
        Name = name;
    }

    public void ModifyInstruction(string instruction)
    {
        Instruction = instruction;
    }
    
    public void IsArchived(bool isArchived)
    {
        Archived = isArchived;
    }

    public void IsRequired(bool isRequired)
    {
        if(isRequired) { 
            Settings |= ProcedureFieldSetting.Required;
        } else {
            Settings &= ~ProcedureFieldSetting.Required;
        }
    }

    public void AllowMultiSelect(bool allowMultiSelect)
    {
        Guard.Operation(Type == ProcedureFieldType.List, "Only Field's of Type 'List' can Allow MultiSelect.");

        if(allowMultiSelect) { 
            Settings |= ProcedureFieldSetting.MultiSelect;
        } else {
            Settings &= ~ProcedureFieldSetting.MultiSelect;
        }
    }

    public void AddOptions(params FieldOption[] options)
    {
        Guard.Operation(Type == ProcedureFieldType.List, "Only Field's of Type 'List' have Options.");
        _options?.AddRange(options);
    }

    public void RemoveOptions(params Guid[] optionIds)
    {
        Guard.Operation(Type == ProcedureFieldType.List, "Only Field's of Type 'List' have Options.");
        _options?.RemoveAll(option => optionIds.Contains(option.Id));
    }
}