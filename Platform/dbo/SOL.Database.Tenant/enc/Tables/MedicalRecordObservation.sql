CREATE TABLE [enc].[MedicalRecordObservation] (
    [MedicalRecordId] UNIQUEIDENTIFIER NOT NULL,
    [ObjectId]        UNIQUEIDENTIFIER NOT NULL,
    [Type]            TINYINT          NOT NULL,
    [Timestamp]       DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_MedicalRecordObservation] PRIMARY KEY CLUSTERED ([ObjectId] ASC, [Type] ASC, [MedicalRecordId] ASC),
    CONSTRAINT [FK_MedicalRecordObservation_MedicalRecord] FOREIGN KEY ([MedicalRecordId]) REFERENCES [enc].[MedicalRecord] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);
GO