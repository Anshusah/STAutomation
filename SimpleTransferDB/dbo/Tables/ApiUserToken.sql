CREATE TABLE [dbo].[ApiUserToken] (
    [UserTokenId]         INT           IDENTITY (1, 1) NOT NULL,
    [Token]               VARCHAR (MAX) NULL,
    [UserId]              VARCHAR (200) NULL,
    [Status]              BIT           NOT NULL,
    [TokenCreatedDate]    DATETIME2 (3) NOT NULL,
    [TokenModifiedDate]   DATETIME2 (3) NOT NULL,
    [TokenExpiryDatetime] DATETIME2 (3) NOT NULL,
    CONSTRAINT [PK_ApiUserToken] PRIMARY KEY CLUSTERED ([UserTokenId] ASC)
);

