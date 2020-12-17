CREATE TABLE [dbo].[WorkflowObject] (
    [Id]         NVARCHAR (450) NOT NULL,
    [TypeId]     INT            NOT NULL,
    [CaseFormId] INT            NOT NULL,
    [Type]       NVARCHAR (MAX) NULL,
    [TenantId]   INT            NOT NULL,
    CONSTRAINT [PK_WorkflowObject] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowObject_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkflowObject_CaseFormId]
    ON [dbo].[WorkflowObject]([CaseFormId] ASC);

