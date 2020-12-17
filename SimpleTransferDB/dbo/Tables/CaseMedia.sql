CREATE TABLE [dbo].[CaseMedia] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [CaseId]  INT NOT NULL,
    [MediaId] INT NOT NULL,
    CONSTRAINT [PK_CaseMedia] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CaseMedia_Case_CaseId] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[Case] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CaseMedia_Media_MediaId] FOREIGN KEY ([MediaId]) REFERENCES [dbo].[Media] ([Id])
);

