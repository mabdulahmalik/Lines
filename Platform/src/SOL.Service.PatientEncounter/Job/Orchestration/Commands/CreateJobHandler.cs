using Microsoft.Extensions.Logging;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Job.Domain;
using Jb = SOL.Service.PatientEncounter.Job.Domain.Job;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class CreateJobHandler : CommandHandler<CreateJob>
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IAggregateRepository<Jb> _jobRepository;

    public CreateJobHandler(ILogger<CreateJobHandler> logger
        , IOperationContextFactory operationContextFactory
        , IAggregateRepository<Jb> jobRepository)
        : base(logger)
    { 
        _jobRepository = jobRepository;
        _operationContext = new(operationContextFactory.Get);
    }

    protected override async Task HandleAsync(CreateJob command, CancellationToken stoppageToken)
    {
        var activeRoutines = new List<ActiveRoutine>();
        
        if (command is { RoutineProcedureId: not null, RoutineAssignmentId: not null })
            activeRoutines.Add(new ActiveRoutine(command.RoutineProcedureId.Value, command.RoutineAssignmentId.Value));
        
        var job = Jb.Create(command.PurposeId, command.RoomId, command.LineId
            , command.MedicalRecordId, command.Contact, command.OrderingProvider
            , command.ScheduledDate, command.ScheduledTime, activeRoutines);

        if(!String.IsNullOrWhiteSpace(command.PreNote)) {
            var pinnedNote = new Note(_operationContext.Value.ActorId, command.PreNote);
            job.PinNote(pinnedNote);
        }

        await _jobRepository.Add(job, stoppageToken);
        await _jobRepository.Commit(stoppageToken);
    }
}