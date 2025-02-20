using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.UserMgmt.Enums;

public class UserPreferenceType : EnumType<UserPreference>
{
    protected override void Configure(IEnumTypeDescriptor<UserPreference> descriptor)
    {
        descriptor
            .Name("UserPreference")
            .Description("Represents a user's preference settings.");
        
        descriptor
            .Value(UserPreference.ElapsedTime)
            .Name("ELAPSED_TIME")
            .Description("The user prefers elapsed time format.");
        
        descriptor
            .Value(UserPreference.MilitaryTime)
            .Name("MILITARY_TIME")
            .Description("The user prefers military time format.");
        
        descriptor
            .Value(UserPreference.MiddleEndianDate)
            .Name("MIDDLE_ENDIAN_DATE")
            .Description("The user prefers middle-endian date format.");
    }
}