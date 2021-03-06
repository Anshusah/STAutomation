﻿CREATE TABLE [dbo].[Beneficiary] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [UserId]               VARCHAR (200)  NOT NULL,
    [FirstName]            VARCHAR (200)  NULL,
    [MiddleName]           VARCHAR (200)  NULL,
    [LastName]             VARCHAR (200)  NULL,
    [ShortName]            VARCHAR (200)  NULL,
    [AddressLine1]         VARCHAR (500)  NULL,
    [AddressLine2]         VARCHAR (500)  NULL,
    [Suburb]               VARCHAR (200)  NULL,
    [PostalCode]           VARCHAR (10)   NULL,
    [CityId]               INT            NOT NULL,
    [CountryId]            INT            NOT NULL,
    [EmailAddress]         VARCHAR (200)  NULL,
    [MobileNo]             VARCHAR (20)   NULL,
    [PhoneNo]              VARCHAR (20)   NULL,
    [RelationshipToBeneId] INT            NOT NULL,
    [Gender]               INT            NOT NULL,
    [Remark]               VARCHAR (500)  NULL,
    [Status]               BIT            NOT NULL,
    [UpdatedDate]          DATETIME2 (3)  NULL,
    [CreatedBy]            NVARCHAR (450) NULL,
    [CreatedDate]          DATETIME2 (3)  NOT NULL,
    [UpdatedBy]            NVARCHAR (450) NULL,
    CONSTRAINT [PK_Beneficiary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

