using SOL.Abstractions.Storage;
using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterPhotoType : ObjectType<EncounterPhotoView>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterPhotoView> descriptor)
    {
        descriptor
            .Name("EncounterPhoto")
            .Description("The Photo associated with the Encounter.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the photo.");

        descriptor
            .Field("url")
            .Description("The Absolute Url to the Photo.")
            .Resolve(ctx => {
                var photo = ctx.Parent<EncounterPhotoView>();
                var fileStorage = ctx.Service<IFileStorage>();
                return fileStorage.GetAbsoluteUrl($"encounters/{photo.EncounterId}/{photo.ObjectName}");
            });

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Photo was created.");

        descriptor
            .Field(t => t.FileName)
            .Name("fileName")
            .Description("The uploaded file name of the File.");

        descriptor
            .Field(t => t.ContentType)
            .Name("contentType")
            .Description("The content type of the file.");

        descriptor
            .Field(t => t.Length)
            .Name("length")
            .Description("The size of the file.");
    }
}