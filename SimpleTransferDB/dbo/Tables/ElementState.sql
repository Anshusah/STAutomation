CREATE TABLE [dbo].[ElementState] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Type]         NVARCHAR (MAX) NULL,
    [isDefaultEnd] BIT            NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [ElementId]    NVARCHAR (MAX) NULL,
    [TenantId]     INT            NOT NULL,
    [CreatedAt]    DATETIME2 (7)  NOT NULL,
    [ForEventType] INT            NOT NULL,
    [FormId]       INT            NOT NULL,
    CONSTRAINT [PK_ElementState] PRIMARY KEY CLUSTERED ([Id] ASC)
);

