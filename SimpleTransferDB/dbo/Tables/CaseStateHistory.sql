CREATE TABLE [dbo].[CaseStateHistory] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CaseId]          INT            NOT NULL,
    [UpdatedAt]       DATETIME2 (3)  NOT NULL,
    [UpdatedBy]       NVARCHAR (MAX) NULL,
    [PreviousStateId] INT            NULL,
    [CurrentStateId]  INT            NULL,
    [Reason]          NVARCHAR (MAX) NULL,
    [UserId]          VARCHAR (256)  NULL,
    CONSTRAINT [PK_CaseStateHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CaseStateHistory_Case_CaseId] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[Case] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CaseStateHistory_State_CurrentStateId] FOREIGN KEY ([CurrentStateId]) REFERENCES [dbo].[State] ([Id]),
    CONSTRAINT [FK_CaseStateHistory_State_PreviousStateId] FOREIGN KEY ([PreviousStateId]) REFERENCES [dbo].[State] ([Id]),
    CONSTRAINT [FK_CaseStateHistory_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CaseStateHistory_CaseId]
    ON [dbo].[CaseStateHistory]([CaseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CaseStateHistory_CurrentStateId]
    ON [dbo].[CaseStateHistory]([CurrentStateId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CaseStateHistory_PreviousStateId]
    ON [dbo].[CaseStateHistory]([PreviousStateId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CaseStateHistory_UserId]
    ON [dbo].[CaseStateHistory]([UserId] ASC);

