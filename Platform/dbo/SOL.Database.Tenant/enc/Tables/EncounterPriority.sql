CREATE TABLE [enc].[EncounterPriority] (
    [EncounterId]  UNIQUEIDENTIFIER NOT NULL,
    [Priority]     TINYINT          NOT NULL,
    [Timestamp]    DATETIME2         NOT NULL,
    CONSTRAINT [FK_EncounterPriority_Encounter] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id])
);


GO