CREATE TABLE [dbo].[StateForForm] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [StateId]         INT            NOT NULL,
    [CaseFormId]      INT            NOT NULL,
    [Icon]            NVARCHAR (MAX) NULL,
    [Order]           INT            NOT NULL,
    [AllUser]         BIT            DEFAULT ((0)) NOT NULL,
    [FirstBackState]  BIT            DEFAULT ((0)) NOT NULL,
    [FirstFrontState] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StateForForm] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StateForForm_CaseForm_CaseFormId] FOREIGN KEY ([CaseFormId]) REFERENCES [dbo].[CaseForm] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StateForForm_State_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_StateForForm_CaseFormId]
    ON [dbo].[StateForForm]([CaseFormId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StateForForm_StateId]
    ON [dbo].[StateForForm]([StateId] ASC);

