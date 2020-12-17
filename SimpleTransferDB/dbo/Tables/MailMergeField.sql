CREATE TABLE [dbo].[MailMergeField] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [FieldName]     NVARCHAR (MAX) NULL,
    [Alias]         NVARCHAR (MAX) NULL,
    [DbSourceTable] NVARCHAR (MAX) NULL,
    [DbSourceField] NVARCHAR (MAX) NULL,
    [TemplateType]  INT            NOT NULL,
    [TenantId]      INT            NOT NULL,
    [FormId]        INT            DEFAULT ((0)) NOT NULL,
    [isDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MailMergeField] PRIMARY KEY CLUSTERED ([Id] ASC)
);

