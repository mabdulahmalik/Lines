CREATE TABLE [org].[RoutineRolling] (
    [RoutineId]     UNIQUEIDENTIFIER NOT NULL,
    [IntervalUnit]  TINYINT          NOT NULL,
    [IntervalValue] INT              NOT NULL,
    [DelayUnit]     TINYINT          NOT NULL,
    [DelayValue]    INT              NOT NULL,
    CONSTRAINT [PK_RoutineRolling] PRIMARY KEY CLUSTERED ([RoutineId] ASC),
    CONSTRAINT [FK_RoutineRolling_Routine] FOREIGN KEY ([RoutineId]) REFERENCES [org].[Routine] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO