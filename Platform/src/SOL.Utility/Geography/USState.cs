namespace SOL.Utility.Geography;

public class USState // TODO: Move to SOL.Geography
{
    public string Name { get; set; }
    public string Code { get; set; }

    // Method to return a list of all states and their codes
    public static List<USState> GetAll()
    {
        return new List<USState>
        {
            new USState { Name = "Alabama", Code = "AL" },
            new USState { Name = "Alaska", Code = "AK" },
            new USState { Name = "Arizona", Code = "AZ" },
            new USState { Name = "Arkansas", Code = "AR" },
            new USState { Name = "California", Code = "CA" },
            new USState { Name = "Colorado", Code = "CO" },
            new USState { Name = "Connecticut", Code = "CT" },
            new USState { Name = "Delaware", Code = "DE" },
            new USState { Name = "Florida", Code = "FL" },
            new USState { Name = "Georgia", Code = "GA" },
            new USState { Name = "Hawaii", Code = "HI" },
            new USState { Name = "Idaho", Code = "ID" },
            new USState { Name = "Illinois", Code = "IL" },
            new USState { Name = "Indiana", Code = "IN" },
            new USState { Name = "Iowa", Code = "IA" },
            new USState { Name = "Kansas", Code = "KS" },
            new USState { Name = "Kentucky", Code = "KY" },
            new USState { Name = "Louisiana", Code = "LA" },
            new USState { Name = "Maine", Code = "ME" },
            new USState { Name = "Maryland", Code = "MD" },
            new USState { Name = "Massachusetts", Code = "MA" },
            new USState { Name = "Michigan", Code = "MI" },
            new USState { Name = "Minnesota", Code = "MN" },
            new USState { Name = "Mississippi", Code = "MS" },
            new USState { Name = "Missouri", Code = "MO" },
            new USState { Name = "Montana", Code = "MT" },
            new USState { Name = "Nebraska", Code = "NE" },
            new USState { Name = "Nevada", Code = "NV" },
            new USState { Name = "New Hampshire", Code = "NH" },
            new USState { Name = "New Jersey", Code = "NJ" },
            new USState { Name = "New Mexico", Code = "NM" },
            new USState { Name = "New York", Code = "NY" },
            new USState { Name = "North Carolina", Code = "NC" },
            new USState { Name = "North Dakota", Code = "ND" },
            new USState { Name = "Ohio", Code = "OH" },
            new USState { Name = "Oklahoma", Code = "OK" },
            new USState { Name = "Oregon", Code = "OR" },
            new USState { Name = "Pennsylvania", Code = "PA" },
            new USState { Name = "Rhode Island", Code = "RI" },
            new USState { Name = "South Carolina", Code = "SC" },
            new USState { Name = "South Dakota", Code = "SD" },
            new USState { Name = "Tennessee", Code = "TN" },
            new USState { Name = "Texas", Code = "TX" },
            new USState { Name = "Utah", Code = "UT" },
            new USState { Name = "Vermont", Code = "VT" },
            new USState { Name = "Virginia", Code = "VA" },
            new USState { Name = "Washington", Code = "WA" },
            new USState { Name = "West Virginia", Code = "WV" },
            new USState { Name = "Wisconsin", Code = "WI" },
            new USState { Name = "Wyoming", Code = "WY" }
        };
    }
}