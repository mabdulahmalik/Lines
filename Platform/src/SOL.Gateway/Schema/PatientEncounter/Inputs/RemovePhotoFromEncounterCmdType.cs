using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RemovePhotoFromEncounterCmdType : InputObjectType<RemovePhotoFromEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<RemovePhotoFromEncounter> descriptor)
    {
        descriptor
            .Name("RemovePhotoFromEncounterCmd")
            .Description("The Command that removes a Photo from it's Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Photo.");
    }
}


