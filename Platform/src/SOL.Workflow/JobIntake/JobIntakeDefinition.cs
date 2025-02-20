using MassTransit;
using SOL.Messaging.Contracts;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Workflow.JobIntake;

public class JobIntakeDefinition : SagaDefinition<JobIntakeState>
{
    private const int ConcurrencyLimit = 20;
    
    public JobIntakeDefinition()
    {
        Endpoint(e => {
            e.Name = "saga-job-intake";
            e.PrefetchCount = ConcurrencyLimit;
        });
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator
        , ISagaConfigurator<JobIntakeState> sagaConfigurator, IRegistrationContext context)
    {
        var partition = endpointConfigurator.CreatePartitioner(ConcurrencyLimit);
        sagaConfigurator.Message<EnactJobIntake>(x => x.UsePartitioner(partition, m => m.CorrelationId!.Value));
        sagaConfigurator.Message<ObjectCreated>(x => x.UsePartitioner(partition, m => m.CorrelationId!.Value));
    }
}