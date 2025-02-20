CREATE TABLE [enc].[JobRoutine] (
    [Timestamp]            DATETIME2 (7)    NOT NULL,
    [JobId]                UNIQUEIDENTIFIER NOT NULL,
    [EncounterProcedureId] UNIQUEIDENTIFIER NOT NULL,
    [FacilityRoutineId]    UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_JobRoutine_EncounterProcedure] FOREIGN KEY ([EncounterProcedureId]) REFERENCES [enc].[EncounterProcedure] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_JobRoutine_FacilityRoutine] FOREIGN KEY ([FacilityRoutineId]) REFERENCES [org].[FacilityRoutine] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_JobRoutine_Job] FOREIGN KEY ([JobId]) REFERENCES [enc].[Job] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IDX_JobRoutine]
    ON [enc].[JobRoutine]([Timestamp] DESC, [JobId] ASC);