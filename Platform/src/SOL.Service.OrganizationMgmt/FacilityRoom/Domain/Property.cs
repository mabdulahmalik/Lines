using Dawn;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.FacilityRoom.Domain;

public class Property : ValueType<Property>
{
    public Guid Id { get; set; }
    public Guid OptionId { get; set; }

    internal Property(Guid id, Guid optionId)
    {
        Guard.Argument(id).NotDefault();
        Guard.Argument(optionId).NotDefault();

        Id = id;
        OptionId = optionId;
    }

    protected override bool EqualsInternal(Property other)
    {
        return Id.Equals(other.Id)
            && OptionId.Equals(other.OptionId);
    }

    protected override int GetHashCodeInternal()
    {
        return HashCode.Combine(Id, OptionId);
    }
}