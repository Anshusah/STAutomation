CREATE TABLE [dbo].[PaymentDetails] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CreatedAt]       DATETIME2 (3)  NOT NULL,
    [UpdatedAt]       DATETIME2 (3)  NOT NULL,
    [Status]          BIT            NOT NULL,
    [Extras]          NVARCHAR (MAX) NULL,
    [UserId]          VARCHAR (256)  NULL,
    [TenantId]        INT            NOT NULL,
    [CoreCaseTableId] INT            NOT NULL,
    [Order]           INT            NOT NULL,
    [CaseId]          NVARCHAR (MAX) NULL,
    [JsonExtras]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PaymentDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

