CREATE TABLE [enc].[JobNote] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [JobId]     UNIQUEIDENTIFIER NOT NULL,
    [AuthorId]  UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2        NOT NULL,    
    [Treatment] TINYINT          NOT NULL,
    [Text]      TEXT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobNoteNote_Job] FOREIGN KEY ([JobId]) REFERENCES [enc].[Job] ([Id])
);


GO