CREATE TABLE [dbo].[StatePermission] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [StateForFormId] INT            NOT NULL,
    [DisplayName]    NVARCHAR (MAX) NULL,
    [RoleId]         VARCHAR (256)  NULL,
    [CanEdit]        BIT            NOT NULL,
    [ViewMode]       BIT            NOT NULL,
    CONSTRAINT [PK_StatePermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StatePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_StatePermission_StateForForm_StateForFormId] FOREIGN KEY ([StateForFormId]) REFERENCES [dbo].[StateForForm] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_StatePermission_RoleId]
    ON [dbo].[StatePermission]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StatePermission_StateForFormId]
    ON [dbo].[StatePermission]([StateForFormId] ASC);

