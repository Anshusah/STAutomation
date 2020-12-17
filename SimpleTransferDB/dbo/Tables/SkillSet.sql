CREATE TABLE [dbo].[SkillSet] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (100) NULL,
    [CreatedAt] DATETIME2 (3)  NOT NULL,
    [UpdatedAt] DATETIME2 (3)  NOT NULL,
    [CreatedBy] VARCHAR (250)  NULL,
    [CaseLimit] INT            NOT NULL,
    [IsActive]  BIT            NOT NULL,
    [TenantId]  INT            NOT NULL,
    CONSTRAINT [PK_SkillSet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SkillSet_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_SkillSet_TenantId]
    ON [dbo].[SkillSet]([TenantId] ASC);

