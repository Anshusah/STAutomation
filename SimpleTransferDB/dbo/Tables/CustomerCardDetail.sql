CREATE TABLE [dbo].[CustomerCardDetail] (
    [UserId]     NVARCHAR (MAX) NULL,
    [CustomerId] NVARCHAR (MAX) NULL,
    [Type]       NVARCHAR (MAX) NULL,
    [Number]     NVARCHAR (MAX) NULL,
    [ExpDate]    NVARCHAR (MAX) NULL,
    [Csv]        NVARCHAR (MAX) NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [UpdatedAt]  DATETIME2 (7)  NOT NULL,
    [TenantId]   INT            NOT NULL,
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_CustomerCardDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

