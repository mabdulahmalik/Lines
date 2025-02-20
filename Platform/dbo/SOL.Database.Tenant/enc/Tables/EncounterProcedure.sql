CREATE TABLE [enc].[EncounterProcedure] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [EncounterId] UNIQUEIDENTIFIER NOT NULL,
    [ProcedureId] UNIQUEIDENTIFIER NOT NULL,
    [PerformedBy] UNIQUEIDENTIFIER NOT NULL,
    [PerformedAt] DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_EncounterProcedure] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EncounterProcedure_Encounter] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_EncounterProcedure_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [enc].[Procedure] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO