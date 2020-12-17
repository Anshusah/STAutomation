CREATE TABLE [dbo].[STPaymentRequest] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [PayerId]           NVARCHAR (MAX)  NULL,
    [PayeeId]           NVARCHAR (MAX)  NULL,
    [RequestId]         NVARCHAR (MAX)  NULL,
    [RequestAmount]     DECIMAL (18, 2) NOT NULL,
    [CurrencyCode]      NVARCHAR (MAX)  NULL,
    [Status]            INT             NOT NULL,
    [CreatedBy]         NVARCHAR (MAX)  NULL,
    [UpdatedBy]         NVARCHAR (MAX)  NULL,
    [CreatedDate]       DATETIME2 (7)   NOT NULL,
    [UpdatedDate]       DATETIME2 (7)   NOT NULL,
    [PayerEmail]        NVARCHAR (MAX)  NULL,
    [PayerMobileNumber] NVARCHAR (MAX)  NULL,
    [PayeeCountry]      NVARCHAR (MAX)  NULL,
    [PayerName]         NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_STPaymentRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

