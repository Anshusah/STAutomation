CREATE TABLE [dbo].[ExchangeRates] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [DateTime]         DATETIME2 (7)   NULL,
    [FromCountryCode]  NVARCHAR (MAX)  NULL,
    [ToCountryCode]    NVARCHAR (MAX)  NULL,
    [FromCurrencyCode] NVARCHAR (MAX)  NULL,
    [ToCurrencyCode]   NVARCHAR (MAX)  NULL,
    [Bank]             NVARCHAR (MAX)  NULL,
    [ModeOfPayment]    INT             NOT NULL,
    [ExchangeRate]     DECIMAL (18, 4) NULL,
    [Source]           NVARCHAR (MAX)  NULL,
    [UpdatedBy]        NVARCHAR (MAX)  NULL,
    [UpdatedOn]        DATETIME2 (7)   NULL,
    [BankCode]         NVARCHAR (MAX)  NULL,
    [IsActive]         BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ExchangeRates] PRIMARY KEY CLUSTERED ([Id] ASC)
);

