﻿CREATE TABLE [dbo].[OnfidoApplicant] (
    [id]              NVARCHAR (450) NOT NULL,
    [CustomerId]      NVARCHAR (MAX) NULL,
    [created_at]      DATETIME2 (7)  NOT NULL,
    [updated_at]      DATETIME2 (7)  NULL,
    [sandbox]         BIT            NOT NULL,
    [first_name]      NVARCHAR (MAX) NOT NULL,
    [last_name]       NVARCHAR (MAX) NOT NULL,
    [email]           NVARCHAR (MAX) NULL,
    [dob]             NVARCHAR (MAX) NULL,
    [delete_at]       DATETIME2 (7)  NULL,
    [href]            NVARCHAR (MAX) NULL,
    [flat_number]     NVARCHAR (MAX) NULL,
    [building_number] NVARCHAR (MAX) NULL,
    [building_name]   NVARCHAR (MAX) NULL,
    [street]          NVARCHAR (MAX) NULL,
    [sub_street]      NVARCHAR (MAX) NULL,
    [town]            NVARCHAR (MAX) NULL,
    [state]           NVARCHAR (MAX) NULL,
    [postcode]        NVARCHAR (MAX) NULL,
    [country]         NVARCHAR (MAX) NULL,
    [line1]           NVARCHAR (MAX) NULL,
    [line2]           NVARCHAR (MAX) NULL,
    [line3]           NVARCHAR (MAX) NULL,
    [id_numbers]      NVARCHAR (MAX) NULL,
    [Status]          BIT            NOT NULL,
    [CreatedBy]       NVARCHAR (450) NULL,
    [UpdatedBy]       NVARCHAR (450) NULL,
    [ApplicantId]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OnfidoApplicant] PRIMARY KEY CLUSTERED ([id] ASC)
);

