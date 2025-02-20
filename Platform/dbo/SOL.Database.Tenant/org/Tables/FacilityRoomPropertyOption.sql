CREATE TABLE [org].[FacilityRoomPropertyOption] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [PropertyId] UNIQUEIDENTIFIER NOT NULL,
    [Order]      INT              DEFAULT ((2147483647)) NOT NULL,
    [Text]       NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_FacilityRoomPropertyOption] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilityRoomPropertyOption_FacilityRoomProperty] FOREIGN KEY ([PropertyId]) REFERENCES [org].[FacilityRoomProperty] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO