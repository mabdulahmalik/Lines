CREATE TABLE [dbo].[Activity] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [ActivityType] NVARCHAR(250) NOT NULL,
    [AggregateType] NVARCHAR(250) NOT NULL,
    [AggregateId] UNIQUEIDENTIFIER NOT NULL,
    [Metadata] NVARCHAR(MAX),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Timestamp] DATETIME2(7) NOT NULL DEFAULT GETDATE()
);

GO
CREATE NONCLUSTERED INDEX [IDX_Timestamp]
    ON [dbo].[Activity]([Timestamp] DESC);


GO
CREATE NONCLUSTERED INDEX [IDX_Aggregate]
    ON [dbo].[Activity]([AggregateType] ASC, [AggregateId] ASC);