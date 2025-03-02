using MassTransit;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.ServiceMesh.Workflows.EncounterRevision;

public class EncounterRevisionDefinition : SagaDefinition<EncounterRevisionState>
{
    private const int ConcurrencyLimit = 20;
    
    public EncounterRevisionDefinition()
    {
        Endpoint(e => {
            e.Name = "saga-encounter-revision";
            e.PrefetchCount = ConcurrencyLimit;
        });
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator
        , ISagaConfigurator<EncounterRevisionState> sagaConfigurator, IRegistrationContext context)
    {
        var partition = endpointConfigurator.CreatePartitioner(ConcurrencyLimit);
        sagaConfigurator.Message<EnactEncounterRevision>(x => x.UsePartitioner(partition, m => m.CorrelationId!.Value));
        sagaConfigurator.Message<ObjectCreated>(x => x.UsePartitioner(partition, m => m.CorrelationId!.Value));
    }    
}