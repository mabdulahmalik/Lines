using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.Purpose.Domain;

public class Purpose : AggregateRoot, ISortable, IArchivable
{
    private Purpose(Guid id, string name) 
        : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public bool Archived { get; private set; }
    public int Order { get; private set; }

    internal static Purpose Create(string name)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();

        var retVal = new Purpose(Guid.NewGuid(), name);
        retVal.RaiseEventCreated();

        return retVal;
    }

    public void Rename(string name)
    {
        Guard.Argument(name).NotEmpty().NotWhiteSpace();

        Name = name;
        RaiseEventModified();
    }
}