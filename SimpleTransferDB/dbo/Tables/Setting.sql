CREATE TABLE [dbo].[Setting] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [FieldKey]       VARCHAR (50)  NULL,
    [FieldValue]     VARCHAR (MAX) NULL,
    [FieldDisplay]   VARCHAR (200) NULL,
    [FieldVisiblity] INT           NOT NULL,
    [FieldType]      VARCHAR (50)  NULL,
    [FieldOptions]   VARCHAR (500) NULL,
    [FieldGridSize]  VARCHAR (200) NULL,
    [TenantId]       INT           NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Setting_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);

