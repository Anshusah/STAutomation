CREATE TABLE [dbo].[StateToState] (
    [FromStateId] INT           NOT NULL,
    [ToStateId]   INT           NOT NULL,
    [CaseFormId]  INT           DEFAULT ((0)) NOT NULL,
    [LinePosX]    VARCHAR (200) NULL,
    [LinePosY]    VARCHAR (200) NULL,
    [StatePosX]   VARCHAR (200) NULL,
    [StatePosY]   VARCHAR (200) NULL,
    [Aero]        BIT           DEFAULT ((0)) NOT NULL,
    [Id]          INT           DEFAULT ((0)) NOT NULL,
    [ActionsId]   INT           NULL,
    CONSTRAINT [PK_StateToState] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StateToState_Actions_ActionsId] FOREIGN KEY ([ActionsId]) REFERENCES [dbo].[Actions] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_StateToState_State_FromStateId] FOREIGN KEY ([FromStateId]) REFERENCES [dbo].[State] ([Id]),
    CONSTRAINT [FK_StateToState_State_ToStateId] FOREIGN KEY ([ToStateId]) REFERENCES [dbo].[State] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_StateToState_ActionsId]
    ON [dbo].[StateToState]([ActionsId] ASC);

