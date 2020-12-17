CREATE TABLE [dbo].[OnfidoCheck] (
    [id]                      NVARCHAR (450) NOT NULL,
    [ChecksId]                NVARCHAR (MAX) NULL,
    [created_at]              DATETIME2 (7)  NOT NULL,
    [updated_at]              DATETIME2 (7)  NULL,
    [status]                  NVARCHAR (MAX) NULL,
    [redirect_uri]            NVARCHAR (MAX) NULL,
    [result]                  NVARCHAR (MAX) NULL,
    [sandbox]                 BIT            NOT NULL,
    [tags]                    NVARCHAR (MAX) NULL,
    [results_uri]             NVARCHAR (MAX) NULL,
    [form_uri]                NVARCHAR (MAX) NULL,
    [paused]                  BIT            NOT NULL,
    [version]                 NVARCHAR (MAX) NULL,
    [report_ids]              NVARCHAR (MAX) NULL,
    [href]                    NVARCHAR (MAX) NULL,
    [applicant_id]            NVARCHAR (MAX) NULL,
    [applicant_provides_data] BIT            NOT NULL,
    [CreatedBy]               NVARCHAR (450) NULL,
    [UpdatedBy]               NVARCHAR (450) NULL,
    CONSTRAINT [PK_OnfidoCheck] PRIMARY KEY CLUSTERED ([id] ASC)
);

