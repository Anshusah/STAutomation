CREATE TABLE [dbo].[ElementWorkflow] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [CaseFormId] INT            NOT NULL,
    [ElementId]  NVARCHAR (MAX) NULL,
    [EventType]  INT            NOT NULL,
    [TenantId]   INT            NOT NULL,
    [JsonData]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ElementWorkflow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

