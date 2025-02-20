CREATE TABLE [enc].[ProcedureFieldOption] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [FieldId]  UNIQUEIDENTIFIER NOT NULL,
    [Archived] BIT              NOT NULL,
    [Order]    INT              DEFAULT ((2147483647)) NOT NULL,
    [Text]     NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_ProcedureFieldOption] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureFieldOption_ProcedureField] FOREIGN KEY ([FieldId]) REFERENCES [enc].[ProcedureField] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO