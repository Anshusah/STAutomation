CREATE TABLE [dbo].[EmailGroup] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (100) NULL,
    [CreatedAt] DATETIME2 (3)  NOT NULL,
    [UpdatedAt] DATETIME2 (3)  NOT NULL,
    [CreatedBy] VARCHAR (250)  NULL,
    [TenantId]  INT            NOT NULL,
    CONSTRAINT [PK_EmailGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmailGroup_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EmailGroup_TenantId]
    ON [dbo].[EmailGroup]([TenantId] ASC);

