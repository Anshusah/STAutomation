CREATE TABLE [dbo].[ExchangeRatesHistory] (
    [HistoryId]        INT             IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_ExchangeRatesHistory] PRIMARY KEY CLUSTERED ([HistoryId] ASC)
);

