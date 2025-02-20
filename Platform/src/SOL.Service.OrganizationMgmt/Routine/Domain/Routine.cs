using Dawn;
using NodaTime;
using SOL.Abstractions.Domain;

namespace SOL.Service.OrganizationMgmt.Routine.Domain;

public class Routine : AggregateRoot
{
    private readonly List<Trigger> _origins = new();
    private readonly List<Trigger> _termini = new();
    private readonly RecurringSchedule _recurring = new();

    private Routine(Guid id, string name, string? description, bool active) 
        : base(id)
    {
        Name = name;
        Description = description;
        Active = active;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool Active { get; private set; }
    public Guid PurposeId { get; private set; }
    public RoutineSetting Settings { get; private set; }
    public RollingSchedule? Rolling { get; private set; }
    public RecurringSchedule Recurring => _recurring;
    public IReadOnlyList<Trigger> Origins => _origins;
    public IReadOnlyList<Trigger> Termini => _termini;

    internal static Routine Create(string name, string? description, bool active, Guid purposeId, bool isFollowUp
        , Trigger[] origins, Trigger[] termini, RollingSchedule? rolling = null, RecurringSchedule? recurring = null)
    {
        Guard.Argument(name).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(purposeId).NotDefault();
        Guard.Argument(origins).NotNull().NotEmpty();
        Guard.Argument(termini).NotNull().NotEmpty();
        Guard.Argument(rolling).Invariant(_ => rolling is null || recurring is null, _ => "Rolling and Recurring schedules cannot be set at the same time");

        var retVal = new Routine(Guid.NewGuid(), name, description, active) {
            PurposeId = purposeId,
            Settings = isFollowUp ? RoutineSetting.FollowUp : 0
        };

        retVal._origins.AddRange(origins);
        retVal._termini.AddRange(termini);

        if(rolling is not null)
            retVal.Rolling = rolling;

        if(recurring is not null && recurring.Any())
            retVal.SetRecurringSchedule(recurring);

        retVal.RaiseEventCreated();
        return retVal;
    }
    
    public ZonedDateTime GetFirstRun(DateTimeZone timeZone, Instant current = default)
    {
        Guard.Argument(_recurring).Invariant(arg => arg.Any() || Rolling is not null, _ => "No Schedule set.");
        
        if (current == default)
            current = SystemClock.Instance.GetCurrentInstant();

        return Recurring.Any() 
            ? Recurring.NextRun(timeZone, current) 
            : Rolling.StartDelay(current).InZone(timeZone);
    }    
    
    public ZonedDateTime GetNextRun(DateTimeZone timeZone, Instant current = default)
    {
        Guard.Argument(_recurring).Invariant(arg => arg.Any() || Rolling is not null, _ => "No Schedule set.");
        
        if (current == default)
            current = SystemClock.Instance.GetCurrentInstant();

        return Recurring.Any() 
            ? Recurring.NextRun(timeZone, current) 
            : Rolling.NextRun(current).InZone(timeZone);
    }

    public void Modify(string name, string? description, Guid purposeId, bool isFollowUp)
    {
        Guard.Argument(name).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(purposeId).NotDefault();

        Name = name;
        Description = description;
        PurposeId = purposeId;
        Settings = isFollowUp ? RoutineSetting.FollowUp : 0;
        
        RaiseEventModified();
    }
    
    public void Activate()
    {
        Active = true;
        RaiseEvent(new RoutineActivationChanged(Id, true));
    }

    public void Deactivate()
    {
        Active = false;
        RaiseEvent(new RoutineActivationChanged(Id, false));
    }    

    public void SetOrigins(params Trigger[] origins)
    {
        Guard.Argument(origins).NotNull().NotEmpty();

        _origins.Clear();
        _origins.AddRange(origins);

        RaiseEvent(new RoutineOriginsAdded(Id, origins));
    }

    public void ClearOrigins()
    {
        _origins.Clear();

        RaiseEvent(new RoutineOriginsCleared(Id));
    }

    public void SetTermini(params Trigger[] termini)
    {
        Guard.Argument(termini).NotNull().NotEmpty();

        _termini.Clear();
        _termini.AddRange(termini);

        RaiseEvent(new RoutineTerminiAdded(Id, termini));
    }

    public void ClearTermini()
    {
        _termini.Clear();

        RaiseEvent(new RoutineTerminiCleared(Id));
    }

    public void SetRollingSchedule(RollingSchedule schedule)
    {
        Guard.Argument(schedule).NotNull();

        Rolling = schedule;
        RaiseEventModified();
    }

    public void ClearRollingSchedule()
    {
        Rolling = null;
        RaiseEventModified();
    }

    public void SetRecurringSchedule(RecurringSchedule schedule)
    {
        Guard.Argument(schedule).NotNull().NotEmpty();

        _recurring.Clear();
        _recurring.AddRange(schedule);

        RaiseEventModified();
    }

    public void ClearRecurringSchedule()
    {
        _recurring.Clear();
        RaiseEventModified();
    }
}