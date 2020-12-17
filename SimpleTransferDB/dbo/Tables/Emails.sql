CREATE TABLE [dbo].[Emails] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Emailstring]  NVARCHAR (100) NULL,
    [EmailGroupId] INT            NOT NULL,
    CONSTRAINT [PK_Emails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Emails_EmailGroup_EmailGroupId] FOREIGN KEY ([EmailGroupId]) REFERENCES [dbo].[EmailGroup] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Emails_EmailGroupId]
    ON [dbo].[Emails]([EmailGroupId] ASC);

