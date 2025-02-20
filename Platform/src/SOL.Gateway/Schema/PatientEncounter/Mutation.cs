using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Messaging;
using SOL.Gateway.Schema.Common;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class MutationJobExtensions : ObjectTypeExtension<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field("enactJobIntake")
            .Description("Creates a new Job, and an Encounter if the Job is not scheduled for the future.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<EnactJobIntakePrcType>())
            .ResolveWith<MutationResolver<EnactJobIntake>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("enactEncounterRevision")
            .Description("Modifies the ancillary details of an Encounter and Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<EnactEncounterRevisionPrcType>())
            .ResolveWith<MutationResolver<EnactEncounterRevision>>(x => x.Mutate(default!, default!, default!, default!));        

        descriptor
            .Field("enactJobReschedule")
            .Description("Reschedule an existing Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<EnactJobReschedulePrcType>())
            .ResolveWith<MutationResolver<EnactJobReschedule>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("cancelJob")
            .Description("Cancels an existing Job along with with any associated encounters.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CancelJobCmdType>())
            .ResolveWith<MutationResolver<CancelJob>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("deleteJob")
            .Description("Deletes an existing Job along with with any associated encounters.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeleteJobCmdType>())
            .ResolveWith<MutationResolver<DeleteJob>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("createPurpose")
            .Description("Creates a new Purpose to describe a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CreatePurposeCmdType>())
            .ResolveWith<MutationResolver<CreatePurpose>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("deletePurpose")
            .Description("Deletes an existing Purpose that describes a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeletePurposeCmdType>())
            .ResolveWith<MutationResolver<DeletePurpose>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("renamePurpose")
            .Description("Adjust the descriptor of the Purpose.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RenamePurposeCmdType>())
            .ResolveWith<MutationResolver<RenamePurpose>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("sortPurpose")
            .Description("Changes the sort order of the Purpose.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<SortPurposeCmdType>())
            .ResolveWith<MutationResolver<SortPurpose>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("archivePurpose")
            .Description("Archives the Purpose.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ArchivePurposeCmdType>())
            .ResolveWith<MutationResolver<ArchivePurpose>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("unarchivePurpose")
            .Description("Restores (from Archives) the Purpose.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<UnarchivePurposeCmdType>())
            .ResolveWith<MutationResolver<UnarchivePurpose>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("createProcedure")
            .Description("Creates a new Procedure to be executed within a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CreateProcedureCmdType>())
            .ResolveWith<MutationResolver<CreateProcedure>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("deleteProcedure")
            .Description("Deletes an existing Procedure that's being executed within a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeleteProcedureCmdType>())
            .ResolveWith<MutationResolver<DeleteProcedure>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("modifyProcedure")
            .Description("Modifies the Procedure to be executed within a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyProcedureCmdType>())
            .ResolveWith<MutationResolver<ModifyProcedure>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("sortProcedure")
            .Description("Sorts the Procedure to be executed within a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<SortProcedureCmdType>())
            .ResolveWith<MutationResolver<SortProcedure>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("archiveProcedure")
            .Description("Archives the Procedure to be executed within a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ArchiveProcedureCmdType>())
            .ResolveWith<MutationResolver<ArchiveProcedure>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("unarchiveProcedure")
            .Description("Restores (from Archives) the Procedure to be executed within a Job.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<UnarchiveProcedureCmdType>())
            .ResolveWith<MutationResolver<UnarchiveProcedure>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("placeEncounterOnHold")
            .Description("Places the Encounter on Hold.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<PlaceEncounterOnHoldCmdType>())
            .ResolveWith<MutationResolver<PlaceEncounterOnHold>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("removeHoldFromEncounter")
            .Description("Removes the Hold from the Encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RemoveHoldFromEncounterCmdType>())
            .ResolveWith<MutationResolver<RemoveHoldFromEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("escalateEncounter")
            .Description("Escalates the Encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<EscalateEncounterCmdType>())
            .ResolveWith<MutationResolver<EscalateEncounter>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("deescalateEncounter")
            .Description("Deescalates the Encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeescalateEncounterCmdType>())
            .ResolveWith<MutationResolver<DeescalateEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("requestAssistanceForEncounter")
            .Description("Requests assistance for a specific encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RequestAssistanceForEncounterCmdType>())
            .ResolveWith<MutationResolver<RequestAssistanceForEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("cancelAssistanceRequestForEncounter")
            .Description("Cancels assistance request for a specific encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CancelAssistanceRequestForEncounterCmdType>())
            .ResolveWith<MutationResolver<CancelAssistanceRequestForEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("attendToPatient")
            .Description("Advances the Encounter workflow to 'Attending'.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<AttendToPatientCmdType>())
            .ResolveWith<MutationResolver<AttendToPatient>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("submitProcedures")
            .Description("Submit the applied Procedures to the Encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<SubmitProceduresCmdType>())
            .ResolveWith<MutationResolver<SubmitProcedures>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("completeEncounter")
            .Description("Completes the Encounter.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CompleteEncounterCmdType>())
            .ResolveWith<MutationResolver<CompleteEncounter>>(x => x.Mutate(default!, default!, default!, default!));        

        descriptor
            .Field("attachPhotosToEncounter")
            .Description("Attaches Photos to the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<AttachPhotosToEncounterCmdType>())
            .Resolve(async ctx => {
                var commandBus = ctx.Service<ICommandBus>();
                var command = ctx.ArgumentValue<AttachPhotosToEncounterCmd>("command");
                var operationContext = ctx.Service<IOperationContextFactory>().Get();
                var msgDataRepo = ctx.Service<IMessageDataRepository>();

                var photosData = new List<AttachPhotosToEncounter.PhotoData>();

                foreach (var photo in command.Photos)
                {
                    var result = await msgDataRepo.PutStream(photo.OpenReadStream(), TimeSpan.FromHours(6), ctx.RequestAborted);

                    photosData.Add(new AttachPhotosToEncounter.PhotoData
                    {
                        FileName = photo.Name,
                        Photo = result
                    });
                }

                await commandBus.SendAsync(new AttachPhotosToEncounter
                {
                    EncounterId = command.EncounterId,
                    Photos = photosData
                }, ctx.RequestAborted);

                return new MutationResponse
                {
                    CorrelationId = operationContext.CorrelationId
                };
            });

        descriptor
            .Field("removePhotoFromEncounter")
            .Description("Removes a Photo from the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RemovePhotoFromEncounterCmdType>())
            .ResolveWith<MutationResolver<RemovePhotoFromEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("attachNoteToJob")
            .Description("Attaches a Note to the Job")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<AttachNoteToJobCmdType>())
            .ResolveWith<MutationResolver<AttachNoteToJob>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyNoteForJob")
            .Description("Modifies a Note for the Job")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyNoteForJobCmdType>())
            .ResolveWith<MutationResolver<ModifyNoteForJob>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("removeNoteFromJob")
            .Description("Removes a Note from the Job")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RemoveNoteFromJobCmdType>())
            .ResolveWith<MutationResolver<RemoveNoteFromJob>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("pinNoteToJob")
            .Description("PINS a Note to it's Job")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<PinNoteToJobCmdType>())
            .ResolveWith<MutationResolver<PinNoteToJob>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("unpinNoteFromJob")
            .Description("UNPINS a Note from it's Job")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<UnpinNoteFromJobCmdType>())
            .ResolveWith<MutationResolver<UnpinNoteFromJob>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("assignMeToEncounter")
            .Description("Assigns the logged-in User to the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<AssignMeToEncounterCmdType>())
            .ResolveWith<MutationResolver<AssignMeToEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("withdrawMeFromEncounter")
            .Description("Withdraws the logged-in User from the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<WithdrawMeFromEncounterCmdType>())
            .ResolveWith<MutationResolver<WithdrawMeFromEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("applyProcedureToEncounter")
            .Description("Applies a Procedure to the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ApplyProcedureToEncounterCmdType>())
            .ResolveWith<MutationResolver<ApplyProcedureToEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("modifyProcedureForEncounter")
            .Description("Modifies a Procedure for the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyProcedureForEncounterCmdType>())
            .ResolveWith<MutationResolver<ModifyProcedureForEncounter>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("removeProcedureFromEncounter")
            .Description("Removes a Procedure from the Encounter")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RemoveProcedureFromEncounterCmdType>())
            .ResolveWith<MutationResolver<RemoveProcedureFromEncounter>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("enactListRevision")
            .Description("Modifies a Line.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<EnactListRevisionPrcType>())
            .ResolveWith<MutationResolver<EnactLineRevision>>(x => x.Mutate(default!, default!, default!, default!));                
        
        descriptor
            .Field("closeLine")
            .Description("Closes/Removes a Line.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<CloseLineCmdType>())
            .ResolveWith<MutationResolver<CloseLine>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("recordLineInfection")
            .Description("Records an Infection on a Line.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RecordLineInfectionCmdType>())
            .ResolveWith<MutationResolver<RecordLineInfection>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("clearLineInfection")
            .Description("Clears an Infection on a Line.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ClearLineInfectionCmdType>())
            .ResolveWith<MutationResolver<ClearLineInfection>>(x => x.Mutate(default!, default!, default!, default!));        
        
        descriptor
            .Field("placeLineExternally")
            .Description("Places a Line Externally.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<PlaceLineExternallyCmdType>())
            .ResolveWith<MutationResolver<PlaceLineExternally>>(x => x.Mutate(default!, default!, default!, default!));

        descriptor
            .Field("placeLineInternally")
            .Description("Places a Line Internally.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<PlaceLineInternallyCmdType>())
            .ResolveWith<MutationResolver<PlaceLineInternally>>(x => x.Mutate(default!, default!, default!, default!));        

        descriptor
            .Field("renumberMedicalRecord")
            .Description("Changes the Number of a Medical Record.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<RenumberMedicalRecordCmdType>())
            .ResolveWith<MutationResolver<RenumberMedicalRecord>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("modifyMedicalRecord")
            .Description("Modifies a Medical Record.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<ModifyMedicalRecordCmdType>())
            .ResolveWith<MutationResolver<ModifyMedicalRecord>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("deleteMedicalRecord")
            .Description("Deletes a Medical Record.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DeleteMedicalRecordCmdType>())
            .ResolveWith<MutationResolver<DeleteMedicalRecord>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("makeNoteAnObservation")
            .Description("Turns a Job Note into a Medical Record Observation.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<MakeNoteAnObservationCmdType>())
            .ResolveWith<MutationResolver<MakeNoteAnObservation>>(x => x.Mutate(default!, default!, default!, default!));
        
        descriptor
            .Field("discardNoteAsObservation")
            .Description("Discards a Job Note as a Medical Record Observation.")
            .Type<MutationResponseType>()
            .Argument("command", a => a.Type<DiscardNoteAsObservationCmdType>())
            .ResolveWith<MutationResolver<DiscardNoteAsObservation>>(x => x.Mutate(default!, default!, default!, default!));
    }
}