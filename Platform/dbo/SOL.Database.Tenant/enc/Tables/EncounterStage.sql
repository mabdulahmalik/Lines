CREATE TABLE [enc].[EncounterStage] (
    [EncounterId]  UNIQUEIDENTIFIER NOT NULL,
    [Stage]        TINYINT          NOT NULL,
    [Timestamp]    DATETIME2        NOT NULL,
    CONSTRAINT [FK_EncounterStage_Encounter] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id])
);

GO