CREATE TABLE [dbo].[RateSupplier] (
    [CreatedAt]    DATETIME2 (7)  NOT NULL,
    [CreatedBy]    NVARCHAR (MAX) NULL,
    [UpdatedBy]    NVARCHAR (MAX) NULL,
    [UpdatedAt]    DATETIME2 (7)  NOT NULL,
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [Username]     NVARCHAR (MAX) NULL,
    [Password]     NVARCHAR (MAX) NULL,
    [SystemId]     NVARCHAR (MAX) NULL,
    [RatePriority] INT            NOT NULL,
    [IsActive]     BIT            NOT NULL,
    CONSTRAINT [PK_RateSupplier] PRIMARY KEY CLUSTERED ([Id] ASC)
);

