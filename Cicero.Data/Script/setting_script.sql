USE [Cicerov2]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 3/25/2019 5:41:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FieldKey] [varchar](50) NULL,
	[FieldValue] [varchar](max) NULL,
	[FieldDisplay] [varchar](200) NULL,
	[FieldVisiblity] [int] NOT NULL,
	[FieldType] [varchar](50) NULL,
	[FieldOptions] [varchar](500) NULL,
	[FieldGridSize] [varchar](200) NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Setting] ON 

INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (1, N'app_name', N'', N'Name', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (2, N'app_name_frontend', N'', N'Front Title', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (3, N'app_phone', N'', N'Phone', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (4, N'app_email', N'', N'Email', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (5, N'app_address', N'', N'Address', 1, N'TEXTAREA', NULL, N'12', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (6, N'app_claim_front', N'', N'Starting Claim for Claimant', 1, N'TENANTCLAIM', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (7, N'app_claim_back', N'', N'Starting Claim for Back Office', 1, N'TENANTCLAIM', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (8, N'app_user_role', N'', N'Role for register User', 1, N'USERROLE', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (9, N'app_url', N'http://52.228.24.65/', N'Url', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (10, N'app_facebook', N'http://facebook.com', N'Facebook Url', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (11, N'app_twitter', N'http://twitter.com', N'Twitter Url', 1, N'TEXTBOX', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (12, N'Primary', N'[{"index":0,"menu":"Home","type":"custom","url":"/","desc":"","url_title":"","target":"off","childrens":[]},{"index":1,"menu":"About Us","type":"article","url":"2","desc":"","url_title":"About Us","target":"off","childrens":[]}]', N'Navigation - Primary', 0, N'TEXTBOX', NULL, NULL, NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (13, N'Bottom', N'[{"index":0,"menu":"Our Products","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"Cargo Claim","type":"article","url":"2","desc":"","css_class":"","url_title":"About Us","target":"off","childrens":[]},{"index":1,"menu":"Home Claim","type":"article","url":"3","desc":"","css_class":"","url_title":"Privacy Policy","target":"off","childrens":[]},{"index":2,"menu":"Auto Claim","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]},{"index":3,"menu":"Business Claim","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]}]},{"index":1,"menu":"Learn More","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"About Us","type":"article","url":"1","desc":"","css_class":"","url_title":"Terms and Conditions","target":"off","childrens":[]},{"index":1,"menu":"Join Us","type":"article","url":"3","desc":"","css_class":"","url_title":"Privacy Policy","target":"off","childrens":[]},{"index":2,"menu":"FAQs","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]},{"index":3,"menu":"Blog","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]},{"index":4,"menu":"News","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]}]},{"index":2,"menu":"Support","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"Contact Us","type":"article","url":"18","desc":"","css_class":"","url_title":"Contact Us","target":"off","childrens":[]},{"index":1,"menu":"Feedback","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]},{"index":2,"menu":"Help & Support","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]}]}]', N'Navigation - Bottom', 0, N'TEXTBOX', NULL, NULL, NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (14, N'app_themes', N'[{"Css":[{"Id":"style","Url":"style.css"}],"Js":[{"Id":"jquery","Url":"https://code.jquery.com/jquery-3.3.1.min.js"}],"Widgets":[{"Title":"Text","Description":"This is your shortcut description","Id":"text","Class":{}}],"WidgetLocations":[{"Id":"test","TitleWrap":"<h1>%</h1>","ContentWrap":"<p>%</p>","Status":"active"}],"Navigation":["Primary","Bottom"],"Name":"Test","Version":"0.1"},{"Css":[{"Id":"style","Url":"style.css"}],"Js":[{"Id":"jquery","Url":"https://code.jquery.com/jquery-3.3.1.min.js"}],"Widgets":[{"Title":"Text","Description":"This is your shortcut description","Id":"text","Class":{}}],"WidgetLocations":[{"Id":"test","TitleWrap":"<h1>%</h1>","ContentWrap":"<p>%</p>","Status":"active"}],"Navigation":["Primary","Bottom"],"Name":"Sea","Version":"0.1"}]', NULL, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (15, N'app_theme', N'Test', N'Theme', 1, N'TENANTTHEME', NULL, N'6', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (16, N'app_case_synchronization', N'', N'Sync Case', 1, N'CASESYNCHRONIZATION', NULL, N'12', NULL)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (17, N'app_name', N'Cicero', N'Name', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (18, N'app_name_frontend', N'Home & Contents Claim', N'Front Title', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (19, N'app_phone', N'', N'Phone', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (20, N'app_email', N'', N'Email', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (21, N'app_address', N'', N'Address', 1, N'TEXTAREA', NULL, N'12', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (22, N'app_claim_front', N'', N'Starting Claim for Claimant', 1, N'TENANTCLAIM', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (23, N'app_claim_back', N'', N'Starting Claim for Back Office', 1, N'TENANTCLAIM', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (24, N'app_user_role', N'Administrator Cicero', N'Role for register User', 1, N'USERROLE', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (25, N'app_url', N'http://52.228.24.65/', N'Url', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (26, N'app_facebook', N'http://facebook.com', N'Facebook Url', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (27, N'app_twitter', N'http://twitter.com', N'Twitter Url', 1, N'TEXTBOX', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (28, N'Primary', N'[{"index":0,"menu":"Home","type":"custom","url":"/","desc":"","url_title":"","target":"off","childrens":[]},{"index":1,"menu":"About Us","type":"article","url":"2","desc":"","url_title":"About Us","target":"off","childrens":[]}]', N'Navigation - Primary', 0, N'TEXTBOX', NULL, NULL, 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (29, N'Bottom', N'[{"index":0,"menu":"Our Products","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"Cargo Claim","type":"article","url":"2","desc":"","css_class":"","url_title":"About Us","target":"off","childrens":[]},{"index":1,"menu":"Home Claim","type":"article","url":"3","desc":"","css_class":"","url_title":"Privacy Policy","target":"off","childrens":[]},{"index":2,"menu":"Auto Claim","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]},{"index":3,"menu":"Business Claim","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]}]},{"index":1,"menu":"Learn More","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"About Us","type":"article","url":"1","desc":"","css_class":"","url_title":"Terms and Conditions","target":"off","childrens":[]},{"index":1,"menu":"Join Us","type":"article","url":"3","desc":"","css_class":"","url_title":"Privacy Policy","target":"off","childrens":[]},{"index":2,"menu":"FAQs","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]},{"index":3,"menu":"Blog","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]},{"index":4,"menu":"News","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]}]},{"index":2,"menu":"Support","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"Contact Us","type":"article","url":"18","desc":"","css_class":"","url_title":"Contact Us","target":"off","childrens":[]},{"index":1,"menu":"Feedback","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]},{"index":2,"menu":"Help & Support","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]}]}]', N'Navigation - Bottom', 0, N'TEXTBOX', NULL, NULL, 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (30, N'app_theme_widgets_test', N'[{"Location":"link","Widgets":[{"WidgetId":"05505501","WidgetType":"Themes.Blue.Widgets.CustomArticle","WidgetLocation":"link","Data":{"Title":"Cvhvh","ArticleId":null,"ArticleTitle":null,"ShortDescription":null,"AnchorLink":null,"ArticleImage":null,"ArticleImageUrl":null,"Style":"Custom","LayoutType":null,"DisplayAs":null,"CustomStyleArticle":null,"CustomStyleBanner":null,"Navigation":null,"Size":null,"Align":"left","Infinite":null,"SlideToShow":"1","SlideToScroll":"1","Speed":"200","WidgetId":"05505501","WidgetType":"Themes.Blue.Widgets.CustomArticle","WidgetLocation":"link","Data":null,"Action":"update"},"Action":"create"}]},{"Location":"aboutus","Widgets":[{"WidgetId":"56744664","WidgetType":"Themes.Blue.Widgets.Text","WidgetLocation":"aboutus","Data":{"Title":"About Us","Content":"about us","FormatContent":null,"WidgetId":"56744664","WidgetType":"Themes.Blue.Widgets.Text","WidgetLocation":"aboutus","Data":null,"Action":"update"},"Action":"create"},{"WidgetId":"60925150","WidgetType":"Themes.Blue.Widgets.Social","WidgetLocation":"aboutus","Data":{"WidgetId":"60925150","WidgetType":"Themes.Blue.Widgets.Social","WidgetLocation":"aboutus","Action":"update","Title":"Follow Us:","Url":["www.facebook.com","www.twitter.com"],"Icon":["fab fa-facebook","fab fa-linkedin"],"Style":"Custom","LayoutType":"Without Title","Target":"_new","BorderRadius":"25","ButtonColor":"#CF92DE","ButtonHoverColor":"#3BD593","ButtonBGColor":"#9869E8","ButtonBGHoverColor":"#3C598B","Data":null},"Action":"create"},{"WidgetId":"66168836","WidgetType":"Themes.Blue.Widgets.FeatureArticle","WidgetLocation":"aboutus","Data":{"WidgetId":"66168836","WidgetType":"Themes.Blue.Widgets.FeatureArticle","WidgetLocation":"aboutus","Action":"update","Title":"xxx","Style":"Custom","LayoutType":"with-title","Data":null,"Article":["45","48"],"DisplayAs":"banner","CustomStyleArticle":null,"CustomStyleBanner":null,"Navigation":"bullet","Size":null,"Align":"right"},"Action":"create"},{"WidgetId":"84550004","WidgetType":"Themes.Blue.Widgets.CustomArticle","WidgetLocation":"aboutus","Data":{"Title":"Custom Article","ArticleId":["25512303"],"ArticleTitle":["test 1"],"ShortDescription":["test 1"],"AnchorLink":["#"],"ArticleImage":["8"],"ArticleImageUrl":["0424426a-d8c6-4254-b30a-aaad4e060c9f..jpg"],"Style":"Custom","LayoutType":"with-title","DisplayAs":"article","CustomStyleArticle":"image-title-subtitle","CustomStyleBanner":null,"Navigation":null,"Size":null,"Align":"left","Infinite":null,"SlideToShow":"1","SlideToScroll":"1","Speed":"200","WidgetId":"84550004","WidgetType":"Themes.Blue.Widgets.CustomArticle","WidgetLocation":"aboutus","Data":null,"Action":"update"},"Action":"create"}]}]', NULL, 0, NULL, NULL, NULL, 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (31, N'app_theme', N'Test', N'Theme', 1, N'TENANTTHEME', NULL, N'6', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (32, N'app_case_synchronization', N'', N'Sync Case', 1, N'CASESYNCHRONIZATION', NULL, N'12', 1)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (33, N'app_name', N'HCL', N'Name', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (34, N'app_name_frontend', N'Home and Contents Claim', N'Front Title', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (35, N'app_phone', N'', N'Phone', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (36, N'app_email', N'', N'Email', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (37, N'app_address', N'', N'Address', 1, N'TEXTAREA', NULL, N'12', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (38, N'app_claim_front', N'', N'Starting Claim for Claimant', 1, N'TENANTCLAIM', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (39, N'app_claim_back', N'', N'Starting Claim for Back Office', 1, N'TENANTCLAIM', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (40, N'app_user_role', N'User HCL', N'Role for register User', 1, N'USERROLE', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (41, N'app_url', N'http://52.228.24.65/', N'Url', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (42, N'app_facebook', N'http://facebook.com', N'Facebook Url', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (43, N'app_twitter', N'http://twitter.com', N'Twitter Url', 1, N'TEXTBOX', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (44, N'Primary', N'[{"index":0,"menu":"Home","type":"custom","url":"/","desc":"","url_title":"","target":"off","childrens":[]},{"index":1,"menu":"About Us","type":"article","url":"2","desc":"","url_title":"About Us","target":"off","childrens":[]}]', N'Navigation - Primary', 0, N'TEXTBOX', NULL, NULL, 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (45, N'Bottom', N'[{"index":0,"menu":"Our Products","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"Cargo Claim","type":"article","url":"2","desc":"","css_class":"","url_title":"About Us","target":"off","childrens":[]},{"index":1,"menu":"Home Claim","type":"article","url":"3","desc":"","css_class":"","url_title":"Privacy Policy","target":"off","childrens":[]},{"index":2,"menu":"Auto Claim","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]},{"index":3,"menu":"Business Claim","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]}]},{"index":1,"menu":"Learn More","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"About Us","type":"article","url":"1","desc":"","css_class":"","url_title":"Terms and Conditions","target":"off","childrens":[]},{"index":1,"menu":"Join Us","type":"article","url":"3","desc":"","css_class":"","url_title":"Privacy Policy","target":"off","childrens":[]},{"index":2,"menu":"FAQs","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]},{"index":3,"menu":"Blog","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]},{"index":4,"menu":"News","type":"custom","url":"#","desc":"","url_title":"","target":"off","childrens":[]}]},{"index":2,"menu":"Support","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[{"index":0,"menu":"Contact Us","type":"article","url":"18","desc":"","css_class":"","url_title":"Contact Us","target":"off","childrens":[]},{"index":1,"menu":"Feedback","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]},{"index":2,"menu":"Help & Support","type":"custom","url":"#","desc":"","css_class":"","url_title":"","target":"off","childrens":[]}]}]', N'Navigation - Bottom', 0, N'TEXTBOX', NULL, NULL, 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (46, N'app_theme_widgets_test', N'[{"Location":"aboutus","Widgets":[{"WidgetId":"45785741","WidgetType":"Themes.Blue.Widgets.Text","WidgetLocation":"aboutus","Data":{"Name":"Text Block","Title":"hello","Content":"xhgdyd chgch chjvj","WidgetId":"45785741","WidgetType":"Themes.Blue.Widgets.Text","WidgetLocation":"aboutus","Data":null,"Action":"update","FormatContent":"1"},"Action":"create"},{"WidgetId":"71741688","WidgetType":"Themes.Blue.Widgets.Social","WidgetLocation":"aboutus","Data":{"Name":"Social Icon","Title":"test","Url":null,"Icon":null,"WidgetId":"71741688","WidgetType":"Themes.Blue.Widgets.Social","WidgetLocation":"aboutus","Data":null,"Action":"update","Style":"Default","LayoutType":"With Title","Target":"_new","BorderRadius":"","ButtonColor":"","ButtonHoverColor":"","ButtonBGColor":"","ButtonBGHoverColor":""},"Action":"create"},{"WidgetId":"24005180","WidgetType":"Themes.Blue.Widgets.Text","WidgetLocation":"aboutus","Data":{"WidgetId":"24005180","WidgetType":"Themes.Blue.Widgets.Text","WidgetLocation":"aboutus","Action":"update","Title":"bbxydyh","Content":"v,jghlihvliyl;h;","Data":null,"FormatContent":"1"},"Action":"create"}]}]', NULL, 0, NULL, NULL, NULL, 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (47, N'app_theme', N'Test', N'Theme', 1, N'TENANTTHEME', NULL, N'6', 2)
INSERT [dbo].[Setting] ([Id], [FieldKey], [FieldValue], [FieldDisplay], [FieldVisiblity], [FieldType], [FieldOptions], [FieldGridSize], [TenantId]) VALUES (48, N'app_case_synchronization', N'{"configs":[{"typesource":"tenant-database","tenant":"home-claim","source":"18","destination":"1","sourcefield":["text-1553143794209","text-1553143794689","select-1553143843420","text-1553144559926","text-1553144560536","text-1553144936329","text-1553145449975","select-1553145473535","text-1553149473685","text-1553149657712","radio-group-1553149736615","date-1553150450848","date-1553150451796","radio-group-1553150520006","number-1553150577311","number-1550130631332","select-1550130656132","radio-group-1550131028917","radio-group-1550131156424","radio-group-1550131156988","radio-group-1550131213921","radio-group-1550725614489","number-1550131266353","select-1550131302046","date-1550131748015","radio-group-1550132199715","select-1550132291400","radio-group-1550132518361","radio-group-1550132579759","radio-group-1550132797354","radio-group-1550133352158","radio-group-1550133428421","radio-group-1550133449896","radio-group-1550133451245","radio-group-1550133554999","radio-group-1550133557238","text-1550133632314","text-1550133634564","text-1550134052181","text-1550134092357","select-1550134114872","radio-group-1550134311965","radio-group-1550134314089","radio-group-1550134372618","radio-group-1550134709670","radio-group-1550135275554","radio-group-1550135335648","select-1550135361256","select-1550135657544","select-1550136166347","select-1550136406768","select-1550140212684","radio-group-1550140280996","number-1550140305712",null,null],"destinationfield":["FirstName","SurName","MaritialStatus","Address1","Address2","PostCode","City","Country","TelephoneNumber","Email","VatRegistered","PolicyStartDate","PolicyEndDate","HasChildren","NumberOfChildren","number-1550130631332","select-1550130656132","radio-group-1550131028917","radio-group-1550131156424","radio-group-1550131156988","radio-group-1550131213921","radio-group-1550725614489","number-1550131266353","select-1550131302046","date-1550131748015","radio-group-1550132199715","select-1550132291400","radio-group-1550132518361","radio-group-1550132579759","radio-group-1550132797354","radio-group-1550133352158","radio-group-1550133428421","radio-group-1550133449896","radio-group-1550133451245","radio-group-1550133554999","radio-group-1550133557238","text-1550133632314","text-1550133634564","text-1550134052181","text-1550134092357","select-1550134114872","radio-group-1550134311965","radio-group-1550134314089","radio-group-1550134372618","radio-group-1550134709670","radio-group-1550135275554","radio-group-1550135335648","select-1550135361256","select-1550135657544","select-1550136166347","select-1550136406768","select-1550140212684","radio-group-1550140280996","number-1550140305712",null,null],"pull":"19","pass":"20","fail":"28","policyfield":"text-1553142774522"}]}', N'Sync Case', 1, N'CASESYNCHRONIZATION', NULL, N'12', 2)
SET IDENTITY_INSERT [dbo].[Setting] OFF
ALTER TABLE [dbo].[Setting]  WITH CHECK ADD  CONSTRAINT [FK_Setting_Tenant_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([Id])
GO
ALTER TABLE [dbo].[Setting] CHECK CONSTRAINT [FK_Setting_Tenant_TenantId]
GO
