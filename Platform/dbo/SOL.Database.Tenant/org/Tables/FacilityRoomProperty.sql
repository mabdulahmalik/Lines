CREATE TABLE [org].[FacilityRoomProperty] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [FacilityTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Order]          INT              DEFAULT ((2147483647)) NOT NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_FacilityRoomProperty] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilityRoomProperty_FacilityFacilityType] FOREIGN KEY ([FacilityTypeId]) REFERENCES [org].[FacilityType] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO