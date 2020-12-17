CREATE TABLE [dbo].[ElementWorkflowState] (
    [Id]                    NVARCHAR (450) NOT NULL,
    [FromStateId]           BIGINT         NOT NULL,
    [ToStateId]             BIGINT         NOT NULL,
    [CaseFormId]            INT            NOT NULL,
    [ElementId]             NVARCHAR (MAX) NULL,
    [EventType]             INT            NOT NULL,
    [BeforeChangeActionsId] NVARCHAR (MAX) NULL,
    [TenantId]              INT            NOT NULL,
    [AfterChangeActionsId]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ElementWorkflowState] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ElementWorkflowState_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ElementWorkflowState_CaseFormId]
    ON [dbo].[ElementWorkflowState]([CaseFormId] ASC);

