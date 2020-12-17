CREATE TABLE [dbo].[SupplierBankMap] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [TransfastBankCode] VARCHAR (50)   NULL,
    [NecMoneyBankCode]  VARCHAR (50)   NULL,
    [Status]            BIT            NOT NULL,
    [UpdatedAt]         DATETIME2 (3)  NULL,
    [CreatedBy]         NVARCHAR (450) NULL,
    [CreatedAt]         DATETIME2 (3)  NOT NULL,
    [UpdatedBy]         NVARCHAR (450) NULL,
    [CountryCode]       VARCHAR (10)   DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_SupplierBankMap] PRIMARY KEY CLUSTERED ([Id] ASC)
);

