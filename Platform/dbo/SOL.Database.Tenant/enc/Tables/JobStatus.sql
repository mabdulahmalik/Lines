CREATE TABLE [enc].[JobStatus] (
    [JobId]     UNIQUEIDENTIFIER NOT NULL,
    [Status]    TINYINT          NOT NULL,
    [Timestamp] DATETIME2        NOT NULL
    CONSTRAINT [FK_JobStatus_Job] FOREIGN KEY ([JobId]) REFERENCES [enc].[Job] ([Id])
);

GO