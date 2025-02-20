using MassTransit;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Workflow.LineRevision;

public class LineRevisionDefinition : SagaDefinition<LineRevisionState>
{
    private const int ConcurrencyLimit = 20;
    
    public LineRevisionDefinition()
    {
        Endpoint(e => {
            e.Name = "saga-line-revision";
            e.PrefetchCount = ConcurrencyLimit;
        });
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator
        , ISagaConfigurator<LineRevisionState> sagaConfigurator, IRegistrationContext context)
    {
        var partition = endpointConfigurator.CreatePartitioner(ConcurrencyLimit);
        sagaConfigurator.Message<EnactLineRevision>(x => x.UsePartitioner(partition, m => m.CorrelationId!.Value));
        sagaConfigurator.Message<ObjectModified>(x => x.UsePartitioner(partition, m => m.CorrelationId!.Value));
    }
}