CREATE TABLE [dbo].[CoreCaseTable] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (100)  NULL,
    [Fields]     NVARCHAR (MAX) NULL,
    [CreatedAt]  DATETIME2 (3)  NOT NULL,
    [UpdatedAt]  DATETIME2 (3)  NOT NULL,
    [UserId]     NVARCHAR (MAX) NULL,
    [Status]     BIT            NOT NULL,
    [TenantId]   INT            NULL,
    [CaseFormId] INT            NULL,
    CONSTRAINT [PK_CoreCaseTable] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoreCaseTable_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]),
    CONSTRAINT [FK_CoreCaseTable_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CoreCaseTable_CaseFormId]
    ON [dbo].[CoreCaseTable]([CaseFormId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CoreCaseTable_TenantId]
    ON [dbo].[CoreCaseTable]([TenantId] ASC);

