CREATE TABLE [dbo].[Component] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [FieldKey]       VARCHAR (50)  NULL,
    [FieldValue]     VARCHAR (MAX) NULL,
    [FieldDisplay]   VARCHAR (200) NULL,
    [FieldVisiblity] INT           NOT NULL,
    [ComponentType]  VARCHAR (50)  NULL,
    [FieldOptions]   VARCHAR (500) NULL,
    [FieldGridSize]  VARCHAR (200) NULL,
    [TenantId]       INT           NULL,
    CONSTRAINT [PK_Component] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Component_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Component_TenantId]
    ON [dbo].[Component]([TenantId] ASC);

