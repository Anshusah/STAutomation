CREATE TABLE [dbo].[UserMediaGroup] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [UserId]  VARCHAR (256)  NULL,
    [GroupId] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UserMediaGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserMediaGroup_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserMediaGroup_UserId]
    ON [dbo].[UserMediaGroup]([UserId] ASC);

