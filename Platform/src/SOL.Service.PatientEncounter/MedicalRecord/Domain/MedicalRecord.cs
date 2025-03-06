using Dawn;
using NodaTime;
using SOL.Abstractions.Activity;
using SOL.Abstractions.Domain;

namespace SOL.Service.PatientEncounter.MedicalRecord.Domain;

public class MedicalRecord : AggregateRoot, ITrackableAggregate
{
    private List<Observation> _observations = new();
    
    private MedicalRecord(Guid id, Guid facilityId, string number) 
        : base(id) 
    { 
        FacilityId = facilityId;
        Number = number;
    }

    public Guid FacilityId { get; private set; }
    public string Number { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public LocalDate? Birthday { get; private set; }
    public IReadOnlyList<Observation> Observations => _observations.AsReadOnly();

    internal static MedicalRecord Create(Guid facilityId, string number, string? firstName = null, string? lastName = null, LocalDate? birthday = null)
    {
        Guard.Argument(facilityId).NotDefault("Invalid FacilityId.");
        Guard.Argument(number).NotEmpty().NotWhiteSpace();

        var retVal = new MedicalRecord(Guid.NewGuid(), facilityId, number)
        {
            FirstName = firstName,
            LastName = lastName,
            Birthday = birthday
        };

        retVal.RaiseEventCreated();
        return retVal;
    }
    
    public void Renumber(string number)
    {
        Guard.Argument(number).NotEmpty().NotWhiteSpace();
        
        Number = number;

        RaiseEvent(new MedicalRecordRenumbered(Id, Number));
    }

    public void Modify(string? firstName, string? lastName, LocalDate? birthday) 
    {
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;

        RaiseEventModified();
    }
    
    public void AddObservations(params Observation[] observations)
    {
        Guard.Argument(observations).NotEmpty();
        
        _observations.AddRange(observations);
        
        RaiseEvent(new MedicalRecordObservationsAdded(Id, observations));
    }
    
    public void RemoveObservations(params Observation[] observations)
    {
        Guard.Argument(observations).NotEmpty();
        
        foreach (var observation in observations)
        {
            _observations.Remove(observation);
        }
        
        RaiseEvent(new MedicalRecordObservationsRemoved(Id, observations));
    }
}