CREATE TABLE [dbo].[BeneficiaryRelationship] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]         INT            NOT NULL,
    [RelationshipName] VARCHAR (100)  NULL,
    [Status]           BIT            NOT NULL,
    [UpdatedDate]      DATETIME2 (3)  NULL,
    [CreatedBy]        NVARCHAR (450) NULL,
    [CreatedDate]      DATETIME2 (3)  NOT NULL,
    [UpdatedBy]        NVARCHAR (450) NULL,
    CONSTRAINT [PK_BeneficiaryRelationship] PRIMARY KEY CLUSTERED ([Id] ASC)
);

