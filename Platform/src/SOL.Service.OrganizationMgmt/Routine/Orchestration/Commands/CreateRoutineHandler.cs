using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.OrganizationMgmt.Routine.Domain;
using Domain_RollingSchedule = SOL.Service.OrganizationMgmt.Routine.Domain.RollingSchedule;
using RollingSchedule = SOL.Service.OrganizationMgmt.Routine.Domain.RollingSchedule;
using Routine_Domain_RollingSchedule = SOL.Service.OrganizationMgmt.Routine.Domain.RollingSchedule;
using Routines_RollingSchedule = SOL.Service.OrganizationMgmt.Routine.Domain.RollingSchedule;

namespace SOL.Service.OrganizationMgmt.Routine.Orchestration.Commands;

public class CreateRoutineHandler : CommandHandler<CreateRoutine>
{
    private readonly IAggregateRepository<Domain.Routine> _repository;

    public CreateRoutineHandler(ILogger<CreateRoutineHandler> logger, IAggregateRepository<Domain.Routine> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(CreateRoutine command, CancellationToken stoppageToken)
    {
        var routine = Domain.Routine.Create(command.Name, command.Description, true, command.PurposeId, command.IsFollowUp
            , origins: command.Origins.Select(x => new Trigger(x.ProcedureId, x.PropertyId, x.PropertyValue)).ToArray()
            , termini: command.Termini.Select(x => new Trigger(x.ProcedureId, x.PropertyId, x.PropertyValue)).ToArray()
            , rolling: command.Rolling is not null ? new Routine_Domain_RollingSchedule(new RollingDuration(command.Rolling.Interval.Value
                , command.Rolling.Interval.Unit), new RollingDuration(command.Rolling.Delay.Value
                , command.Rolling.Delay.Unit)) : null
            , recurring: command.Recurrence is not null 
                ? new RecurringSchedule(command.Recurrence.Select(x => new Recurrence(x.Time, x.Days.ToArray())).ToArray())
                : null
        );

        await _repository.Add(routine, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}