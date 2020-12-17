CREATE TABLE [dbo].[AutoSchedulerSetting] (
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [CreatedBy] NVARCHAR (MAX) NULL,
    [UpdatedBy] NVARCHAR (MAX) NULL,
    [UpdatedAt] DATETIME2 (7)  NOT NULL,
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      INT            NOT NULL,
    [Hour]      NVARCHAR (MAX) NULL,
    [Minutes]   NVARCHAR (MAX) NULL,
    [Interval]  NVARCHAR (MAX) NULL,
    [IsActive]  BIT            NOT NULL,
    CONSTRAINT [PK_AutoSchedulerSetting] PRIMARY KEY CLUSTERED ([Id] ASC)
);

