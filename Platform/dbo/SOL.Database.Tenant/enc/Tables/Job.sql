CREATE TABLE [enc].[Job] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]        DATETIME2        NOT NULL,
    [ModifiedAt]       DATETIME2        NULL,
    [PurposeId]        UNIQUEIDENTIFIER NOT NULL,
    [RoomId]           UNIQUEIDENTIFIER NOT NULL, 
    [LineId]           UNIQUEIDENTIFIER NULL,
    [MedicalRecordId]  UNIQUEIDENTIFIER NULL, 
    [OriginProcedureId]UNIQUEIDENTIFIER NULL,
    [OrderingProvider] NVARCHAR (255)   NULL,
    [Contact]          NVARCHAR (255)   NULL,
    [ScheduledDate]    DATE             NULL,
    [ScheduledTime]    TIME(0)          NULL,
    [DeletedAt]        DATETIME2        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Job_Purpose] FOREIGN KEY ([PurposeId]) REFERENCES [enc].[Purpose] ([Id]),
    CONSTRAINT [FK_Job_Room] FOREIGN KEY ([RoomId]) REFERENCES [org].[FacilityRoom] ([Id])
);

GO