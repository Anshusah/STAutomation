CREATE TABLE [dbo].[ActionsSender] (
    [Id]        NVARCHAR (450) NOT NULL,
    [ActionsId] INT            NOT NULL,
    [RoleId]    INT            NOT NULL,
    CONSTRAINT [PK_ActionsSender] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ActionsSender_Actions_ActionsId] FOREIGN KEY ([ActionsId]) REFERENCES [dbo].[Actions] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ActionsSender_ActionsId]
    ON [dbo].[ActionsSender]([ActionsId] ASC);

