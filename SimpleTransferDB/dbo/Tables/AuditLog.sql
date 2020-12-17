CREATE TABLE [dbo].[AuditLog] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Object]           NVARCHAR (MAX) NULL,
    [ObjectId]         NVARCHAR (MAX) NULL,
    [FieldName]        NVARCHAR (MAX) NULL,
    [OldValue]         NVARCHAR (MAX) NULL,
    [NewValue]         NVARCHAR (MAX) NULL,
    [TimeStamp]        DATETIME2 (7)  NOT NULL,
    [UserId]           NVARCHAR (MAX) NULL,
    [User]             NVARCHAR (MAX) NULL,
    [TenantId]         INT            NULL,
    [OperationType]    INT            NOT NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [ParentId]         NVARCHAR (MAX) NULL,
    [ParentObject]     NVARCHAR (MAX) NULL,
    [IsManual]         BIT            NULL,
    [IsDeleted]        BIT            NULL,
    [DeletedTimeStamp] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

