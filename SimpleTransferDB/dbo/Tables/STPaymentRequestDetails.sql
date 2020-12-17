CREATE TABLE [dbo].[STPaymentRequestDetails] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [STPaymentRequestId] NVARCHAR (MAX) NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [PurposeOfRequest]   INT            NOT NULL,
    [Bank]               NVARCHAR (MAX) NULL,
    [Branch]             NVARCHAR (MAX) NULL,
    [AccountNumber]      NVARCHAR (MAX) NULL,
    [Invoice]            NVARCHAR (MAX) NULL,
    [Reminder]           NVARCHAR (MAX) NULL,
    [DueDate]            DATETIME2 (7)  NOT NULL,
    [CreatedBy]          NVARCHAR (MAX) NULL,
    [UpdatedBy]          NVARCHAR (MAX) NULL,
    [CreatedDate]        DATETIME2 (7)  NOT NULL,
    [UpdatedDate]        DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_STPaymentRequestDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

