using HotChocolate.Resolvers;
using Newtonsoft.Json.Linq;
using SOL.Abstractions.Storage;
using SOL.Activities.Views;
using Path = System.IO.Path;

namespace SOL.Gateway.Schema.Activities.Objects;

public class ActivityType : ObjectType<ActivityView>
{
    protected override void Configure(IObjectTypeDescriptor<ActivityView> descriptor)
    {
        descriptor
            .Name("Activity")
            .Description("The Activity.");

        descriptor
            .Field(x => x.Id)
            .Name("activityId")
            .Description("The unique identifier of the Activity.");

        descriptor
            .Field(x => x.ActivityType)
            .Name("activityType")
            .Description("The Activity.");

        descriptor
            .Field(x => x.AggregateId)
            .Name("aggregateId")
            .Description("The unique identifier of the Aggregate.");

        descriptor
            .Field(x => x.AggregateType)
            .Name("aggregateType")
            .Description("The Aggregate.");

        descriptor
            .Field(x => x.Metadata)
            .Name("metadata")
            .Description("The Metadata of the Activity.")
            .Resolve(HandlePhotos);

        descriptor
            .Field(x => x.UserId)
            .Name("userId")
            .Description("The unique identifier of the User who performed the Activity.");

        descriptor
            .Field(t => t.Timestamp)
            .Name("timestamp")
            .Description("The date and time the Activity was performed.");
    }

    private static string? HandlePhotos(IResolverContext ctx)
    {
        var metadataJson = ctx.Parent<ActivityView>().Metadata;
        if (string.IsNullOrEmpty(metadataJson)) return null;

        var parsedMetadata = JObject.Parse(metadataJson);
        var photosArray = parsedMetadata["Photos"] as JArray;

        if (photosArray == null) return parsedMetadata.ToString();

        var fileStorage = ctx.Service<IFileStorage>();
        foreach (var photo in photosArray)
        {
            var fileName = $"{photo["Id"]}{Path.GetExtension(photo["FileName"]?.ToString())}";
            if (string.IsNullOrEmpty(fileName)) continue;

            photo["Url"] = fileStorage.GetAbsoluteUrl($"encounters/{parsedMetadata["EncounterId"]}/{fileName}");
        }

        return parsedMetadata.ToString();
    }
}