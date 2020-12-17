CREATE TABLE [dbo].[Role] (
    [Id]               VARCHAR (256)  NOT NULL,
    [Name]             NVARCHAR (256) NULL,
    [NormalizedName]   NVARCHAR (256) NULL,
    [ConcurrencyStamp] NVARCHAR (MAX) NULL,
    [Type]             INT            NOT NULL,
    [Status]           SMALLINT       NOT NULL,
    [UpdatedAt]        DATETIME2 (3)  NOT NULL,
    [CreatedAt]        DATETIME2 (3)  NOT NULL,
    [TenantId]         INT            NOT NULL,
    [DisplayName]      VARCHAR (50)   NULL,
    [CreatedBy]        NVARCHAR (450) NULL,
    [OrganizationName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Role_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]) ON DELETE CASCADE
);

