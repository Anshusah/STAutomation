CREATE TABLE [dbo].[MailMergeObject] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [FormId]      INT            NOT NULL,
    [Title]       NVARCHAR (MAX) NULL,
    [IsActive]    BIT            NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [TemplateId]  INT            NOT NULL,
    [CreatedDate] DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [TenantId]    INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MailMergeObject] PRIMARY KEY CLUSTERED ([Id] ASC)
);

