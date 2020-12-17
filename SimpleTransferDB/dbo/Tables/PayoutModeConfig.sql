CREATE TABLE [dbo].[PayoutModeConfig] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [TenantId]       INT           NOT NULL,
    [PayoutModeName] VARCHAR (500) NULL,
    [Status]         BIT           NOT NULL,
    [CreatedBy]      VARCHAR (500) NULL,
    [UpdatedBy]      VARCHAR (500) NULL,
    [UpdatedDate]    DATETIME2 (3) NOT NULL,
    [CreatedDate]    DATETIME2 (3) NOT NULL,
    CONSTRAINT [PK_PayoutModeConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

