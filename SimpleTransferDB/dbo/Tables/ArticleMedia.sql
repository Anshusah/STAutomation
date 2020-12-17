CREATE TABLE [dbo].[ArticleMedia] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [ArticleId] INT NOT NULL,
    [MediaId]   INT NOT NULL,
    CONSTRAINT [PK_ArticleMedia] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ArticleMedia_Article_ArticleId] FOREIGN KEY ([ArticleId]) REFERENCES [dbo].[Article] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ArticleMedia_Media_MediaId] FOREIGN KEY ([MediaId]) REFERENCES [dbo].[Media] ([Id])
);

