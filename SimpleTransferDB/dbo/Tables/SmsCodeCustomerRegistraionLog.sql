CREATE TABLE [dbo].[SmsCodeCustomerRegistraionLog] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]     INT            NOT NULL,
    [MobileNumber] NVARCHAR (MAX) NULL,
    [SmsCode]      INT            NOT NULL,
    [CreatedDate]  DATETIME2 (3)  NOT NULL,
    [SmsSuccess]   BIT            NOT NULL,
    CONSTRAINT [PK_SmsCodeCustomerRegistraionLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

