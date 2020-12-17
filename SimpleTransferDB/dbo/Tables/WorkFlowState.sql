CREATE TABLE [dbo].[WorkFlowState] (
    [Id]                    NVARCHAR (450) NOT NULL,
    [FromStateId]           INT            NOT NULL,
    [ToStateId]             INT            NOT NULL,
    [CaseFormId]            INT            NOT NULL,
    [BeforeChangeActionsId] NVARCHAR (MAX) NULL,
    [AfterChangeActionsId]  NVARCHAR (MAX) NULL,
    [TenantId]              INT            CONSTRAINT [DF__WorkFlowS__Tenan__7EF6D905] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_WorkFlowState_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkFlowState_CaseFormId]
    ON [dbo].[WorkFlowState]([CaseFormId] ASC);

