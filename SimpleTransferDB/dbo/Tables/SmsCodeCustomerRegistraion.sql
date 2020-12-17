CREATE TABLE [dbo].[SmsCodeCustomerRegistraion] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [TenantId]          INT            NOT NULL,
    [MobileNumber]      NVARCHAR (MAX) NULL,
    [Email]             NVARCHAR (MAX) NULL,
    [SmsSuccess]        BIT            NOT NULL,
    [CustomerSuccess]   BIT            NOT NULL,
    [RetryCount]        INT            NOT NULL,
    [Status]            BIT            NOT NULL,
    [ExpiryMinute]      INT            NOT NULL,
    [RequestedDateTime] DATETIME2 (3)  NOT NULL,
    [ExpiryDateTime]    DATETIME2 (3)  NOT NULL,
    [SmsCode]           INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SmsCodeCustomerRegistraion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

