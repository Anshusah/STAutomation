CREATE TABLE [dbo].[RoleClaim] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]     VARCHAR (256)  NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_RoleClaim] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RoleClaim_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
);

