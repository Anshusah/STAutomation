CREATE TABLE [dbo].[RolePermission] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [RoleId]            VARCHAR (256) NULL,
    [PermissionId]      INT           NOT NULL,
    [PermissionGroupId] INT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePermission_Permission_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
);

