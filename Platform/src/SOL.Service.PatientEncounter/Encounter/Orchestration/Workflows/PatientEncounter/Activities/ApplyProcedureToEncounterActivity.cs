using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class ApplyProcedureToEncounterActivity : IStateMachineActivity<PatientEncounterState, ApplyProcedureToEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly IAggregateReader<Procedure.Domain.Procedure> _procedureReader;
    private readonly Lazy<IOperationContext> _operationContext;

    public ApplyProcedureToEncounterActivity(IAggregateRepository<Domain.Encounter> repository
        , IAggregateReader<Procedure.Domain.Procedure> procedureReader
        , IOperationContextFactory operationContextFactory)
    {
        _repository = repository;
        _procedureReader = procedureReader;
        _operationContext = new(operationContextFactory.Get);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ApplyProcedureToEncounterActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<PatientEncounterState, ApplyProcedureToEncounter> context, IBehavior<PatientEncounterState, ApplyProcedureToEncounter> next)
    {
        var encounter = await _repository.Get(context.Message.EncounterId, context.CancellationToken);
        var values = context.Message.Values.Select(x => new ProcedureValue(x.FieldId, x.Value)).ToArray();
        var encounterProcedure = new Domain.Procedure(context.Message.ProcedureId, _operationContext.Value.ActorId, values);

        encounter.ApplyProcedures(encounterProcedure);

        await _repository.Update(encounter, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);

        var procedure = await _procedureReader.Get(
            new SingleInstanceSpecification<Procedure.Domain.Procedure>(encounterProcedure.ProcedureId),
            context.CancellationToken);
        
        if (procedure.Type == ProcedureType.Insertion && !String.IsNullOrWhiteSpace(context.Message.InsertedLineName)) {
            context.Saga.LinesInserted.Add(encounterProcedure.Id, new InsertedLine { 
                Type = procedure.Name,
                Name = context.Message.InsertedLineName, 
                InsertedOn = encounterProcedure.PerformedAt.ToDateTimeUtc() 
            });
        }
        
        if (procedure.Type == ProcedureType.Removal && context.Message.RemovedLineId.HasValue) {
            context.Saga.LinesRemoved.Add(encounterProcedure.Id, context.Message.RemovedLineId.Value);
        }

        context.Message.RoutinesAssigned.ForEach(ra =>
            context.Saga.RoutinesAssigned.Add(Tuple.Create(encounterProcedure.Id, (Guid?)ra, true)));
        
        context.Message.RoutinesRemoved.ForEach(rr =>
            context.Saga.RoutinesRemoved.Add(Tuple.Create(encounterProcedure.Id, rr)));        
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<PatientEncounterState, ApplyProcedureToEncounter, TException> context, IBehavior<PatientEncounterState, ApplyProcedureToEncounter> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}