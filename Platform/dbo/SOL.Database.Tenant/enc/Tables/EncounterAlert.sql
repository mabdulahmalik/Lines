CREATE TABLE [enc].[EncounterAlert] (
    [AlertedAt]   DATETIME2 (7)    NOT NULL,
    [AlertedBy]   UNIQUEIDENTIFIER NOT NULL,
    [EncounterId] UNIQUEIDENTIFIER NOT NULL,
    [Type]        TINYINT          NOT NULL,
    [Text]        TEXT             NULL,
    CONSTRAINT [PK_EncounterAlert] PRIMARY KEY CLUSTERED ([EncounterId] ASC, [Type] ASC),
    CONSTRAINT [FK_EncounterAlert_Encounter] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);