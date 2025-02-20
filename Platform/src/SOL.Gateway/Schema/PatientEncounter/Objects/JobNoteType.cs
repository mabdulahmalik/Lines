using SOL.Service.PatientEncounter.Job.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobNoteType : ObjectType<JobNoteView>
{
    protected override void Configure(IObjectTypeDescriptor<JobNoteView> descriptor)
    {
        descriptor
            .Name("JobNote")
            .Description("A Note associated with a Job.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier for the Note.");

        descriptor
            .Field(t => t.AuthorId)
            .Name("createdBy")
            .Description("The Author of the Note.");

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Note was created.");

        descriptor
            .Field(t => t.Treatment)
            .Type<JobNoteTreatmentType>()
            .Name("treatment")
            .Description("Any special treatments for this Note.");

        descriptor
            .Field(t => t.Text)
            .Name("text")
            .Description("The content of the Note.");
    }
}