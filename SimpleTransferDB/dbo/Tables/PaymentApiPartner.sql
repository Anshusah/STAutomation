CREATE TABLE [dbo].[PaymentApiPartner] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NOT NULL,
    [Username] NVARCHAR (MAX) NOT NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    [SystemId] NVARCHAR (MAX) NOT NULL,
    [IsActive] BIT            NOT NULL,
    CONSTRAINT [PK_PaymentApiPartner] PRIMARY KEY CLUSTERED ([Id] ASC)
);

