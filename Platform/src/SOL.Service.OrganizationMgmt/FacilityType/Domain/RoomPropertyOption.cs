using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityType.Domain;

public class RoomPropertyOption : Entity
{
    public RoomPropertyOption(Guid id, string text, int order) 
        : base(id)
    {
        Guard.Argument(text).NotNull().NotEmpty().NotWhiteSpace();

        Text = text.Trim();
        Order = order;
    }

    public RoomPropertyOption(string text, int order) 
        : this(Guid.NewGuid(), text, order)
    { }

    public string Text { get; private set; }
    public int Order { get; private set; }
}
