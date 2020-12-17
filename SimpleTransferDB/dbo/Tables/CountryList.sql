CREATE TABLE [dbo].[CountryList] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Code]             VARCHAR (10)   NOT NULL,
    [Name]             VARCHAR (200)  NOT NULL,
    [CurrencyCode]     NVARCHAR (MAX) NULL,
    [CurrencyName]     NVARCHAR (MAX) NULL,
    [FlagCode]         NVARCHAR (MAX) NULL,
    [FlagImageUrl]     NVARCHAR (MAX) NULL,
    [DisplayOrder]     INT            NOT NULL,
    [CreatedBy]        NVARCHAR (MAX) NULL,
    [UpdatedBy]        NVARCHAR (MAX) NULL,
    [CreatedDate]      DATETIME2 (7)  NOT NULL,
    [UpdatedDate]      DATETIME2 (7)  NOT NULL,
    [IsActive]         BIT            NOT NULL,
    [CountryPhoneCode] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CountryList] PRIMARY KEY CLUSTERED ([Id] ASC)
);

