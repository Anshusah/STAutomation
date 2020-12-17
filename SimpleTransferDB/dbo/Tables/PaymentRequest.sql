CREATE TABLE [dbo].[PaymentRequest] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [Email]                  NVARCHAR (MAX)  NULL,
    [PayeeName]              NVARCHAR (MAX)  NULL,
    [RequestId]              NVARCHAR (MAX)  NULL,
    [RequestAmount]          DECIMAL (18, 2) NOT NULL,
    [JazzCashAccountNumber]  NVARCHAR (MAX)  NULL,
    [Status]                 INT             NOT NULL,
    [CreatedBy]              NVARCHAR (450)  NULL,
    [UpdatedBy]              NVARCHAR (450)  NULL,
    [CreatedDate]            DATETIME2 (3)   NOT NULL,
    [UpdatedDate]            DATETIME2 (3)   NOT NULL,
    [Currency]               NVARCHAR (MAX)  NULL,
    [DueDate]                DATETIME2 (7)   DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [PayerName]              NVARCHAR (MAX)  NULL,
    [PaymentReferenceNumber] NVARCHAR (MAX)  NULL,
    [Reason]                 INT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PaymentRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

