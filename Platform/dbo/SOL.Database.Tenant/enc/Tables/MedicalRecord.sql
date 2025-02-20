CREATE TABLE [enc].[MedicalRecord] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [FacilityId]  UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]   DATETIME2 (7)    NOT NULL,
    [ModifiedAt]  DATETIME2 (7)    NULL,
    [Number]      NVARCHAR (50)    NOT NULL,
    [FirstName]   NVARCHAR (50)    NULL,
    [LastName]    NVARCHAR (50)    NULL,
    [Birthday]    DATE             NULL,
    [FirstSeenOn] DATE             NULL,
    [LastSeenOn]  DATE             NULL,
    CONSTRAINT [PK_MedicalRecord] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MedicalRecord_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [org].[Facility] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_MedicalRecord]
    ON [enc].[MedicalRecord]([FacilityId] ASC, [Number] ASC);