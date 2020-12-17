CREATE TABLE [dbo].[Case] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CaseGeneratedId] VARCHAR (50)   NULL,
    [CreatedAt]       DATETIME2 (3)  NOT NULL,
    [UpdatedAt]       DATETIME2 (3)  NOT NULL,
    [UserId]          VARCHAR (256)  NULL,
    [TenantId]        INT            NOT NULL,
    [CaseFormId]      INT            NOT NULL,
    [StateId]         INT            DEFAULT ((0)) NOT NULL,
    [AssignedAt]      DATETIME2 (3)  NULL,
    [AssignedTo]      NVARCHAR (MAX) NULL,
    [DueDate]         DATETIME2 (3)  NULL,
    CONSTRAINT [PK_Case] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Case_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Case_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]),
    CONSTRAINT [FK_Case_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

