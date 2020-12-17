CREATE TABLE [dbo].[Actions] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)   NULL,
    [CreatedAt]     DATETIME2 (7)  NOT NULL,
    [UpdatedAt]     DATETIME2 (7)  NOT NULL,
    [CreatedBy]     NVARCHAR (MAX) NULL,
    [UpdatedBy]     NVARCHAR (MAX) NULL,
    [Status]        BIT            NOT NULL,
    [UrlIdentifier] NVARCHAR (MAX) NULL,
    [ActionType]    NVARCHAR (MAX) NULL,
    [TemplateId]    INT            NOT NULL,
    [TenantId]      INT            NOT NULL,
    [CaseFormId]    INT            NOT NULL,
    [Type]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

