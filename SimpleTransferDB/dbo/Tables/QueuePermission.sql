CREATE TABLE [dbo].[QueuePermission] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [QueueForFormId] INT            NOT NULL,
    [DisplayName]    NVARCHAR (MAX) NULL,
    [RoleId]         NVARCHAR (MAX) NULL,
    [RoleForQueueId] VARCHAR (256)  NULL,
    CONSTRAINT [PK_QueuePermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_QueuePermission_QueueForForm_QueueForFormId] FOREIGN KEY ([QueueForFormId]) REFERENCES [dbo].[QueueForForm] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_QueuePermission_Role_RoleForQueueId] FOREIGN KEY ([RoleForQueueId]) REFERENCES [dbo].[Role] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_QueuePermission_QueueForFormId]
    ON [dbo].[QueuePermission]([QueueForFormId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QueuePermission_RoleForQueueId]
    ON [dbo].[QueuePermission]([RoleForQueueId] ASC);

