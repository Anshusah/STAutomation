CREATE TABLE [dbo].[TransactionHistory] (
    [TransactionHistoryId] INT            IDENTITY (1, 1) NOT NULL,
    [TransactionRefNo]     VARCHAR (50)   NULL,
    [TransactionDate]      DATETIME2 (3)  NOT NULL,
    [SupplierTxnRefNo]     VARCHAR (50)   NULL,
    [Status]               INT            NOT NULL,
    [SupplierTxnStatus]    NVARCHAR (MAX) NULL,
    [Remark]               NVARCHAR (MAX) NULL,
    [RemarkBy]             NVARCHAR (MAX) NULL,
    [CreatedBy]            NVARCHAR (450) NULL,
    [UpdatedBy]            NVARCHAR (450) NULL,
    [CreatedDate]          DATETIME2 (3)  NOT NULL,
    [UpdatedDate]          DATETIME2 (3)  NULL,
    CONSTRAINT [PK_TransactionHistory] PRIMARY KEY CLUSTERED ([TransactionHistoryId] ASC)
);

