CREATE TABLE [dbo].[RateSupplierFeeConfig] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [TenantId]         INT             NOT NULL,
    [SupplierId]       INT             NOT NULL,
    [PayoutModeId]     INT             NOT NULL,
    [UpperLimitAmount] DECIMAL (18, 2) NOT NULL,
    [LowerLimitAmount] DECIMAL (18, 2) NOT NULL,
    [FeeAmount]        DECIMAL (18, 2) NOT NULL,
    [FeePercentage]    DECIMAL (18, 2) NOT NULL,
    [Remark]           NVARCHAR (MAX)  NULL,
    [CreatedDate]      DATETIME2 (7)   NOT NULL,
    [CreatedBy]        NVARCHAR (MAX)  NULL,
    [UpdatedBy]        NVARCHAR (MAX)  NULL,
    [UpdatedDate]      DATETIME2 (7)   NOT NULL,
    [Status]           BIT             NOT NULL,
    [CountryCode]      NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_RateSupplierFeeConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

