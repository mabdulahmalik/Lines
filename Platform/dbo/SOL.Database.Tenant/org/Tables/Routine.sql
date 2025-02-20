CREATE TABLE [org].[Routine] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]   DATETIME2 (7)    NOT NULL,
    [ModifiedAt]  DATETIME2 (7)    NULL,
    [Active]      BIT              NOT NULL,
    [Name]        NVARCHAR (80)    NOT NULL,
    [Description] NVARCHAR (255)   NULL,
    [PurposeId]   UNIQUEIDENTIFIER NULL,
    [Settings]    TINYINT          NOT NULL,
    CONSTRAINT [PK_Routine] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Routine_Purpose] FOREIGN KEY ([PurposeId]) REFERENCES [enc].[Purpose] ([Id]) ON DELETE SET NULL
);

GO