CREATE TABLE [enc].[Encounter] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [JobId]           UNIQUEIDENTIFIER NOT NULL,
    [PurposeId]       UNIQUEIDENTIFIER NOT NULL,
    [RoomId]          UNIQUEIDENTIFIER NOT NULL,
    [MedicalRecordId] UNIQUEIDENTIFIER NULL,
    [CreatedAt]       DATETIME2 (7)    NOT NULL,
    [ModifiedAt]      DATETIME2 (7)    NULL,
    CONSTRAINT [PK_Encounter] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Encounter_Job] FOREIGN KEY ([JobId]) REFERENCES [enc].[Job] ([Id]),
    CONSTRAINT [FK_Encounter_Room] FOREIGN KEY ([RoomId]) REFERENCES [org].[FacilityRoom] ([Id])
);

GO