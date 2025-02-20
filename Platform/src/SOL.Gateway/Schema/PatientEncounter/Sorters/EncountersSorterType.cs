using HotChocolate.Data.Sorting;
using SOL.Service.PatientEncounter.Encounter.Views;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncountersSorterType : SortInputType<EncounterView>
{
    protected override void Configure(ISortInputTypeDescriptor<EncounterView> descriptor)
    {
        descriptor
            .Name("EncountersSorter")
            .Description("Sorts the Encounters Query.");

        descriptor
            .Field(t => t.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Encounter was created.");

        descriptor
            .Field(t => t.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Encounter was modified.");
        
        descriptor
            .Field(t => t.Stage)
            .Name("status")
            .Description("The current Status of the Encounter.");

        descriptor
            .Field(t => t.Priority)
            .Name("priority")
            .Description("The Priority of the Encounter.");
    }
}