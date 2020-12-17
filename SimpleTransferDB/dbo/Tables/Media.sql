CREATE TABLE [dbo].[Media] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Url]         VARCHAR (50)  NULL,
    [Title]       VARCHAR (50)  NULL,
    [Description] VARCHAR (500) NULL,
    [CreatedBy]   VARCHAR (250) NULL,
    [TenantId]    INT           NULL,
    [CaseId]      INT           NULL,
    [CreatedAt]   DATETIME2 (7) DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [ParentId]    INT           NULL,
    [Type]        INT           NULL,
    CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Media_Case_CaseId] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[Case] ([Id]),
    CONSTRAINT [FK_Media_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);

