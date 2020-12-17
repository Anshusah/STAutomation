CREATE TABLE [dbo].[Message] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [ParentId]       INT           NOT NULL,
    [ClaimId]        INT           NULL,
    [Subject]        VARCHAR (50)  NULL,
    [Content]        VARCHAR (500) NULL,
    [CreatedAt]      DATETIME2 (7) NOT NULL,
    [IsRead]         BIT           NOT NULL,
    [Attachment]     VARCHAR (100) NULL,
    [From]           VARCHAR (256) NULL,
    [TenantId]       INT           NOT NULL,
    [IsNotice]       BIT           DEFAULT ((0)) NOT NULL,
    [ClientNotified] BIT           DEFAULT ((0)) NOT NULL,
    [ReceiverDelete] BIT           DEFAULT ((0)) NOT NULL,
    [SenderDelete]   BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Message_Case_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [dbo].[Case] ([Id]),
    CONSTRAINT [FK_Message_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Message_User_From] FOREIGN KEY ([From]) REFERENCES [dbo].[User] ([Id])
);

