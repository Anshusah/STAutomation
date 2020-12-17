CREATE TABLE [dbo].[SupplierCity] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SupplierId]  INT            NOT NULL,
    [CityCode]    VARCHAR (MAX)  NULL,
    [CityName]    VARCHAR (150)  NULL,
    [CountryCode] VARCHAR (150)  NULL,
    [StateId]     VARCHAR (150)  NULL,
    [StateName]   VARCHAR (150)  NULL,
    [Status]      BIT            NOT NULL,
    [UpdatedAt]   DATETIME2 (3)  NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [CreatedAt]   DATETIME2 (3)  NOT NULL,
    [UpdatedBy]   NVARCHAR (450) NULL,
    CONSTRAINT [PK_SupplierCity] PRIMARY KEY CLUSTERED ([Id] ASC)
);

