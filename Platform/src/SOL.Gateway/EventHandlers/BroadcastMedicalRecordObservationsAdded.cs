using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Application;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.MedicalRecord.View;

namespace SOL.Gateway.EventHandlers;

public class BroadcastMedicalRecordObservationsAdded : ServiceEventHandler<MedicalRecordObservationsAdded>
{
    private readonly ISubscriptionHub _subscriptionHub;
    private readonly IDomainQuery<MedicalRecordObservationView> _queryObservations;

    public BroadcastMedicalRecordObservationsAdded(ILogger<BroadcastMedicalRecordObservationsAdded> logger
        , IDomainQuery<MedicalRecordObservationView> queryObservations
        , ISubscriptionHub subscriptionHub) : base(logger)
    {
        _subscriptionHub = subscriptionHub;
        _queryObservations = queryObservations;
    }

    protected override async Task HandleAsync(MedicalRecordObservationsAdded message, CancellationToken stoppageToken)
    {
        var obs = new List<MedicalRecordObservationView>();
        
        message.Observations.GroupBy(x => x.Type)
            .ToList().ForEach(async observations =>
        {
            var observationType = observations.Key;
            var observationIds = observations.Select(x => x.ObjectId).ToArray();

            var observationViews = await _queryObservations.Query
                .Where(x => x.MedicalRecordId == message.Id 
                            && observationIds.Contains(x.ObjectId) 
                            && x.Type == observationType)
                .ToListAsync(stoppageToken);
            
            obs.AddRange(observationViews);
        });

        var broadcastMessage = new MedicalRecordObservationMessage {
            Id = message.Id,
            Observations = obs
                .OrderByDescending(x => x.Timestamp)
                .ToArray()
        };

        await _subscriptionHub.SendBroadcast(broadcastMessage, stoppageToken);
    }
    
    public record MedicalRecordObservationMessage : IMessage
    {
        public Guid Id { get; init; }
        public MedicalRecordObservationView[] Observations { get; init; }
    }
}