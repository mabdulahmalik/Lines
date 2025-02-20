CREATE TABLE [org].[Facility] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [TypeId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]    DATETIME2 (7)    NOT NULL,
    [ModifiedAt]   DATETIME2 (7)    NULL,
    [Order]        INT              DEFAULT ((2147483647)) NOT NULL,
    [Archived]     BIT              DEFAULT ((0)) NOT NULL,
    [Name]         NVARCHAR (80)    NOT NULL,
    [TimeZone]     NVARCHAR (50)    NOT NULL,
    [RequiredData] TINYINT          NOT NULL,
    [Street]       NVARCHAR (80)    NOT NULL,
    [City]         NVARCHAR (30)    NOT NULL,
    [State]        NCHAR (2)        NOT NULL,
    [ZipCode]      NCHAR (5)        NOT NULL,
    CONSTRAINT [PK_Facility] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Facility_FacilityType] FOREIGN KEY ([TypeId]) REFERENCES [org].[FacilityType] ([Id])
);

GO