namespace SOL.Gateway.Schema.PatientEncounter;

public class AttachPhotosToEncounterCmdType : InputObjectType<AttachPhotosToEncounterCmd>
{
    protected override void Configure(IInputObjectTypeDescriptor<AttachPhotosToEncounterCmd> descriptor)
    {
        descriptor
            .Name("AttachPhotosToEncounterCmd")
            .Description("The Command that attaches multiple Photos to the Encounter.");
        
        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");
        
        descriptor
            .Field(t => t.Photos)
            .Name("photos")
            .Description("The list of binary representations of the Photos.");
    }
}

public class AttachPhotosToEncounterCmd
{
    public Guid EncounterId { get; init; }
    public List<IFile> Photos { get; init; } = new();
}