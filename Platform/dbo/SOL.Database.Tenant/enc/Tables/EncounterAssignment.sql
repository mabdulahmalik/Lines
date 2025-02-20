CREATE TABLE [enc].[EncounterAssignment] (
    [EncounterId]  UNIQUEIDENTIFIER NOT NULL,
    [ClinicianId]  UNIQUEIDENTIFIER NOT NULL,
    [Position]     TINYINT          NOT NULL,
    [AssignedAt]   DATETIME2        NOT NULL,
    [WithdrawnAt]  DATETIME2        NULL,
    CONSTRAINT [PK_EncounterAssignment] PRIMARY KEY CLUSTERED ([EncounterId] ASC, [ClinicianId] ASC, [AssignedAt] ASC),
    CONSTRAINT [FK_EncounterAssignment_Encounter] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id])
);


GO