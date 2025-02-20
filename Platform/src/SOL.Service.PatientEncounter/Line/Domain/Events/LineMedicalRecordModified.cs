using SOL.Abstractions.Activity;
using SOL.Abstractions.Messaging;

namespace SOL.Service.PatientEncounter.Line.Domain;

[TrackableActivity(nameof(LineMedicalRecordModified))]
public record LineMedicalRecordModified : IMessage
{
        internal LineMedicalRecordModified(Guid id, Guid medicalRecordId)
        {
            LineId = id;
            MedicalRecordId = medicalRecordId;
        }

        public Guid LineId { get; }
        public Guid MedicalRecordId { get; }
    }