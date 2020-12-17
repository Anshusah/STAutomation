CREATE TABLE [dbo].[PaymentPurpose] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT            NOT NULL,
    [PurposeName] VARCHAR (100)  NULL,
    [Status]      BIT            NOT NULL,
    [UpdatedDate] DATETIME2 (3)  NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [CreatedDate] DATETIME2 (3)  NOT NULL,
    [UpdatedBy]   NVARCHAR (450) NULL,
    [TransfastId] INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PaymentPurpose] PRIMARY KEY CLUSTERED ([Id] ASC)
);

