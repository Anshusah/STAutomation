CREATE TABLE [dbo].[SupplierBankBranch] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SupplierId]  INT            NOT NULL,
    [BranchName]  VARCHAR (MAX)  NULL,
    [BranchCode]  VARCHAR (50)   NULL,
    [BankCode]    VARCHAR (50)   NULL,
    [CountryCode] VARCHAR (50)   NULL,
    [Status]      BIT            NOT NULL,
    [UpdatedAt]   DATETIME2 (3)  NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [CreatedAt]   DATETIME2 (3)  NOT NULL,
    [CityCode]    VARCHAR (50)   NULL,
    [UpdatedBy]   NVARCHAR (450) NULL,
    CONSTRAINT [PK_SupplierBankBranch] PRIMARY KEY CLUSTERED ([Id] ASC)
);

