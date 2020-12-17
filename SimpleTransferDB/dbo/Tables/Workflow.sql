CREATE TABLE [dbo].[Workflow] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [CaseFormId] INT            NOT NULL,
    [TenantId]   INT            NOT NULL,
    [JsonData]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

