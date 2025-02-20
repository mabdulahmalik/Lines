using SOL.Gateway.EventHandlers;
using SOL.Messaging.Contracts.Common;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Contracts.UserMgmt;

namespace SOL.Gateway;

static class EventRelayRegistry
{
    public static Type[] EventHandlers => new[] {
        // -- General
        Broadcast<ObjectCreated>(),
        Broadcast<ObjectModified>(),
        Broadcast<ObjectSorted>(),
        Broadcast<ObjectDeleted>(),
        Broadcast<ObjectRestored>(),
        Broadcast<ObjectArchiveStateChanged>(),
        Broadcast<ActivitiesTracked>(),
        Direct<OperationErrored>(),
        // -- Jobs
        Broadcast<EncounterCliniciansAssigned>(),
        Broadcast<EncounterCliniciansWithdrawn>(),
        Broadcast<EncounterPhotosAttached>(),
        Broadcast<EncounterPhotosDetached>(),
        Broadcast<EncounterPriorityChanged>(),
        Broadcast<EncounterProceduresApplied>(),
        Broadcast<EncounterProceduresModified>(),
        Broadcast<EncounterProceduresUndone>(),
        Broadcast<EncounterProgressed>(),
        Broadcast<EncounterAlertAdded>(),
        Broadcast<EncounterAlertRemoved>(),
        Broadcast<JobNotesAdded>(),
        Broadcast<JobNotesModified>(),
        Broadcast<JobNotesRemoved>(),
        Broadcast<JobRescheduled>(),
        Broadcast<JobStatusChanged>(),
        Broadcast<LineInfectionStatusChanged>(),
        Broadcast<LinePlacementChanged>(),
        Broadcast<LineRemoved>(),
        Broadcast<MedicalRecordObservationsRemoved>(),
        // -- Facilities
        Broadcast<FacilityTypeActivationChanged>(),
        Broadcast<RoutineActivationChanged>(),        
        Broadcast<FacilityRoomPropertyAdded>(),
        Broadcast<FacilityRoomPropertyModified>(),
        Broadcast<FacilityRoomPropertySorted>(),
        // -- Users
        Broadcast<UserStatusChanged>(),
        Direct<UserPreferenceChanged>(),
        Broadcast<UserActivationChanged>()
    };

    private static Type Broadcast<TType>() where TType : class
    {
        var eventType = typeof(TType);
        var relayHandlerType = typeof(BroadcastRelayHandler<>);
        var relayHandler = relayHandlerType.MakeGenericType(eventType);

        return relayHandler;
    }
    
    private static Type Direct<TType>() where TType : class
    {
        var eventType = typeof(TType);
        var relayHandlerType = typeof(DirectRelayHandler<>);
        var relayHandler = relayHandlerType.MakeGenericType(eventType);

        return relayHandler;
    }    
}