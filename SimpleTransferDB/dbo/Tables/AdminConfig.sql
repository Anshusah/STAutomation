CREATE TABLE [dbo].[AdminConfig] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [KeyId] NVARCHAR (MAX) NULL,
    [Value] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AdminConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

