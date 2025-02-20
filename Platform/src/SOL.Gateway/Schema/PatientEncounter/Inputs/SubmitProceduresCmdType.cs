using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class SubmitProceduresCmdType : InputObjectType<SubmitProcedures>
{
    protected override void Configure(IInputObjectTypeDescriptor<SubmitProcedures> descriptor)
    {
        descriptor
            .Name("SubmitProceduresCmd")
            .Description("The Command that submits the applied Procedures.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter to submit the applied Procedures to.");
    }
}