namespace SOL.Gateway.Views.PatientEncounter;

public class LocationView
{
    public Guid FacilityId { get; set; }
    public Guid RoomId { get; set; }
    public string? Room { get; set; }
}