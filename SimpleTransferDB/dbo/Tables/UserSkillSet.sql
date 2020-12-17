CREATE TABLE [dbo].[UserSkillSet] (
    [UserId]     VARCHAR (256) NOT NULL,
    [SkillSetId] INT           NOT NULL,
    CONSTRAINT [PK_UserSkillSet] PRIMARY KEY CLUSTERED ([SkillSetId] ASC, [UserId] ASC),
    CONSTRAINT [FK_UserSkillSet_SkillSet_SkillSetId] FOREIGN KEY ([SkillSetId]) REFERENCES [dbo].[SkillSet] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserSkillSet_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserSkillSet_UserId]
    ON [dbo].[UserSkillSet]([UserId] ASC);

