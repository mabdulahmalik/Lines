CREATE TABLE [org].[FacilityType] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]  DATETIME2 (7)    NOT NULL,
    [ModifiedAt] DATETIME2 (7)    NULL,
    [Order]      INT              DEFAULT ((2147483647)) NOT NULL,
    [Active]     BIT              CONSTRAINT [DEFAULT_FacilityType_Active] DEFAULT 1 NOT NULL,
    [Name]       NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_FacilityType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO