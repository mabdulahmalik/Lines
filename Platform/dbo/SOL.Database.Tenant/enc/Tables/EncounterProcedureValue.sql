CREATE TABLE [enc].[EncounterProcedureValue] (
    [EncounterProcedureId] UNIQUEIDENTIFIER NOT NULL,
    [ProcedureFieldId]     UNIQUEIDENTIFIER NOT NULL,
    [Value]                NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_EncounterProcedureValue] PRIMARY KEY CLUSTERED ([EncounterProcedureId] ASC, [ProcedureFieldId] ASC),
    CONSTRAINT [FK_EncounterProcedureValue_EncounterProcedure] FOREIGN KEY ([EncounterProcedureId]) REFERENCES [enc].[EncounterProcedure] ([Id]),
    CONSTRAINT [FK_EncounterProcedureValue_ProcedureField] FOREIGN KEY ([ProcedureFieldId]) REFERENCES [enc].[ProcedureField] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);