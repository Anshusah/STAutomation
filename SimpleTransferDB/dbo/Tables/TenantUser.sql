CREATE TABLE [dbo].[TenantUser] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [UserId]   VARCHAR (256) NULL,
    [TenantId] INT           NOT NULL,
    CONSTRAINT [PK_TenantUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TenantUser_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]),
    CONSTRAINT [FK_TenantUser_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

