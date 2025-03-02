using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Messaging;
using SOL.Abstractions.Persistence;
using SOL.Gateway.Application;
using SOL.Gateway.Views.PatientEncounter.MedicalRecord;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Gateway.EventHandlers;

public class BroadcastMedicalRecordObservationsAdded : ServiceEventHandler<MedicalRecordObservationsAdded>
{
    private readonly ISubscriptionHub _subscriptionHub;
    private readonly IDbContextFactory<LinesDataStore> _dbCtxFactory;

    public BroadcastMedicalRecordObservationsAdded(ILogger<BroadcastMedicalRecordObservationsAdded> logger
        , IDbContextFactory<LinesDataStore> dbCtxFactory, ISubscriptionHub subscriptionHub) 
        : base(logger)
    {
        _subscriptionHub = subscriptionHub;
        _dbCtxFactory = dbCtxFactory;
    }

    protected override async Task HandleAsync(MedicalRecordObservationsAdded message, CancellationToken stoppageToken)
    {
        var obs = new List<MedicalRecordObservationView>();
        await using var dbCtx = await _dbCtxFactory.CreateDbContextAsync(stoppageToken);
        
        message.Observations.GroupBy(x => x.Type)
            .ToList().ForEach(async void (observations) =>
        {
            var observationType = observations.Key;
            var observationIds = observations.Select(x => x.ObjectId).ToArray();

            var observationViews = await dbCtx.Set<MedicalRecordObservationView>()
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