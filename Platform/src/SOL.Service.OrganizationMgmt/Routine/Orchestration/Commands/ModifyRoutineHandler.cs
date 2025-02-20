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

public class ModifyRoutineHandler : CommandHandler<ModifyRoutine>
{
    private readonly IAggregateRepository<Domain.Routine> _repository;

    public ModifyRoutineHandler(ILogger<ModifyRoutineHandler> logger, IAggregateRepository<Domain.Routine> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyRoutine command, CancellationToken stoppageToken)
    {
        var routine = await _repository.Get(command.Id, stoppageToken);

        routine.Modify(command.Name, command.Description, command.PurposeId, command.IsFollowUp);

        routine.SetTermini(command.Termini.Select(x => new Trigger(x.ProcedureId, x.PropertyId, x.PropertyValue)).ToArray());

        routine.SetOrigins(command.Origins.Select(x => new Trigger(x.ProcedureId, x.PropertyId, x.PropertyValue)).ToArray());

        if (command.Rolling is not null)
        {
            routine.ClearRecurringSchedule();
            routine.SetRollingSchedule(new Routine_Domain_RollingSchedule(new RollingDuration(command.Rolling.Interval.Value
                , command.Rolling.Interval.Unit), new RollingDuration(command.Rolling.Delay.Value
                , command.Rolling.Delay.Unit)));
        }
        else if (command.Recurrence?.Any() == true)
        {
            routine.ClearRollingSchedule();
            routine.SetRecurringSchedule(new RecurringSchedule(command.Recurrence
                .Select(x => new Recurrence(x.Time, x.Days.ToArray())).ToArray()));
        }

        _repository.Update(routine);
        await _repository.Commit(stoppageToken);
    }
}