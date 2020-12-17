CREATE TABLE [dbo].[ActivityLog] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Details]   VARCHAR (500)  NULL,
    [DisplayTo] NVARCHAR (MAX) NULL,
    [IsRead]    BIT            NOT NULL,
    [CreatedOn] DATETIME2 (7)  NOT NULL,
    [UserId]    VARCHAR (256)  NULL,
    [TenantId]  INT            NULL,
    [ClaimId]   INT            NULL,
    [StateId]   INT            NULL,
    CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ActivityLog_Case_ClaimId] FOREIGN KEY ([ClaimId]) REFERENCES [dbo].[Case] ([Id]),
    CONSTRAINT [FK_ActivityLog_State_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([Id]),
    CONSTRAINT [FK_ActivityLog_Tenant_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]),
    CONSTRAINT [FK_ActivityLog_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

