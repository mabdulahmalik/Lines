CREATE TABLE [org].[FacilityPurpose] (
    [FacilityId] UNIQUEIDENTIFIER NOT NULL,
    [PurposeId]  UNIQUEIDENTIFIER NOT NULL,
    [Timestamp]  DATETIME2        NOT NULL,
    CONSTRAINT [PK_FacilityPurpose] PRIMARY KEY CLUSTERED ([FacilityId] ASC, [PurposeId] ASC),
    CONSTRAINT [FK_FacilityPurpose_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [org].[Facility] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_FacilityPurpose_Purpose] FOREIGN KEY ([PurposeId]) REFERENCES [enc].[Purpose] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO