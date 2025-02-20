CREATE TABLE [enc].[Procedure] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]           DATETIME2 (7)    NOT NULL,
    [ModifiedAt]          DATETIME2 (7)    NULL,
    [Archived]            BIT              NOT NULL,
    [Order]               INT              DEFAULT ((2147483647)) NOT NULL,
    [Name]                NVARCHAR (255)   NOT NULL,
    [Settings]            TINYINT          NOT NULL,    
    [RequiredData]        TINYINT          NOT NULL,
    CONSTRAINT [PK_Procedure] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO