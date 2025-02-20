namespace SOL.Service.PatientEncounter;

public class LocationView
{
    public Guid FacilityId { get; set; }
    public Guid RoomId { get; set; }
    public string? Room { get; set; }
}