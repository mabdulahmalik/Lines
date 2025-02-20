using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.UserMgmt.Enums;

public class UserAvailabilityType : EnumType<UserAvailability>
{
    protected override void Configure(IEnumTypeDescriptor<UserAvailability> descriptor)
    {
        descriptor
            .Name("UserAvailability")
            .Description("Represents a user's current availability.");
        
        descriptor
            .Value(UserAvailability.Offline)
            .Name("OFFLINE")
            .Description("The user is currently offline and unavailable.");

        descriptor
            .Value(UserAvailability.Free)
            .Name("FREE")
            .Description("The user is currently online and available.");
        
        descriptor
            .Value(UserAvailability.Busy)
            .Name("BUSY")
            .Description("The user is currently online but unavailable.");

        descriptor
            .Value(UserAvailability.Away)
            .Name("AWAY")
            .Description("The user is temporarily away but may return soon.");
    }
}
