CREATE TABLE [usr].[UserProfileExt] (
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [Preferences] TINYINT          NOT NULL,
    [InTraining]  BIT              NOT NULL,
    CONSTRAINT [PK_UserProfileExt] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

GO