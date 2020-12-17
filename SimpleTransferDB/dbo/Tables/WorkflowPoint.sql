CREATE TABLE [dbo].[WorkflowPoint] (
    [Id]         NVARCHAR (450) NOT NULL,
    [CaseFormId] INT            NOT NULL,
    [FWObjectId] NVARCHAR (450) NULL,
    [LWObjectId] NVARCHAR (450) NULL,
    [TenantId]   INT            NOT NULL,
    [Type]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_WorkflowPoint] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowPoint_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkflowPoint_WorkflowObject_FWObjectId] FOREIGN KEY ([FWObjectId]) REFERENCES [dbo].[WorkflowObject] ([Id]),
    CONSTRAINT [FK_WorkflowPoint_WorkflowObject_LWObjectId] FOREIGN KEY ([LWObjectId]) REFERENCES [dbo].[WorkflowObject] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkflowPoint_CaseFormId]
    ON [dbo].[WorkflowPoint]([CaseFormId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkflowPoint_FWObjectId]
    ON [dbo].[WorkflowPoint]([FWObjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkflowPoint_LWObjectId]
    ON [dbo].[WorkflowPoint]([LWObjectId] ASC);

