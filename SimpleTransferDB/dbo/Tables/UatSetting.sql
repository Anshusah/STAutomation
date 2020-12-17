CREATE TABLE [dbo].[UatSetting] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [PhoneNumber] VARCHAR (50) NULL,
    [Status]      BIT          NOT NULL,
    CONSTRAINT [PK_UatSetting] PRIMARY KEY CLUSTERED ([Id] ASC)
);

