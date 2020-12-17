CREATE TABLE [dbo].[State] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ActionName] VARCHAR (50)   NULL,
    [SystemName] VARCHAR (50)   NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [UpdatedAt]  DATETIME2 (7)  NOT NULL,
    [CreatedBy]  NVARCHAR (MAX) NULL,
    [UpdatedBy]  NVARCHAR (MAX) NULL,
    [Status]     BIT            NOT NULL,
    [Color]      VARCHAR (50)   NULL,
    [NotifyUser] BIT            NOT NULL,
    [CanEdit]    BIT            NOT NULL,
    [CanDelete]  BIT            NOT NULL,
    [NeedReason] BIT            NOT NULL,
    [TenantId]   INT            NOT NULL,
    [Icon]       VARCHAR (100)  NULL,
    [UserAccess] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_State_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]) ON DELETE CASCADE
);

