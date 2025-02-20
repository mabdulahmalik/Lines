CREATE TABLE [usr].[UserStatus] (
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [Availability]  TINYINT          NOT NULL,
    [ChangedAt]     DATETIME2 (7)    NOT NULL,
    [Message]       NVARCHAR(255)    NULL,
    CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED ([UserId], [ChangedAt])
);

GO