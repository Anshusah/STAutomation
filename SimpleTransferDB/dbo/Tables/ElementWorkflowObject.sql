CREATE TABLE [dbo].[ElementWorkflowObject] (
    [Id]         NVARCHAR (450) NOT NULL,
    [TypeId]     INT            NOT NULL,
    [CaseFormId] INT            NOT NULL,
    [ElementId]  NVARCHAR (MAX) NULL,
    [Type]       NVARCHAR (MAX) NULL,
    [TenantId]   INT            NOT NULL,
    [EventType]  INT            NOT NULL,
    CONSTRAINT [PK_ElementWorkflowObject] PRIMARY KEY CLUSTERED ([Id] ASC)
);

