CREATE TABLE [org].[FacilityRoomPropertyValue] (
    [RoomId]     UNIQUEIDENTIFIER NOT NULL,
    [PropertyId] UNIQUEIDENTIFIER NOT NULL,
    [OptionId]   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_FacilityRoomPropertyValue] PRIMARY KEY CLUSTERED ([RoomId] ASC, [PropertyId] ASC),
    CONSTRAINT [FK_FacilityRoomPropertyValue_FacilityRoom] FOREIGN KEY ([RoomId]) REFERENCES [org].[FacilityRoom] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_FacilityRoomPropertyValue_FacilityRoomProperty] FOREIGN KEY ([PropertyId]) REFERENCES [org].[FacilityRoomProperty] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_FacilityRoomPropertyValue_FacilityRoomPropertyOption] FOREIGN KEY ([OptionId]) REFERENCES [org].[FacilityRoomPropertyOption] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

GO