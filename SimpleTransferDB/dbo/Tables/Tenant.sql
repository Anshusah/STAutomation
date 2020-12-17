CREATE TABLE [dbo].[Tenant] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50)  NULL,
    [Identifier]       VARCHAR (50)  NULL,
    [PhoneNumber]      VARCHAR (11)  NULL,
    [Email]            VARCHAR (50)  NULL,
    [AddressPrimary]   VARCHAR (150) NULL,
    [AddressSecondary] VARCHAR (150) NULL,
    [PostCode]         VARCHAR (10)  NULL,
    [City]             VARCHAR (100) NULL,
    [CreatedBy]        VARCHAR (250) NULL,
    [CreatedAt]        DATETIME2 (7) NOT NULL,
    [UpdatedAt]        DATETIME2 (7) NOT NULL,
    [Status]           VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED ([Id] ASC)
);

