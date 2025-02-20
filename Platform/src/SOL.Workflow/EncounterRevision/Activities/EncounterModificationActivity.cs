using MassTransit;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Workflow.EncounterRevision.Activities;

public class EncounterModificationActivity : IStateMachineActivity<EncounterRevisionState>
{
    private readonly ICommandBus _commandBus;

    public EncounterModificationActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(EncounterModificationActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncounterRevisionState> context, IBehavior<EncounterRevisionState> next)
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
        
        var modifyEncounterCmd = new ModifyEncounter {
            JobId = context.Saga.JobId,
            Id = context.Saga.EncounterId,
            RoomId = context.Saga.RoomId,
            FacilityId = context.Saga.FacilityId,
            OrderingProvider = context.Saga.OrderingProvider,
            Contact = context.Saga.Contact,
            MedicalRecord = medicalRecord
        };
        
        await _commandBus.SendAsync(modifyEncounterCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public Task Execute<T>(BehaviorContext<EncounterRevisionState, T> context, IBehavior<EncounterRevisionState, T> next) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<EncounterRevisionState, TException> context, IBehavior<EncounterRevisionState> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<EncounterRevisionState, T, TException> context, IBehavior<EncounterRevisionState, T> next) where T : class where TException : Exception
    {
        throw new NotImplementedException();
    }
}