CREATE TABLE [dbo].[MessageUser] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [MessageId] INT           NOT NULL,
    [UserId]    VARCHAR (256) NULL,
    CONSTRAINT [PK_MessageUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MessageUser_Message_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Message] ([Id]),
    CONSTRAINT [FK_MessageUser_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

