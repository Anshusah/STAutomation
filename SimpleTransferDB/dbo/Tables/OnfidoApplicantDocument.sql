CREATE TABLE [dbo].[OnfidoApplicantDocument] (
    [id]              NVARCHAR (450) NOT NULL,
    [created_at]      DATETIME2 (7)  NOT NULL,
    [updated_at]      DATETIME2 (7)  NULL,
    [file_name]       NVARCHAR (MAX) NULL,
    [file_type]       NVARCHAR (MAX) NULL,
    [type]            NVARCHAR (MAX) NULL,
    [side]            NVARCHAR (MAX) NULL,
    [issuing_country] NVARCHAR (MAX) NULL,
    [applicant_id]    NVARCHAR (MAX) NULL,
    [CreatedBy]       NVARCHAR (450) NULL,
    [UpdatedBy]       NVARCHAR (450) NULL,
    [DocumentId]      NVARCHAR (MAX) NULL,
    [Url]             NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OnfidoApplicantDocument] PRIMARY KEY CLUSTERED ([id] ASC)
);

