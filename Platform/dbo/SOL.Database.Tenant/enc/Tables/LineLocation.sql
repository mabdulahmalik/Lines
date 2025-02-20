CREATE TABLE [enc].[LineLocation] (
    [LineId]       UNIQUEIDENTIFIER NOT NULL,
    [RoomId]       UNIQUEIDENTIFIER NOT NULL,
    [Timestamp]    DATETIME2        NOT NULL,
    CONSTRAINT [FK_LineLocation_Line] FOREIGN KEY ([LineId]) REFERENCES [enc].[Line] ([Id])
);

GO