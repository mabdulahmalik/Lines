using SOL.Gateway.Views.PatientEncounter.Job;

namespace SOL.Gateway.Schema.PatientEncounter;

public class JobRoutineType : ObjectType<JobRoutineView>
{
    protected override void Configure(IObjectTypeDescriptor<JobRoutineView> descriptor)
    {
        descriptor
            .Name("JobRoutine")
            .Description("A view of a Job's Routine Assignment.");
        
        descriptor
            .Field(x => x.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the referenced Encounter.");
        
        descriptor
            .Field(x => x.EncounterProcedureId)
            .Name("encounterProcedureId")
            .Description("The unique identifier of the applied Procedure.");
        
        descriptor
            .Field(x => x.RoutineAssignmentId)
            .Name("routineAssignmentId")
            .Description("The unique identifier of the Routine Assignment.");
        
        descriptor
            .Field(x => x.RoutineId)
            .Name("routineId")
            .Description("The unique identifier of the Routine.");
    }
}