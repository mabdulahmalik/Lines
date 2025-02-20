CREATE TABLE [org].[FacilityProcedure] (
    [FacilityId]  UNIQUEIDENTIFIER NOT NULL,
    [ProcedureId] UNIQUEIDENTIFIER NOT NULL,
    [Timestamp]   DATETIME2         NOT NULL,
    CONSTRAINT [PK_FacilityProcedure] PRIMARY KEY CLUSTERED ([FacilityId] ASC, [ProcedureId] ASC),
    CONSTRAINT [FK_FacilityProcedure_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [org].[Facility] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_FacilityProcedure_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [enc].[Procedure] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO