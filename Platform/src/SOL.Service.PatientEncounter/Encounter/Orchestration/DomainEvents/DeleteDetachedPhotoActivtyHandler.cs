using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SOL.Abstractions.Application;
using SOL.Activities;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration;

public class DeleteDetachedPhotoActivtyHandler : DomainEventHandler<EncounterPhotosDetached>
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly ActivitiesManager _activitiesManager;

    public DeleteDetachedPhotoActivtyHandler(ILogger<DeleteDetachedPhotoActivtyHandler> logger, IOperationContextFactory operationContextFactory, ActivitiesManager activitiesManager)
        : base(logger)
    {
        _operationContext = new(operationContextFactory.Get);
        _activitiesManager = activitiesManager;
    }
    protected override async Task HandleAsync(EncounterPhotosDetached message, CancellationToken stoppageToken)
    {
        var activities = await _activitiesManager.List(message.EncounterId, nameof(EncounterPhotosAttached), stoppageToken);

        foreach (var activity in activities)
        {
            var metadata = JObject.Parse(activity.Metadata);
            var photosArray = metadata["Photos"] as JArray;

            if (photosArray == null)
            {
                continue;
            }

            var photoIds = photosArray.Select(x => Guid.Parse(x["Id"]!.ToString())).ToList();

            if (!photoIds.Any(id => message.Photos.Any(x => x.Id == id)))
            {
                continue;
            }

            // Remove matching photos from the Photos array
            var updatedPhotos = photosArray.Where(x => !message.Photos.Any(photo => photo.Id.ToString() == x["Id"]!.ToString())).ToList();

            if (updatedPhotos.Count == 0)
            {
                // If no photos remain, remove the activity
                await _activitiesManager.Remove(activity.Id, stoppageToken);
            }
            else
            {
                // Update the metadata with the remaining photos
                metadata["Photos"] = JArray.FromObject(updatedPhotos);

                // Save the updated metadata back to the activity
                activity.Metadata = metadata.ToString();

                await _activitiesManager.Update(activity, stoppageToken);
            }
        }
    }
}