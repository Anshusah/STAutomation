CREATE TABLE [dbo].[ElementComponent] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [FieldKey]   VARCHAR (50)   NULL,
    [FieldValue] VARCHAR (MAX)  NULL,
    [FormId]     INT            NULL,
    [ElementId]  NVARCHAR (MAX) NULL,
    [EventType]  INT            NULL,
    [TenantId]   INT            NULL,
    CONSTRAINT [PK_ElementComponent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ElementComponent_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ElementComponent_TenantId]
    ON [dbo].[ElementComponent]([TenantId] ASC);

