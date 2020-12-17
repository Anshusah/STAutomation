CREATE TABLE [dbo].[CountryPayoutConfig] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CountryCode]     VARCHAR (10)   NOT NULL,
    [PaymentMethodId] VARCHAR (10)   NOT NULL,
    [CreatedBy]       NVARCHAR (MAX) NULL,
    [UpdatedBy]       NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME2 (7)  NOT NULL,
    [UpdatedDate]     DATETIME2 (7)  NOT NULL,
    [IsActive]        BIT            NOT NULL,
    CONSTRAINT [PK_CountryPayoutConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

