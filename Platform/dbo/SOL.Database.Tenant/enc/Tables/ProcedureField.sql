CREATE TABLE [enc].[ProcedureField] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [ProcedureId]   UNIQUEIDENTIFIER NOT NULL,
    [Archived]      BIT              NOT NULL,
    [Order]         INT              DEFAULT ((2147483647)) NOT NULL,
    [Type]          TINYINT          NOT NULL,
    [Settings]      TINYINT          NOT NULL,    
    [Name]          NVARCHAR (255)   NOT NULL,
    [Instruction]   NVARCHAR (4000)  NULL,
    CONSTRAINT [PK_ProcedureField] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureField_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [enc].[Procedure] ([Id])
);

GO