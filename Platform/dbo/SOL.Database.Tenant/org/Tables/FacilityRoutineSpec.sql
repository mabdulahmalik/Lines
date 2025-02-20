CREATE TABLE [org].[FacilityRoutineProperty] (
    [FacilityRoutineId] UNIQUEIDENTIFIER NOT NULL,
    [PropertyId]        UNIQUEIDENTIFIER NOT NULL,
    [PropertyValue]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_FacilityRoutineProperty] PRIMARY KEY CLUSTERED ([FacilityRoutineId] ASC, [PropertyId] ASC, [PropertyValue] ASC),
    CONSTRAINT [FK_FacilityRoutineProperty_FacilityRoutine] FOREIGN KEY ([FacilityRoutineId]) REFERENCES [org].[FacilityRoutine] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO