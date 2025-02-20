using MassTransit;
using NodaTime;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Service.OrganizationMgmt.Facility.Domain.Specifications;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Workflows.RoutineFollowUp;

public class EstablishNextRunTimeActivity : IStateMachineActivity<RoutineFollowUpState, CreateRoutineFollowUp>
{
    private readonly IAggregateReader<Facility.Domain.Facility> _facilityReader;
    private readonly IAggregateReader<Domain.Routine> _routineReader;

    public EstablishNextRunTimeActivity(IAggregateReader<Facility.Domain.Facility> facilityReader, IAggregateReader<Domain.Routine> routineReader)
    {
        _facilityReader = facilityReader;
        _routineReader = routineReader;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(EstablishNextRunTimeActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<RoutineFollowUpState, CreateRoutineFollowUp> context, IBehavior<RoutineFollowUpState, CreateRoutineFollowUp> next)
    {
        var facility = await _facilityReader.Get(new SingleFacilityByRoutineAssignmentSpecification(context.Message.RoutineAssignmentId), context.CancellationToken);
        var routineAssignment = facility.Routines.Single(x => x.Id == context.Message.RoutineAssignmentId);
        var routine = await _routineReader.Get(routineAssignment.RoutineId, context.CancellationToken);
        
        var systemZone = DateTimeZoneProviders.Tzdb[facility.TimeZone];
        var followUpDate = context.Message.InitialRun
            ? routine.GetFirstRun(systemZone)
            : routine.GetNextRun(systemZone);

        context.Saga.FacilityId = facility.Id;
        context.Saga.RoomId = context.Message.FacilityRoomId;
        context.Saga.PurposeId = routine.PurposeId;
        context.Saga.IsRoutineActive = routine.Active;
        context.Saga.ScheduledDate = followUpDate.Date.ToDateOnly();
        context.Saga.ScheduledTime = followUpDate.TimeOfDay != LocalTime.Midnight 
            ? followUpDate.TimeOfDay.ToTimeOnly() 
            : null;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<RoutineFollowUpState, CreateRoutineFollowUp, TException> context
        , IBehavior<RoutineFollowUpState, CreateRoutineFollowUp> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}