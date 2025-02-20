using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.OrganizationMgmt;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class AssignedRoutinesActivity : IStateMachineActivity<PatientEncounterState>
{
    private readonly ICommandBus _commandBus;

    public AssignedRoutinesActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(AssignedRoutinesActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<PatientEncounterState> context, IBehavior<PatientEncounterState> next)
    {
        foreach (var routineAssignment in context.Saga.RoutinesAssigned.Where(x => x.Item2.HasValue))
        {
            await _commandBus.SendAsync(new CreateRoutineFollowUp {
                EncounterProcedureId = routineAssignment.Item1,
                RoutineAssignmentId = routineAssignment.Item2!.Value,
                FacilityRoomId = context.Saga.FacilityRoomId,
                MedicalRecordId = context.Saga.MedicalRecordId,
                LineId = context.Saga.LinesInserted.Where(x => x.Key == routineAssignment.Item1)
                    .Select(x => x.Value.Id).FirstOrDefault(),
                InitialRun = routineAssignment.Item3
            }, context.CancellationToken);            
        }

        await next.Execute(context);
    }

    public Task Execute<T>(BehaviorContext<PatientEncounterState, T> context, IBehavior<PatientEncounterState, T> next) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<PatientEncounterState, TException> context
        , IBehavior<PatientEncounterState> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<PatientEncounterState, T, TException> context
        , IBehavior<PatientEncounterState, T> next) where T : class where TException : Exception
    {
        throw new NotImplementedException();
    }
}