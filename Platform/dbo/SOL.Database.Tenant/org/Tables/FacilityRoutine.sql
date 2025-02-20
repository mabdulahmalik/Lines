CREATE TABLE [org].[FacilityRoutine] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [FacilityId] UNIQUEIDENTIFIER NOT NULL,
    [RoutineId]  UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (80)    NOT NULL,
    CONSTRAINT [PK_FacilityRoutine] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FacilityRoutine_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [org].[Facility] ([Id]),
    CONSTRAINT [FK_FacilityRoutine_Routine] FOREIGN KEY ([RoutineId]) REFERENCES [org].[Routine] ([Id])
);

GO