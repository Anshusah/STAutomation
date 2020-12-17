CREATE TABLE [dbo].[OnfidoApplicantLivePhoto] (
    [id]           NVARCHAR (450) NOT NULL,
    [created_at]   DATETIME2 (7)  NOT NULL,
    [updated_at]   DATETIME2 (7)  NULL,
    [file_name]    NVARCHAR (MAX) NULL,
    [file_type]    NVARCHAR (MAX) NULL,
    [applicant_id] NVARCHAR (MAX) NULL,
    [CreatedBy]    NVARCHAR (450) NULL,
    [UpdatedBy]    NVARCHAR (450) NULL,
    [PhotoId]      NVARCHAR (MAX) NULL,
    [Url]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OnfidoApplicantLivePhoto] PRIMARY KEY CLUSTERED ([id] ASC)
);

