CREATE TABLE [dbo].[IdentificationType] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]               INT            NOT NULL,
    [IdentificationTypeName] VARCHAR (100)  NULL,
    [Status]                 BIT            NOT NULL,
    [UpdatedDate]            DATETIME2 (3)  NULL,
    [UpdatedBy]              NVARCHAR (450) NULL,
    [CreatedBy]              NVARCHAR (450) NULL,
    [CreatedDate]            DATETIME2 (3)  NOT NULL,
    [Code]                   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_IdentificationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

