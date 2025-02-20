CREATE TABLE [enc].[LineEncounter] (
    [LineId]        UNIQUEIDENTIFIER NOT NULL,
    [EncounterId]   UNIQUEIDENTIFIER NOT NULL,
    [Timestamp]     DATETIME2        NOT NULL,    
    CONSTRAINT [PK_LineEncounter] PRIMARY KEY CLUSTERED ([LineId] ASC, [EncounterId] ASC),
    CONSTRAINT [FK_LineEncounter_Line] FOREIGN KEY ([LineId]) REFERENCES [enc].[Line] ([Id]),
    CONSTRAINT [FK_LineEncounter_Job] FOREIGN KEY ([EncounterId]) REFERENCES [enc].[Encounter] ([Id])
);