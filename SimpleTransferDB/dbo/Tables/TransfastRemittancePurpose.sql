﻿CREATE TABLE [dbo].[TransfastRemittancePurpose] (
    [TenantId]    INT            NOT NULL,
    [CountryCode] NVARCHAR (MAX) NULL,
    [Name]        VARCHAR (100)  NULL,
    [Status]      BIT            NOT NULL,
    [UpdatedDate] DATETIME2 (3)  NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [UpdatedBy]   NVARCHAR (450) NULL,
    [CreatedDate] DATETIME2 (3)  NOT NULL,
    [Id]          INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TransfastRemittancePurpose] PRIMARY KEY CLUSTERED ([Id] ASC)
);
