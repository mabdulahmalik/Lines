using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Procedure.Domain;

public class FieldOption : Entity
{
    public string Text { get; private set; }
    public bool Archived { get; private set; }

    internal FieldOption(string text) 
        : this(Guid.NewGuid(), text) 
    { }

    internal FieldOption(Guid id, string text) 
        : base(id)
    {
        Text = text;
    }

    public void ChangeText(string text)
    {
        Text = text;
    }
    
    public void IsArchived(bool isArchived)
    {
        Archived = isArchived;
    }
}