CREATE TABLE [dbo].[Queue] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)   NULL,
    [CreatedAt]     DATETIME2 (7)  NOT NULL,
    [UpdatedAt]     DATETIME2 (7)  NOT NULL,
    [CreatedBy]     NVARCHAR (MAX) NULL,
    [UpdatedBy]     NVARCHAR (MAX) NULL,
    [Status]        BIT            NOT NULL,
    [UrlIdentifier] NVARCHAR (MAX) NULL,
    [TenantId]      INT            NOT NULL,
    [CaseFormId]    INT            DEFAULT ((0)) NOT NULL,
    [Icon]          NVARCHAR (255) NULL,
    [Color]         VARCHAR (50)   NULL,
    CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Queue_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]) ON DELETE CASCADE
);

