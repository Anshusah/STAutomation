CREATE TABLE [dbo].[QueueForForm] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [CaseFormId] INT NOT NULL,
    [Order]      INT NOT NULL,
    [AllUser]    BIT NOT NULL,
    [QueueId]    INT NOT NULL,
    CONSTRAINT [PK_QueueForForm] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_QueueForForm_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_QueueForForm_Queue_QueueId] FOREIGN KEY ([QueueId]) REFERENCES [dbo].[Queue] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_QueueForForm_CaseFormId]
    ON [dbo].[QueueForForm]([CaseFormId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_QueueForForm_QueueId]
    ON [dbo].[QueueForForm]([QueueId] ASC);

