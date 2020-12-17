CREATE TABLE [dbo].[QueueToState] (
    [StateId]    INT            NOT NULL,
    [QueueId]    INT            NOT NULL,
    [CaseFormId] INT            DEFAULT ((0)) NOT NULL,
    [LinePosX]   VARCHAR (200)  NULL,
    [LinePosY]   VARCHAR (200)  NULL,
    [PosX]       VARCHAR (200)  NULL,
    [PosY]       VARCHAR (200)  NULL,
    [Id]         NVARCHAR (450) NOT NULL,
    [IsQueue]    BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_QueueToState] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_QueueToState_Queue_QueueId] FOREIGN KEY ([QueueId]) REFERENCES [dbo].[Queue] ([Id]),
    CONSTRAINT [FK_QueueToState_State_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([Id])
);

