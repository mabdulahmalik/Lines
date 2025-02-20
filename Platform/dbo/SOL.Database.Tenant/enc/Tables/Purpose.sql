CREATE TABLE [enc].[Purpose] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]  DATETIME2 (7)    NOT NULL,
    [ModifiedAt] DATETIME2 (7)    NULL,
    [Archived]   BIT              NOT NULL,
    [Order]      INT              DEFAULT ((2147483647)) NOT NULL,
    [Name]       NVARCHAR (80)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO