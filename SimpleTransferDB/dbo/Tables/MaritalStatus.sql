CREATE TABLE [dbo].[MaritalStatus] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]          INT            NOT NULL,
    [MaritalStatusName] VARCHAR (100)  NULL,
    [Status]            BIT            NOT NULL,
    [UpdatedDate]       DATETIME2 (3)  NULL,
    [UpdatedBy]         NVARCHAR (450) NULL,
    [CreatedBy]         NVARCHAR (450) NULL,
    [CreatedDate]       DATETIME2 (3)  NOT NULL,
    CONSTRAINT [PK_MaritalStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

