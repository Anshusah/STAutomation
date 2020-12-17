CREATE TABLE [dbo].[PermissionGroup] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100)  NULL,
    [PermissionIds] NVARCHAR (MAX) NULL,
    [CreatedAt]     DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [CreatedBy]     NVARCHAR (MAX) NULL,
    [TenantId]      INT            DEFAULT ((0)) NOT NULL,
    [UpdatedAt]     DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [UpdatedBy]     NVARCHAR (MAX) NULL,
    [CaseFormId]    INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PermissionGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

