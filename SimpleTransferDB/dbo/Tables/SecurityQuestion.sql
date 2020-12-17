CREATE TABLE [dbo].[SecurityQuestion] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT            NOT NULL,
    [Question]    NVARCHAR (MAX) NULL,
    [Status]      BIT            NOT NULL,
    [CreatedBy]   NVARCHAR (450) NULL,
    [UpdatedBy]   NVARCHAR (450) NULL,
    [CreatedDate] DATETIME2 (3)  NOT NULL,
    [UpdatedDate] DATETIME2 (3)  NOT NULL,
    CONSTRAINT [PK_SecurityQuestion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

