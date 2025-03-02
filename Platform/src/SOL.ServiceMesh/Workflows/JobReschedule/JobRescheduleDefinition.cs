using MassTransit;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.ServiceMesh.Workflows.JobReschedule;

public class JobRescheduleDefinition : SagaDefinition<JobRescheduleState>
{
    private const int ConcurrencyLimit = 20;
    
    public JobRescheduleDefinition()
    {
        Endpoint(e => {
            e.Name = "saga-job-reschedule";
            e.PrefetchCount = ConcurrencyLimit;
        });
    }
    
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator
        , ISagaConfigurator<JobRescheduleState> sagaConfigurator, IRegistrationContext context)
    {
        var partition = endpointConfigurator.CreatePartitioner(ConcurrencyLimit);
        sagaConfigurator.Message<EnactJobReschedule>(x => x.UsePartitioner(partition, m => m.Message.Id));
        sagaConfigurator.Message<ScheduledJobRunTimeEstablished>(x => x.UsePartitioner(partition, m => m.Message.JobId));
        sagaConfigurator.Message<JobRescheduled>(x => x.UsePartitioner(partition, m => m.Message.Id));
    }
}