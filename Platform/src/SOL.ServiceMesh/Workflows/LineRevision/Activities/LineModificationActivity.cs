using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.ServiceMesh.Workflows.LineRevision.Activities;

public class LineModificationActivity : IStateMachineActivity<LineRevisionState>
{
    private readonly ICommandBus _commandBus;

    public LineModificationActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(LineModificationActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LineRevisionState> context, IBehavior<LineRevisionState> next)
    {
        ModifyMedicalRecord medicalRecord = null;
        
        if (context.Saga.MedicalRecordId.HasValue) {
            medicalRecord = new ModifyMedicalRecord {
                Id = context.Saga.MedicalRecordId
            };
        }

        if (!String.IsNullOrWhiteSpace(context.Saga.Number) 
            || !String.IsNullOrWhiteSpace(context.Saga.FirstName) 
            || !String.IsNullOrWhiteSpace(context.Saga.LastName) 
            || context.Saga.Birthday.HasValue) {
            medicalRecord = new ModifyMedicalRecord {
                Number = context.Saga.Number,
                FirstName = context.Saga.FirstName,
                LastName = context.Saga.LastName,
                Birthday = context.Saga.Birthday,
                Id = context.Saga.MedicalRecordId
            };
        }
        
        var modifyLineCmd = new ModifyLine {
            Id = context.Saga.LineId,
            Name = context.Saga.LineName,
            RoomId = context.Saga.RoomId,
            FacilityId = context.Saga.FacilityId,
            MedicalRecord = medicalRecord
        };
        
        await _commandBus.SendAsync(modifyLineCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public Task Execute<T>(BehaviorContext<LineRevisionState, T> context, IBehavior<LineRevisionState, T> next) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<LineRevisionState, TException> context, IBehavior<LineRevisionState> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<LineRevisionState, T, TException> context, IBehavior<LineRevisionState, T> next) where T : class where TException : Exception
    {
        throw new NotImplementedException();
    }
}