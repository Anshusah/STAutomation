CREATE TABLE [dbo].[UserMedia] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [UserId]  VARCHAR (256) NULL,
    [MediaId] INT           NOT NULL,
    CONSTRAINT [PK_UserMedia] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserMedia_Media_MediaId] FOREIGN KEY ([MediaId]) REFERENCES [dbo].[Media] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserMedia_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

