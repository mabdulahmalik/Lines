CREATE TABLE [enc].[EncounterPhoto] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [EncounterId]  UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]    DATETIME2        NOT NULL,
    [FileName]     NVARCHAR (80)    NOT NULL,
    [ContentType]  NVARCHAR (20)    NOT NULL,
    [Length]       BIGINT           NOT NULL,
    CONSTRAINT [PK_EncounterPhoto] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EncounterPhoto_Encounter] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id])
);


GO