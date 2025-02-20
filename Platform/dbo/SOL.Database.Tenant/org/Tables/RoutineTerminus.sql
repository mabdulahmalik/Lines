CREATE TABLE [org].[RoutineTerminus] (
    [RoutineId]     UNIQUEIDENTIFIER NOT NULL,
    [ProcedureId]   UNIQUEIDENTIFIER NOT NULL,
    [PropertyId]    UNIQUEIDENTIFIER NULL,
    [PropertyValue] NVARCHAR (800)   NULL,
    CONSTRAINT [FK_RoutineTerminus_Routine] FOREIGN KEY ([RoutineId]) REFERENCES [org].[Routine] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_RoutineTerminus_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [enc].[Procedure] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_RoutineTerminus_ProcedureField] FOREIGN KEY ([PropertyId]) REFERENCES [enc].[ProcedureField] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO