CREATE TABLE [dbo].[CaseForm] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100)  NULL,
    [Fields]        NVARCHAR (MAX) NULL,
    [CreatedAt]     DATETIME2 (3)  NOT NULL,
    [UpdatedAt]     DATETIME2 (3)  NOT NULL,
    [UserId]        NVARCHAR (MAX) NULL,
    [TenantId]      INT            NULL,
    [Status]        BIT            DEFAULT ((0)) NOT NULL,
    [ModelName]     VARCHAR (50)   NULL,
    [UrlIdentifier] VARCHAR (50)   NULL,
    [Icon]          VARCHAR (50)   NULL,
    [ModelTitle]    VARCHAR (50)   NULL,
    [Default]       INT            NULL,
	[TabEnable] [varchar](255) NULL
    CONSTRAINT [PK_CaseForm] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CaseForm_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);

