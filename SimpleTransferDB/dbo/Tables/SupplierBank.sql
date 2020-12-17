CREATE TABLE [dbo].[SupplierBank] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SupplierId]  INT            NOT NULL,
    [BankName]    VARCHAR (MAX)  NULL,
    [BankCode]    VARCHAR (50)   NULL,
    [CountryCode] VARCHAR (50)   NULL,
    [Status]      BIT            NOT NULL,
    [UpdatedAt]   DATETIME2 (3)  NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [CreatedAt]   DATETIME2 (3)  NOT NULL,
    CONSTRAINT [PK_SupplierBank] PRIMARY KEY CLUSTERED ([Id] ASC)
);

