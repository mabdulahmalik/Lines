CREATE TABLE [org].[RoutineRecurrence] (
    [RoutineId] UNIQUEIDENTIFIER NOT NULL,
    [Time]      TIME (7)         NOT NULL,
    [Days]      NVARCHAR (255)   NOT NULL,
    CONSTRAINT [FK_RoutineRecurrence_Routine] FOREIGN KEY ([RoutineId]) REFERENCES [org].[Routine] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO