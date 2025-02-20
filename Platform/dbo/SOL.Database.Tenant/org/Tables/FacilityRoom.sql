CREATE TABLE [org].[FacilityRoom] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [FacilityId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]  DATETIME2        NOT NULL,
    [ModifiedAt] DATETIME2        NULL,
    [Name]       NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_FacilityRoom] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilityRoom_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [org].[Facility] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO