CREATE TABLE [dbo].[ActionsReceiver] (
    [Id]        NVARCHAR (450) NOT NULL,
    [ActionsId] INT            NOT NULL,
    [RoleId]    INT            NOT NULL,
    CONSTRAINT [PK_ActionsReceiver] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ActionsReceiver_Actions_ActionsId] FOREIGN KEY ([ActionsId]) REFERENCES [dbo].[Actions] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ActionsReceiver_ActionsId]
    ON [dbo].[ActionsReceiver]([ActionsId] ASC);

