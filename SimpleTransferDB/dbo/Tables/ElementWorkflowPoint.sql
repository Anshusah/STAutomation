CREATE TABLE [dbo].[ElementWorkflowPoint] (
    [Id]         NVARCHAR (450) NOT NULL,
    [CaseFormId] INT            NOT NULL,
    [FWObjectId] NVARCHAR (450) NULL,
    [LWObjectId] NVARCHAR (450) NULL,
    [TenantId]   INT            NOT NULL,
    [Type]       NVARCHAR (MAX) NULL,
    [ElementId]  NVARCHAR (MAX) NULL,
    [EventType]  INT            NOT NULL,
    CONSTRAINT [PK_ElementWorkflowPoint] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ElementWorkflowPoint_ElementWorkflowObject_FWObjectId] FOREIGN KEY ([FWObjectId]) REFERENCES [dbo].[ElementWorkflowObject] ([Id]),
    CONSTRAINT [FK_ElementWorkflowPoint_ElementWorkflowObject_LWObjectId] FOREIGN KEY ([LWObjectId]) REFERENCES [dbo].[ElementWorkflowObject] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ElementWorkflowPoint_FWObjectId]
    ON [dbo].[ElementWorkflowPoint]([FWObjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ElementWorkflowPoint_LWObjectId]
    ON [dbo].[ElementWorkflowPoint]([LWObjectId] ASC);

