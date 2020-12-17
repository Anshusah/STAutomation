CREATE TABLE [dbo].[Gender] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT            NOT NULL,
    [Name]        VARCHAR (100)  NULL,
    [Status]      BIT            NOT NULL,
    [UpdatedDate] DATETIME2 (3)  NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [CreatedDate] DATETIME2 (3)  NOT NULL,
    [UpdatedBy]   NVARCHAR (450) NULL,
    [Code]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED ([Id] ASC)
);

