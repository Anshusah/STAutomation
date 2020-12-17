CREATE TABLE [dbo].[TransactionLimitConfig] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [TenantId]            INT             NOT NULL,
    [CountryCode]         NVARCHAR (MAX)  NULL,
    [LimitAmountPerTxn]   DECIMAL (18, 2) NOT NULL,
    [LimitAmountPerDay]   DECIMAL (18, 2) NOT NULL,
    [LimitAmountPerMonth] DECIMAL (18, 2) NOT NULL,
    [LimitNoPerDay]       INT             NOT NULL,
    [LimitNoPerMonth]     INT             NOT NULL,
    [Status]              BIT             NOT NULL,
    [CreatedBy]           VARCHAR (500)   NULL,
    [UpdatedBy]           VARCHAR (500)   NULL,
    [UpdatedDate]         DATETIME2 (3)   NOT NULL,
    [CreatedDate]         DATETIME2 (3)   NOT NULL,
    CONSTRAINT [PK_TransactionLimitConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

