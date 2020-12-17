CREATE TABLE [dbo].[SmsLog] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]        INT            NOT NULL,
    [StatusMessage]   NVARCHAR (MAX) NULL,
    [MobileNumber]    NVARCHAR (MAX) NULL,
    [Email]           NVARCHAR (MAX) NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [ResponseMessage] NVARCHAR (MAX) NULL,
    [CreatedBy]       NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME2 (3)  NOT NULL,
    [ErrorCode]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SmsLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

