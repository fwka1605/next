--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO

/* 送付先マスター 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[Destination]'))
CREATE TABLE [dbo].[Destination]
([Id]                       INT                 NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[CustomerId]               INT                 NOT NULL
,[Code]                     NVARCHAR(2)         NOT NULL
,[PostalCode]               NVARCHAR(10)        NOT NULL
,[Address1]                 NVARCHAR(40)        NOT NULL
,[Address2]                 NVARCHAR(40)        NOT NULL
,[DepartmentName]           NVARCHAR(40)        NOT NULL
,[Addressee]                NVARCHAR(20)        NOT NULL
,[Honorific]                NVARCHAR(6)         NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkDestination] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,CONSTRAINT [UqDestination] UNIQUE CLUSTERED 
([CompanyId] ASC
,[CustomerId] ASC
,[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkDestinationCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Destination]')
       )
ALTER TABLE [dbo].[Destination] ADD CONSTRAINT
 [FkDestinationCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkDestinationCustomer]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Destination]')
       )
ALTER TABLE [dbo].[Destination] ADD CONSTRAINT
 [FkDestinationCustomer] FOREIGN KEY
([CustomerId]) REFERENCES [dbo].[Customer] ([Id])
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送付先ID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'CompanyId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'      , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'CustomerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送付先コード', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Code'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'PostalCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵便番号'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'PostalCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'Address1'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住所1'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Address1'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'Address2'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住所2'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Address2'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'DepartmentName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部署'        , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'DepartmentName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'Addressee'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'宛名'        , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Addressee'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'Honorific'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'敬称'        , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Honorific'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'CreateBy'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'CreateAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'UpdateBy'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Destination', N'COLUMN',N'UpdateAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO


/* 請求データ 送付先ID カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Billing', N'COLUMN',N'DestinationId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'DestinationId'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Billing]')
 and name = N'DestinationId' )
ALTER TABLE [dbo].[Billing] ADD
 [DestinationId]     INT                 NULL
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Billing', N'COLUMN',N'DestinationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送付先ID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'DestinationId'
GO


/* 基本設定用 請求書発行利用 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UsePublishInvoice'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UsePublishInvoice'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UsePublishInvoice' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UsePublishInvoice]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UsePublishInvoice'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書発行利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UsePublishInvoice'
GO


/* 基本設定用 働くDB WebAPI利用 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseHatarakuDBWebApi'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseHatarakuDBWebApi'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UseHatarakuDBWebApi' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UseHatarakuDBWebApi]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseHatarakuDBWebApi'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'働くDB WebAPI 利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseHatarakuDBWebApi'
GO


/* 基本設定用 PCA会計DX WebAPI利用 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UsePCADXWebApi'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UsePCADXWebApi'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UsePCADXWebApi' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UsePCADXWebApi]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UsePCADXWebApi'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PCA会計DX WebAPI利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UsePCADXWebApi'
GO


/* 基本設定用 督促管理利用 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseReminder'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseReminder'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UseReminder' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UseReminder]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseReminder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'督促管理利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseReminder'
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WebApiSetting]') AND type IN (N'U'))
BEGIN
CREATE TABLE [dbo].[WebApiSetting]
([CompanyId]        INT             NOT NULL
,[ApiTypeId]        INT             NOT NULL
,[BaseUri]          NVARCHAR(100)   NOT NULL
,[ApiVersion]       NVARCHAR(5)     NOT NULL
,[AccessToken]      NVARCHAR(100)   NOT NULL
,[ClientId]         NVARCHAR(100)   NOT NULL
,[ClientSecret]     NVARCHAR(100)   NOT NULL
,[ExtractSetting]   NVARCHAR(MAX)       NULL
,[OutputSetting]    NVARCHAR(MAX)       NULL
,[CreateBy]         INT             NOT NULL
,[CreateAt]         DATETIME2(3)    NOT NULL
,[UpdateBy]         INT             NOT NULL
,[UpdateAt]         DATETIME2(3)    NOT NULL
,CONSTRAINT [PkWebApiSetting] PRIMARY KEY CLUSTERED
([CompanyId]        ASC
,[ApiTypeId]        ASC )
)

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkWebApiSettingCompnay]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[WebApiSetting]')
       )
ALTER TABLE [dbo].[WebApiSetting] ADD CONSTRAINT
 [FkWebApiSettingCompnay] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
;

DECLARE @v sql_variant
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'CompanyId'
SET @v = N'WebAPI種別ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'ApiTypeId'
SET @v = N'基本URI'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'BaseUri'
SET @v = N'ApiVersion'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'ApiVersion'
SET @v = N'アクセストークン'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'AccessToken'
SET @v = N'クライアントID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'ClientId'
SET @v = N'クライアントシークレット'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'ClientSecret'
SET @v = N'抽出設定 JSON 形式で保存'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'ExtractSetting'
SET @v = N'出力設定 JSON 形式で保存'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'OutputSetting'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WebApiSetting', N'COLUMN', N'UpdateAt'

END
GO

/* 営業担当者マスター 電話番号,FAX番号 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Staff', N'COLUMN',N'Tel'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Tel'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Staff]')
 and name = N'Tel' )
ALTER TABLE [dbo].[Staff] ADD
 [Tel]     NVARCHAR(20)                 NOT NULL DEFAULT ''
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Staff', N'COLUMN',N'Tel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電話番号'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Tel'
GO

IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Staff', N'COLUMN',N'Fax'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Fax'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Staff]')
 and name = N'Fax' )
ALTER TABLE [dbo].[Staff] ADD
 [Fax]     NVARCHAR(20)                 NOT NULL DEFAULT ''
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Staff', N'COLUMN',N'Fax'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FAX番号'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Fax'
GO

/* ステータスマスターベース 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[StatusMasterBase]'))
BEGIN
CREATE TABLE [dbo].[StatusMasterBase]
([StatusType]               INT                 NOT NULL
,[Code]                     NVARCHAR(10)        NOT NULL
,[Name]                     NVARCHAR(40)        NOT NULL
,[Completed]                INT                 NOT NULL
,CONSTRAINT [PkStatusMasterBase] PRIMARY KEY CLUSTERED 
(
 [StatusType] ASC
,[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

DECLARE @v sql_variant
SET @v = N'ステータス識別'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMasterBase', N'COLUMN', N'StatusType'
SET @v = N'ステータスコード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMasterBase', N'COLUMN', N'Code'
SET @v = N'ステータス名称'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMasterBase', N'COLUMN', N'Name'
SET @v = N'完了フラグ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMasterBase', N'COLUMN', N'Completed'

END
GO

INSERT INTO StatusMasterBase
(StatusType, Code, Name, Completed)
SELECT u.*
  FROM (
          SELECT 1 [StatusType], '00' [Code], '未対応'     [Name], 0    [Completed]
UNION ALL SELECT 1 [StatusType], '99' [Code], '完了'       [Name], 1    [Completed]
) u
WHERE NOT EXISTS (
    SELECT 1
      FROM [dbo].[StatusMasterBase] b
     WHERE u.[StatusType] = b.[StatusType]
       AND u.[Code] = b.[Code]
)
     ORDER BY u.[StatusType], u.[Code]
GO

/* ステータスマスター 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[StatusMaster]'))
BEGIN
CREATE TABLE [dbo].[StatusMaster]
([Id]                       INT                 NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[StatusType]               INT                 NOT NULL
,[Code]                     NVARCHAR(10)        NOT NULL
,[Name]                     NVARCHAR(40)        NOT NULL
,[Completed]                INT                 NOT NULL
,[DisplayOrder]             INT                 NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkStatusMaster] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,CONSTRAINT [UqStatusMaster] UNIQUE CLUSTERED 
([CompanyId] ASC
,[StatusType] ASC
,[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkStatusMasterCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[StatusMaster]')
       )
ALTER TABLE [dbo].[StatusMaster] ADD CONSTRAINT
 [FkStatusMasterCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'ステータスID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'CompanyId'
SET @v = N'ステータス識別'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'StatusType'
SET @v = N'ステータスコード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'Code'
SET @v = N'ステータス名称'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'Name'
SET @v = N'完了フラグ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'Completed'
SET @v = N'表示順番'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'DisplayOrder'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'StatusMaster', N'COLUMN', N'UpdateAt'

END
GO

INSERT INTO [dbo].[StatusMaster]
     ( [CompanyId]
     , [StatusType]
     , [Code]
     , [Name]
     , [Completed]
     , [DisplayOrder]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt] )
SELECT cm.Id            [CompanyId]
     , u.StatusType     [StatusType]
     , u.Code           [Code]
     , u.Name           [Name]
     , u.Completed      [Completed]
     , 0                [DisplayOrder]
     , 0                [CreateBy]
     , getdate()        [CreateAt]
     , 0                [UpdateBy]
     , getdate()        [UpdateAt]
FROM StatusMasterBase u
  INNER JOIN [dbo].[Company] cm
      ON cm.Id = cm.Id
    AND NOT EXISTS (
       SELECT 1
         FROM [dbo].[StatusMaster] s
        WHERE s.CompanyId = cm.Id
          AND s.StatusType      = u.StatusType
          AND s.Code            = u.Code)
GO

/* 督促状共通設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderCommonSetting]'))
BEGIN
CREATE TABLE [dbo].[ReminderCommonSetting]
([CompanyId]                INT                 NOT NULL
,[OwnDepartmentName]        NVARCHAR(100)       NOT NULL
,[AccountingStaffName]      NVARCHAR(100)       NOT NULL
,[OutputDetail]             INT                 NOT NULL
,[OutputDetailItem]         NVARCHAR(50)        NOT NULL
,[ReminderManagementMode]   INT                 NOT NULL
,[DepartmentSummaryMode]    INT                 NOT NULL
,[CalculateBaseDate]        INT                 NOT NULL
,[IncludeOnTheDay]          INT                 NOT NULL
,[DisplayArrearsInterest]   INT                 NOT NULL
,[ArrearsInterestRate]      NUMERIC(6,3)        NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkReminderCommonSetting] PRIMARY KEY NONCLUSTERED
([CompanyId] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderCommonSettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderCommonSetting]')
       )
ALTER TABLE [dbo].[ReminderCommonSetting] ADD CONSTRAINT
 [FkReminderCommonSettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'CompanyId'
SET @v = N'自社部署名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'OwnDepartmentName'
SET @v = N'経理担当者名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'AccountingStaffName'
SET @v = N'債権明細を表示する'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'OutputDetail'
SET @v = N'明細に表示する項目'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'OutputDetailItem'
SET @v = N'督促管理単位'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'ReminderManagementMode'
SET @v = N'請求部門集計方法'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'DepartmentSummaryMode'
SET @v = N'滞留日数計算方法'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'CalculateBaseDate'
SET @v = N'滞留日数計算に当日を含める'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'IncludeOnTheDay'
SET @v = N'延滞利息を表示する'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'DisplayArrearsInterest'
SET @v = N'延滞料率'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'ArrearsInterestRate'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderCommonSetting', N'COLUMN', N'UpdateAt'

END
GO


/* 督促状文面設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderTemplateSetting]'))
BEGIN
CREATE TABLE [dbo].[ReminderTemplateSetting]
([Id]                       INT                 NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[Code]                     NVARCHAR(10)        NOT NULL
,[Name]                     NVARCHAR(40)        NOT NULL
,[Title]                    NVARCHAR(40)        NOT NULL
,[Greeting]                 NVARCHAR(20)        NOT NULL
,[MainText]                 NVARCHAR(400)       NOT NULL
,[SubText]                  NVARCHAR(200)       NOT NULL
,[Conclusion]               NVARCHAR(20)        NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkReminderTemplateSetting] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,CONSTRAINT [UqReminderTemplateSetting] UNIQUE CLUSTERED 
([CompanyId] ASC
,[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderTemplateSettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderTemplateSetting]')
       )
ALTER TABLE [dbo].[ReminderTemplateSetting] ADD CONSTRAINT
 [FkReminderTemplateSettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'督促状文面設定ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'CompanyId'
SET @v = N'パターンNo'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'Code'
SET @v = N'パターン名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'Name'
SET @v = N'タイトル'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'Title'
SET @v = N'頭語'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'Greeting'
SET @v = N'主文'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'MainText'
SET @v = N'注釈'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'SubText'
SET @v = N'結語'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'Conclusion'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderTemplateSetting', N'COLUMN', N'UpdateAt'

END
GO


/* 督促状レベル設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderLevelSetting]'))
BEGIN
CREATE TABLE [dbo].[ReminderLevelSetting]
([CompanyId]                INT                 NOT NULL
,[ReminderLevel]            INT                 NOT NULL
,[ReminderTemplateId]       INT                 NOT NULL
,[ArrearDays]               INT                 NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkReminderLevelSetting] PRIMARY KEY CLUSTERED
([CompanyId] ASC
,[ReminderLevel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

DECLARE @v sql_variant
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'CompanyId'
SET @v = N'レベル'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'ReminderLevel'
SET @v = N'文面ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'ReminderTemplateId'
SET @v = N'滞留日数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'ArrearDays'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderLevelSetting', N'COLUMN', N'UpdateAt'

END
GO

/* 督促状集計設定ベース 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderSummarySettingBase]'))
BEGIN
CREATE TABLE [dbo].[ReminderSummarySettingBase]
([ColumnName]               NVARCHAR(40)        NOT NULL
,[ColumnNameJp]             NVARCHAR(40)        NOT NULL
,[DisplayOrder]             INT                 NOT NULL
,[Available]                INT                 NOT NULL
,CONSTRAINT [PkReminderSummarySettingBase] PRIMARY KEY CLUSTERED
([DisplayOrder] ASC
,[ColumnName]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

DECLARE @v sql_variant
SET @v = N'項目名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySettingBase', N'COLUMN', N'ColumnName'
SET @v = N'項目名（日本語）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySettingBase', N'COLUMN', N'ColumnNameJp'
SET @v = N'表示順序'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySettingBase', N'COLUMN', N'DisplayOrder'
SET @v = N'使用可否'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySettingBase', N'COLUMN', N'Available'

END
GO

INSERT INTO ReminderSummarySettingBase
(ColumnName, ColumnNameJp, DisplayOrder, Available)
SELECT u.*
  FROM (
          SELECT 'CalculateBaseDate' [ColumnName], '当初予定日'   [ColumnNameJp], 1     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'CustomerCode'      [ColumnName], '得意先コード' [ColumnNameJp], 2     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'CurrencyCode'      [ColumnName], '通貨コード'   [ColumnNameJp], 3     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'ClosingAt'         [ColumnName], '請求締日'     [ColumnNameJp], 4     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'InvoiceCode'       [ColumnName], '請求書番号'   [ColumnNameJp], 5     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'CollectCategory'   [ColumnName], '回収区分'     [ColumnNameJp], 6     [DisplayOrder], 1    [Available]
--UNION ALL SELECT 'DestinationCode'   [ColumnName], '送付先コード' [ColumnNameJp], 7     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'Department'        [ColumnName], '請求部門'     [ColumnNameJp], 8     [DisplayOrder], 1    [Available]
UNION ALL SELECT 'Staff'             [ColumnName], '営業担当者'   [ColumnNameJp], 9     [DisplayOrder], 1    [Available]
) u
WHERE NOT EXISTS (
    SELECT 1
      FROM [dbo].[ReminderSummarySettingBase] b
     WHERE u.[ColumnName] = b.[ColumnName]
       AND u.[DisplayOrder] = b.[DisplayOrder]
)
     ORDER BY u.[ColumnName], u.[DisplayOrder]
GO

/* 督促状集計設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderSummarySetting]'))
BEGIN
CREATE TABLE [dbo].[ReminderSummarySetting]
([Id]                       INT                 NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[ColumnName]               NVARCHAR(40)        NOT NULL
,[ColumnNameJp]             NVARCHAR(40)        NOT NULL
,[DisplayOrder]             INT                 NOT NULL
,[Available]                INT                 NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkReminderSummarySetting] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,CONSTRAINT [UqReminderSummarySetting] UNIQUE CLUSTERED 
([CompanyId] ASC
,[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderSummarySettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderSummarySetting]')
       )
ALTER TABLE [dbo].[ReminderSummarySetting] ADD CONSTRAINT
 [FkReminderSummarySettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'督促状集計設定ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'CompanyId'
SET @v = N'項目名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'ColumnName'
SET @v = N'項目名（日本語）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'ColumnNameJp'
SET @v = N'表示順序'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'DisplayOrder'
SET @v = N'使用可否'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'Available'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummarySetting', N'COLUMN', N'UpdateAt'

END
GO

/* PCA Web API 機能実装 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[WebApiSetting]')
 and name = N'RefreshToken' )
BEGIN
ALTER TABLE [dbo].[WebApiSetting]
 ALTER COLUMN [AccessToken] NVARCHAR(MAX)               NOT NULL;

ALTER TABLE [dbo].[WebApiSetting]
 ADD [RefreshToken]     NVARCHAR(MAX)                 NOT NULL DEFAULT N'';

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'リフレッシュトークン'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebApiSetting', @level2type=N'COLUMN',@level2name=N'RefreshToken'

END
GO


/* 請求書番号採番設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[InvoiceNumberSetting]'))
BEGIN
CREATE TABLE [dbo].[InvoiceNumberSetting]
([CompanyId]          INT                 NOT NULL
,[UseNumbering]       INT                 NOT NULL
,[Length]             INT                 NOT NULL
,[ZeroPadding]        INT                 NOT NULL
,[ResetType]          INT                 NOT NULL
,[ResetMonth]         INT                     NULL
,[FormatType]         INT                 NOT NULL
,[DateType]           INT                     NULL
,[DateFormat]         INT                     NULL
,[FixedStringType]    INT                     NULL
,[FixedString]        NVARCHAR(100)       NOT NULL
,[DisplayFormat]      INT                 NOT NULL
,[Delimiter]          NVARCHAR(100)       NOT NULL
,[CreateBy]           INT                 NOT NULL
,[CreateAt]           DATETIME2(3)        NOT NULL
,[UpdateBy]           INT                 NOT NULL
,[UpdateAt]           DATETIME2(3)        NOT NULL
,CONSTRAINT [PkInvoiceNumberSetting] PRIMARY KEY NONCLUSTERED
([CompanyId] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkInvoiceNumberSettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceNumberSetting]')
       )
ALTER TABLE [dbo].[InvoiceNumberSetting] ADD CONSTRAINT
 [FkInvoiceNumberSettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'CompanyId'
SET @v = N'採番をおこなう'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'UseNumbering'
SET @v = N'桁数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'Length'
SET @v = N'前ゼロ設定'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'ZeroPadding'
SET @v = N'連番リセット'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'ResetType'
SET @v = N'リセット月'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'ResetMonth'
SET @v = N'採番書式設定'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'FormatType'
SET @v = N'使用日付'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'DateType'
SET @v = N'年月日フォーマット'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'DateFormat'
SET @v = N'固定文字列設定'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'FixedStringType'
SET @v = N'固定文字列'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'FixedString'
SET @v = N'書式の配置'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'DisplayFormat'
SET @v = N'区切文字'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'Delimiter'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberSetting', N'COLUMN', N'UpdateAt'

END
GO

/* 請求書パターン設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[InvoiceTemplateSetting]'))
BEGIN
CREATE TABLE [dbo].[InvoiceTemplateSetting]
([Id]                       INT                 NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[Code]                     NVARCHAR(10)        NOT NULL
,[Name]                     NVARCHAR(40)        NOT NULL
,[CollectCategoryId]        INT                     NULL
,[Title]                    NVARCHAR(40)        NOT NULL
,[Greeting]                 NVARCHAR(80)        NOT NULL
,[DisplayStaff]             INT                 NOT NULL
,[DueDateComment]           NVARCHAR(40)        NOT NULL
,[DueDateFormat]            INT                     NULL
,[TransferFeeComment]       NVARCHAR(40)        NOT NULL
,[FixedString]              NVARCHAR(20)        NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkInvoiceTemplateSetting] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,CONSTRAINT [UqInvoiceTemplateSetting] UNIQUE CLUSTERED 
([CompanyId] ASC
,[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkInvoiceTemplateSettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceTemplateSetting]')
       )
ALTER TABLE [dbo].[InvoiceTemplateSetting] ADD CONSTRAINT
 [FkInvoiceTemplateSettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkInvoiceTemplateSettingCategory]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceTemplateSetting]')
       )
ALTER TABLE [dbo].[InvoiceTemplateSetting] ADD CONSTRAINT
 [FkInvoiceTemplateSettingCategory] FOREIGN KEY
([CollectCategoryId]) REFERENCES [dbo].[Category] ([Id])

DECLARE @v sql_variant
SET @v = N'文面設定ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'CompanyId'
SET @v = N'パターンNo'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'Code'
SET @v = N'パターン名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'Name'
SET @v = N'回収区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'CollectCategoryId'
SET @v = N'タイトル'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'Title'
SET @v = N'挨拶文'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'Greeting'
SET @v = N'営業担当者を表示する'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'DisplayStaff'
SET @v = N'期日コメント'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'DueDateComment'
SET @v = N'期日フォーマット'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'DueDateFormat'
SET @v = N'手数料負担コメント'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'TransferFeeComment'
SET @v = N'請求書番号固定文字列'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'FixedString'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceTemplateSetting', N'COLUMN', N'UpdateAt'

END
GO

/* 請求書設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[InvoiceCommonSetting]'))
BEGIN
CREATE TABLE [dbo].[InvoiceCommonSetting]
([CompanyId]             INT                 NOT NULL
,[ExcludeAmountZero]     INT                 NOT NULL  DEFAULT 1
,[ExcludeMinusAmount]    INT                 NOT NULL  DEFAULT 1
,[ExcludeMatchedData]    INT                 NOT NULL  DEFAULT 0
,[CreateBy]              INT                 NOT NULL
,[CreateAt]              DATETIME2(3)        NOT NULL
,[UpdateBy]              INT                 NOT NULL
,[UpdateAt]              DATETIME2(3)        NOT NULL
,CONSTRAINT [PkInvoiceCommonSetting] PRIMARY KEY NONCLUSTERED
([CompanyId] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkInvoiceCommonSettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceCommonSetting]')
       )
ALTER TABLE [dbo].[InvoiceCommonSetting] ADD CONSTRAINT
 [FkInvoiceCommonSettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'CompanyId'
SET @v = N'請求合計０円を外す'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'ExcludeAmountZero'
SET @v = N'請求合計がマイナスになるものは外す'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'ExcludeMinusAmount'
SET @v = N'消し込み済みのデータは外す'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'ExcludeMatchedData'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceCommonSetting', N'COLUMN', N'UpdateAt'

END
GO

/* 請求書番号採番履歴 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[InvoiceNumberHistory]'))
BEGIN
CREATE TABLE [dbo].[InvoiceNumberHistory]
([Id]                INT                 NOT NULL    IDENTITY(1,1)
,[CompanyId]         INT                 NOT NULL
,[NumberingYear]     NVARCHAR(4)         NOT NULL
,[NumberingMonth]    NVARCHAR(2)         NOT NULL
,[FixedString]       nvarchar(100)       NOT NULL
,[LastNumber]        BIGINT              NOT NULL
,[CreateBy]          INT                 NOT NULL
,[CreateAt]          DATETIME2(3)        NOT NULL
,[UpdateBy]          INT                 NOT NULL
,[UpdateAt]          DATETIME2(3)        NOT NULL
,CONSTRAINT [PkInvoiceNumberHistory] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,CONSTRAINT [UqInvoiceNumberHistory] UNIQUE CLUSTERED 
([CompanyId] ASC
,[NumberingYear] ASC
,[NumberingMonth] ASC
,[FixedString] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkInvoiceNumberHistoryCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceNumberHistory]')
       )
ALTER TABLE [dbo].[InvoiceNumberHistory] ADD CONSTRAINT
 [FkInvoiceNumberHistoryCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

DECLARE @v sql_variant
SET @v = N'採番履歴ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'CompanyId'
SET @v = N'採番履歴年'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'NumberingYear'
SET @v = N'採番履歴月'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'NumberingMonth'
SET @v = N'固定文字列'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'FixedString'
SET @v = N'最終採番番号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'LastNumber'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'InvoiceNumberHistory', N'COLUMN', N'UpdateAt'

END
GO

/* 区分マスター 請求書発行対象外 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Category', N'COLUMN',N'ExcludeInvoicePublish'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'ExcludeInvoicePublish'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Category]')
 and name = N'ExcludeInvoicePublish' )
ALTER TABLE [dbo].[Category] ADD
 [ExcludeInvoicePublish]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Category', N'COLUMN',N'ExcludeInvoicePublish'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書発行対象外'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'ExcludeInvoicePublish'
GO


/* 出力項目設定・請求書発行 追加 */
INSERT INTO [dbo].[ExportFieldSettingBase]
([ExportFileType]
,[ColumnName]
,[Caption]
,[ColumnOrder]
,[AllowExport]
,[DataType])
SELECT u.ExportFileType [ExportFileType]
      ,u.ColumnName [ColumnName]
      ,u.Caption [Caption]
      ,u.ColumnOrder [ColumnOrder]
      ,u.AllowExport [AllowExport]
      ,u.DataType [DataType]
FROM(
          SELECT 2 [ExportFileType], 'CompanyCode'                  [ColumnName], '会社コード'              [Caption], 1  [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BillingInputId'               [ColumnName], '請求書ID'                [Caption], 2  [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BillingId'                    [ColumnName], '請求ID'                  [Caption], 3  [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BilledAt'                     [ColumnName], '請求日'                  [Caption], 4  [ColumnOrder], 1 [AllowExport], 1 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ClosingAt'                    [ColumnName], '請求締日'                [Caption], 5  [ColumnOrder], 1 [AllowExport], 1 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'SalesAt'                      [ColumnName], '売上日'                  [Caption], 6  [ColumnOrder], 1 [AllowExport], 1 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'DueAt'                        [ColumnName], '入金予定日'              [Caption], 7  [ColumnOrder], 1 [AllowExport], 1 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BillingAmount'                [ColumnName], '請求額'                  [Caption], 8  [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'TaxAmount'                    [ColumnName], '消費税'                  [Caption], 9  [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Price'                        [ColumnName], '金額（抜）'              [Caption], 10 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'RemainAmount'                 [ColumnName], '請求残'                  [Caption], 11 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'InvoiceCode'                  [ColumnName], '請求書番号'              [Caption], 12 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note1'                        [ColumnName], '請求備考1'               [Caption], 13 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note2'                        [ColumnName], '請求備考2'               [Caption], 14 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note3'                        [ColumnName], '請求備考3'               [Caption], 15 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note4'                        [ColumnName], '請求備考4'               [Caption], 16 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note5'                        [ColumnName], '請求備考5'               [Caption], 17 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note6'                        [ColumnName], '請求備考6'               [Caption], 18 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note7'                        [ColumnName], '請求備考7'               [Caption], 19 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Note8'                        [ColumnName], '請求備考8'               [Caption], 20 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Memo'                         [ColumnName], '請求メモ'                [Caption], 21 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ContractNumber'               [ColumnName], '契約番号'                [Caption], 22 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'Quantity'                     [ColumnName], '数量'                    [Caption], 23 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'UnitSymbol'                   [ColumnName], '単位'                    [Caption], 24 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'UnitPrice'                    [ColumnName], '単価'                    [Caption], 25 [ColumnOrder], 1 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'PublishAt'                    [ColumnName], '発行日'                  [Caption], 26 [ColumnOrder], 1 [AllowExport], 1 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'PublishAt1st'                 [ColumnName], '初回発行日'              [Caption], 27 [ColumnOrder], 1 [AllowExport], 1 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'AssignmentFlag'               [ColumnName], '消込フラグ'              [Caption], 28 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'DepartmentCode'               [ColumnName], '請求部門コード'          [Caption], 29 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'DepartmentName'               [ColumnName], '請求部門名'              [Caption], 30 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BillingCategoryCode'          [ColumnName], '請求区分コード'          [Caption], 31 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BillingCategoryName'          [ColumnName], '請求区分名'              [Caption], 32 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'BillingCategoryExternalCode'  [ColumnName], '請求区分外部コード'      [Caption], 33 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'TaxClassId'                   [ColumnName], '消費税属性'              [Caption], 34 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'TaxClassName'                 [ColumnName], '消費税属性名'            [Caption], 35 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CollectCategoryCode'          [ColumnName], '回収区分コード'          [Caption], 36 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CollectCategoryName'          [ColumnName], '回収区分名'              [Caption], 37 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CollectCategoryExternalCode'  [ColumnName], '回収区分外部コード'      [Caption], 38 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'StaffCode'                    [ColumnName], '担当者コード'            [Caption], 39 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'StaffName'                    [ColumnName], '担当者名'                [Caption], 40 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerCode'                 [ColumnName], '得意先コード'            [Caption], 41 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerName'                 [ColumnName], '得意先名'                [Caption], 42 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ShareTransferFee'             [ColumnName], '手数料負担区分'          [Caption], 43 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerPostalCode'           [ColumnName], '得意先郵便番号'          [Caption], 44 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerAddress1'             [ColumnName], '得意先住所1'             [Caption], 45 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerAddress2'             [ColumnName], '得意先住所2'             [Caption], 46 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerDepartmentName'       [ColumnName], '得意先部署'              [Caption], 47 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerAddressee'            [ColumnName], '得意先宛名'              [Caption], 48 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerHonorific'            [ColumnName], '得意先敬称'              [Caption], 49 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CustomerNote'                 [ColumnName], '得意先備考'              [Caption], 50 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ExclusiveBankCode'            [ColumnName], '専用銀行コード'          [Caption], 51 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ExclusiveBankName'            [ColumnName], '専用銀行名'              [Caption], 52 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ExclusiveBranchCode'          [ColumnName], '専用支店コード'          [Caption], 53 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'VirtualBranchCode'            [ColumnName], '仮想支店コード'          [Caption], 54 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ExclusiveBranchName'          [ColumnName], '仮想支店名'              [Caption], 55 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'VirtualAccountNumber'         [ColumnName], '仮想口座番号'            [Caption], 56 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'ExclusiveAccountTypeId'       [ColumnName], '専用口座種別'            [Caption], 57 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBankName1'             [ColumnName], '会社マスター銀行名1'     [Caption], 58 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBranchName1'           [ColumnName], '会社マスター支店名1'     [Caption], 59 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAccountType1'          [ColumnName], '会社マスター預金種別1'   [Caption], 60 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAccountNumber1'        [ColumnName], '会社マスター口座番号1'   [Caption], 61 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBankName2'             [ColumnName], '会社マスター銀行名2'     [Caption], 62 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBranchName2'           [ColumnName], '会社マスター支店名2'     [Caption], 63 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAccountType2'          [ColumnName], '会社マスター預金種別2'   [Caption], 64 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAccountNumber2'        [ColumnName], '会社マスター口座番号2'   [Caption], 65 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBankName3'             [ColumnName], '会社マスター銀行名3'     [Caption], 66 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBranchName3'           [ColumnName], '会社マスター支店名3'     [Caption], 67 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAccountType3'          [ColumnName], '会社マスター預金種別3'   [Caption], 68 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAccountNumber3'        [ColumnName], '会社マスター口座番号3'   [Caption], 69 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyBankAccountName'       [ColumnName], '会社マスター口座名義人'  [Caption], 70 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyName'                  [ColumnName], '会社マスター会社名'      [Caption], 71 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyPostalCode'            [ColumnName], '会社マスター郵便番号'    [Caption], 72 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAddress1'              [ColumnName], '会社マスター住所1'       [Caption], 73 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyAddress2'              [ColumnName], '会社マスター住所2'       [Caption], 74 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyTel'                   [ColumnName], '会社マスター電話番号'    [Caption], 75 [ColumnOrder], 0 [AllowExport], 0 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'CompanyFax'                   [ColumnName], '会社マスターFAX番号'     [Caption], 76 [ColumnOrder], 0 [AllowExport], 0 [DataType]
) u
WHERE NOT EXISTS (
 SELECT 1
   FROM [dbo].[ExportFieldSettingBase] b
  WHERE b.ExportFileType = u.ExportFileType
   AND  b.ColumnName     = u.ColumnName )
ORDER BY 
 u.ColumnOrder;

GO

/* 帳票設定ベース 請求書発行 帳票設定変更 */
DELETE FROM [dbo].[ReportSettingBase]
 WHERE ReportId IN (N'PC0401')

INSERT INTO [dbo].[ReportSettingBase]
     ( [ReportId]
     , [DisplayOrder]
     , [ItemId]
     , [ItemKey]
     , [Alias]
     , [IsText] )
select u.[rid]      [ReportId]
     , u.[dorder]   [DisplayOrder]
     , u.[iid]      [ItemId]
     , u.[ikey]     [ItemKey]
     , u.[alias]    [Alias]
     , u.[istxt]    [IsText]
  from (
            select N'PC0401' [rid],  1 [dorder], 0 [istxt], N'ReportInvoiceDate'    [iid], N'2' [ikey], N'請求日付'                         [alias]
 union all  select N'PC0401' [rid],  2 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'ロゴの表示'                       [alias]
 union all  select N'PC0401' [rid],  3 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'社判の表示'                       [alias]
 union all  select N'PC0401' [rid],  4 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'丸印の表示'                       [alias]
 union all  select N'PC0401' [rid],  5 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'住所の出力'                       [alias]
 union all  select N'PC0401' [rid],  6 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'承認欄の出力'                     [alias]
 union all  select N'PC0401' [rid],  7 [dorder], 0 [istxt], N'ReportInvoiceAmount'  [iid], N'0' [ikey], N'請求金額'                         [alias]
 union all  select N'PC0401' [rid],  8 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'明細に数量・単価・単位の表示'     [alias]
 union all  select N'PC0401' [rid],  9 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'売上日を取引日として明細に表示'   [alias]
 union all  select N'PC0401' [rid], 10 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'仮想口座を利用'                   [alias]
 union all  select N'PC0401' [rid], 11 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'本書と控えを出力'                 [alias]
 union all  select N'PC0401' [rid], 12 [dorder], 0 [istxt], N'ReportDoOrNot'        [iid], N'1' [ikey], N'再発行の際、「再発行」文面を表示' [alias]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[ReportSettingBase] rb
        WHERE rb.[ReportId]     = u.[rid]
          AND rb.[DisplayOrder] = u.[dorder] )
GO

/* 汎用設定項目定義　新規項目追加 */
INSERT INTO [dbo].[SettingDefinition]
     ( [ItemId]
     , [Description]
     , [ItemKeyLength]
     , [ItemValueLength] )
select u.[iid]             [ItemId]
     , u.[description]     [Description]
     , u.[iKeyLength]      [ItemKeyLength]
     , u.[iValueLength]    [ItemValueLength]
  from (
            select N'ReportInvoiceAmount'    [iid], N'請求金額'    [description], 1    [iKeyLength], 40    [iValueLength]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[SettingDefinition] sd
        WHERE sd.[ItemId]     = u.[iid] )
GO

/* 汎用設定項目 名称変更 作成日(システム日付)⇒発行日 */
UPDATE d
  SET d.[ItemValue] = N'発行日'
FROM [dbo].[Setting] d
  WHERE d.[ItemId]  = N'ReportInvoiceDate'
    AND d.[ItemKey] = 0
GO

/* 汎用設定項目 新規項目追加 */
INSERT INTO [dbo].[Setting]
     ( [ItemId]
     , [ItemKey]
     , [ItemValue] )
select u.[iid]       [ItemId]
     , u.[iKey]      [ItemKey]
     , u.[iValue]    [ItemValue]
  from (
            select N'ReportInvoiceAmount'    [iid], 0    [iKey], N'請求額'    [iValue]
union all   select N'ReportInvoiceAmount'    [iid], 1    [iKey], N'請求残'    [iValue]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
        WHERE s.[ItemId]     = u.[iid] )
GO

/* 会社ロゴ ロゴ種類 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'CompanyLogo', N'COLUMN',N'LogoType'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'LogoType'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[CompanyLogo]')
 and name = N'LogoType' )
ALTER TABLE [dbo].[CompanyLogo] ADD
 [LogoType]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'CompanyLogo', N'COLUMN',N'LogoType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ロゴ種類(0:会社ロゴ/1:社判/2:丸印)'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'LogoType'
GO

ALTER TABLE [dbo].[CompanyLogo] DROP CONSTRAINT [PkCompanyLogo]
GO

ALTER TABLE [dbo].[CompanyLogo] ADD CONSTRAINT [PkCompanyLogo] PRIMARY KEY CLUSTERED
([CompanyId] ASC
,[LogoType] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--BillingInputテーブルにカラム追加
--発行日
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'PublishAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'PublishAt'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[BillingInput]')
 and name = N'PublishAt' )
ALTER TABLE [dbo].[BillingInput] ADD
 [PublishAt] [datetime2](3)  NULL DEFAULT NULL
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'PublishAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'発行日'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'PublishAt'
GO
--初回発行日
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'PublishAt1st'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'PublishAt1st'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[BillingInput]')
 and name = N'PublishAt1st' )
ALTER TABLE [dbo].[BillingInput] ADD
 [PublishAt1st] [datetime2](3)  NULL DEFAULT NULL
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'PublishAt1st'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'初回発行日'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'PublishAt1st'
GO

--請求書文面パターンID
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'InvoiceTemplateId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'InvoiceTemplateId'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[BillingInput]')
 and name = N'InvoiceTemplateId' )
ALTER TABLE [dbo].[BillingInput] ADD
 [InvoiceTemplateId] [int]  NULL DEFAULT NULL
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'InvoiceTemplateId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書文面パターンID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'InvoiceTemplateId'
GO

/* 請求書ワークテーブル 新規追加 */
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkBillingInvoice]'))
BEGIN
    DROP TABLE [dbo].[WorkBillingInvoice];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkBillingInvoice]'))
BEGIN
    CREATE TABLE [dbo].[WorkBillingInvoice]
    ([ClientKey]               [varbinary](20)  NOT NULL
    ,[BillingId]               [bigint]         NOT NULL
    ,[TemporaryBillingInputId] [bigint]         NOT NULL
    ,[BillingInputId]          [bigint]             NULL
    ,CONSTRAINT [PkWorkBillingInvoice] PRIMARY KEY CLUSTERED
    ([ClientKey] ASC
    ,[BillingId] ASC ) )

    CREATE NONCLUSTERED INDEX [IdxWorkBillingInvoice] ON [dbo].[WorkBillingInvoice]
    ([TemporaryBillingInputId] ASC)

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingInvoice', @level2type=N'COLUMN',@level2name=N'ClientKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求ID'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingInvoice', @level2type=N'COLUMN',@level2name=N'BillingId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求入力仮ID'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingInvoice', @level2type=N'COLUMN',@level2name=N'TemporaryBillingInputId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求入力ID'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingInvoice', @level2type=N'COLUMN',@level2name=N'BillingInputId'
END

/* 請求備考追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Billing]')
 and name = N'Note5' )
BEGIN
    ALTER TABLE [dbo].[Billing] ADD
     [Note5]            NVARCHAR(100)                 NOT NULL DEFAULT N''
    ,[Note6]            NVARCHAR(100)                 NOT NULL DEFAULT N''
    ,[Note7]            NVARCHAR(100)                 NOT NULL DEFAULT N''
    ,[Note8]            NVARCHAR(100)                 NOT NULL DEFAULT N''

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考5'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note5'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考6'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note6'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考7'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note7'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考8'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note8'

    INSERT INTO [dbo].[ColumnNameSetting]
    SELECT u.*
         , NULL [Alias]
         , 0 [CreateBy]
         , GETDATE() [CreatAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM (
    SELECT cmp.Id [CompanyId]
         , N'Billing' [TableName]
         , u.[cn] [ColumnName]
         , u.[on] [OriginalName]
      FROM (
                SELECT N'Note5' [cn], N'備考5' [on]
    UNION ALL   SELECT N'Note6' [cn], N'備考6' [on]
    UNION ALL   SELECT N'Note7' [cn], N'備考7' [on]
    UNION ALL   SELECT N'Note8' [cn], N'備考8' [on] ) u
    INNER JOIN [dbo].[Company] cmp  ON cmp.[Id] = cmp.[Id] ) u
    WHERE NOT EXISTS (
          SELECT 1
            FROM [dbo].[ColumnNameSetting] cns
           WHERE cns.[CompanyId]    = u.[CompanyId]
             AND cns.[TableName]    = u.[TableName]
             AND cns.[ColumnName]   = u.[ColumnName] )
END
GO

/* 請求フリーインポーター 取込設定追加 */
IF (EXISTS (SELECT 1
              FROM [dbo].[ImporterSettingBase]
             WHERE [FormatId]       = 1
               AND [Sequence]       = 25
               AND [TargetColumn]   = N'CustomerName' ) )
BEGIN
    UPDATE d
       SET d.[Sequence] = d.[Sequence] + 4
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]  = 1
       AND b.[Sequence] >= 25

    UPDATE b
       SET b.[Sequence] = b.[Sequence] + 4
      FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]  = 1
       AND b.[Sequence] >= 25

    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 1 [FormatId]
         , u.[seq] [Sequence]
         , u.[fn]  [FieldName]
         , u.[tc]  [TargetColumn]
         , 1  [ImportDivision]
         , 3  [AttributeDivision]
      FROM (
                SELECT 25 [seq], N'備考5' [fn], N'Note5' [tc]
    UNION ALL   SELECT 26 [seq], N'備考6' [fn], N'Note6' [tc]
    UNION ALL   SELECT 27 [seq], N'備考7' [fn], N'Note7' [tc]
    UNION ALL   SELECT 28 [seq], N'備考8' [fn], N'Note8' [tc] ) u

    INSERT INTO [dbo].[ImporterSettingDetail]
    SELECT s.[Id] [ImporterSettingId]
         , b.[Sequence]
         , 0 [ImportDivision]
         , 0 [FieldIndex]
         , N'' [Caption]
         , 0 [AttributeDivision]
         , 0 [ItemPriority]
         , 0 [DoOverwrite]
         , 0 [IsUnique]
         , N'' [FixedValue]
         , 0 [UpdateKey]
         , 0 [CreateBy]
         , GETDATE() [CreateAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM [dbo].[ImporterSetting] s
     INNER JOIN [dbo].[ImporterSettingBase] b   ON b.[FormatId] = s.[FormatId]  AND s.[FormatId] = 1
     WHERE NOT EXISTS (
           SELECT 1
             FROM [dbo].[ImporterSettingDetail] d
            WHERE d.[ImporterSettingId]     = s.[Id]
              AND d.[Sequence]              = b.[Sequence] )
END
GO

/* 入金予定フリーインポーター 取込設定追加 */
IF (EXISTS (SELECT 1
              FROM [dbo].[ImporterSettingBase]
             WHERE [FormatId]       = 3
               AND [Sequence]       = 20
               AND [TargetColumn]   = N'CurrencyCode' ) )
BEGIN
    UPDATE d
       SET d.[Sequence] = d.[Sequence] + 4
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]  = 3
       AND b.[Sequence] >= 20

    UPDATE b
       SET b.[Sequence] = b.[Sequence] + 4
      FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]  = 3
       AND b.[Sequence] >= 20

    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 3 [FormatId]
         , u.[seq] [Sequence]
         , u.[fn]  [FieldName]
         , u.[tc]  [TargetColumn]
         , 11 [ImportDivision]
         , 3  [AttributeDivision]
      FROM (
                SELECT 20 [seq], N'備考5' [fn], N'Note5' [tc]
    UNION ALL   SELECT 21 [seq], N'備考6' [fn], N'Note6' [tc]
    UNION ALL   SELECT 22 [seq], N'備考7' [fn], N'Note7' [tc]
    UNION ALL   SELECT 23 [seq], N'備考8' [fn], N'Note8' [tc] ) u

    INSERT INTO [dbo].[ImporterSettingDetail]
    SELECT s.[Id] [ImporterSettingId]
         , b.[Sequence]
         , 0 [ImportDivision]
         , 0 [FieldIndex]
         , N'' [Caption]
         , 0 [AttributeDivision]
         , 0 [ItemPriority]
         , 0 [DoOverwrite]
         , 0 [IsUnique]
         , N'' [FixedValue]
         , 0 [UpdateKey]
         , 0 [CreateBy]
         , GETDATE() [CreateAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM [dbo].[ImporterSetting] s
     INNER JOIN [dbo].[ImporterSettingBase] b   ON b.[FormatId] = s.[FormatId]  AND s.[FormatId] = 3
     WHERE NOT EXISTS (
           SELECT 1
             FROM [dbo].[ImporterSettingDetail] d
            WHERE d.[ImporterSettingId]     = s.[Id]
              AND d.[Sequence]              = b.[Sequence] )
END
GO

/* グリッド表示設定 項目追加 */
INSERT INTO [dbo].[GridSettingBase]
SELECT u.*
    FROM (
            SELECT 1 [id], N'Note5' [name], N'備考5' [jp], 39 [od], 0 [wd]
UNION ALL   SELECT 1 [id], N'Note6' [name], N'備考6' [jp], 40 [od], 0 [wd]
UNION ALL   SELECT 1 [id], N'Note7' [name], N'備考7' [jp], 41 [od], 0 [wd]
UNION ALL   SELECT 1 [id], N'Note8' [name], N'備考8' [jp], 42 [od], 0 [wd]
UNION ALL   SELECT 3 [id], N'Note5' [name], N'備考5' [jp], 22 [od], 0 [wd]
UNION ALL   SELECT 3 [id], N'Note6' [name], N'備考6' [jp], 23 [od], 0 [wd]
UNION ALL   SELECT 3 [id], N'Note7' [name], N'備考7' [jp], 24 [od], 0 [wd]
UNION ALL   SELECT 3 [id], N'Note8' [name], N'備考8' [jp], 25 [od], 0 [wd]
        ) u
WHERE NOT EXISTS (
        SELECT *
        FROM [dbo].[GridSettingBase] b
        WHERE b.[GridId]     = u.[id]
            AND b.[ColumnName] = u.[name] )

/* インポート用 ユーザー定義テーブル型修正 */
IF (NOT EXISTS (
select 1
  from sys.table_types tt
 inner join sys.columns c 
    on tt.user_type_id = type_id(N'BillingImportDuplication')
   and c.object_id = tt.type_table_object_id
   and c.name        = N'Note5' ) )
BEGIN
    DROP TYPE [dbo].[BillingImportDuplication];

    CREATE TYPE [dbo].[BillingImportDuplication] AS TABLE
    (
        [RowNumber]             int           NOT NULL
      , [CustomerId]            int               NULL
      , [BilledAt]              date              NULL
      , [BillingAmount]         numeric(18,5)     NULL
      , [TaxAmount]             numeric(18,5)     NULL
      , [DueAt]                 date              NULL
      , [DepartmentId]          int               NULL
      , [DebitAccountTitleId]   int               NULL
      , [SalesAt]               date              NULL
      , [InvoiceCode]           nvarchar(100)     NULL
      , [ClosingAt]             date              NULL
      , [StaffId]               int               NULL
      , [Note1]                 nvarchar(100)     NULL
      , [BillingCategoryId]     int               NULL
      , [CollectCategoryId]     int               NULL
      , [Price]                 numeric(18,5)     NULL
      , [TaxClassId]            int               NULL
      , [Note2]                 nvarchar(100)     NULL
      , [Note3]                 nvarchar(100)     NULL
      , [Note4]                 nvarchar(100)     NULL
      , [Note5]                 nvarchar(100)     NULL
      , [Note6]                 nvarchar(100)     NULL
      , [Note7]                 nvarchar(100)     NULL
      , [Note8]                 nvarchar(100)     NULL
      , [CurrencyId]            int               NULL
    ,   PRIMARY KEY ([RowNumber])
    );
END
GO

/* 得意先マスター 請求書発行対象外 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'ExcludeInvoicePublish'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExcludeInvoicePublish'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Customer]')
 and name = N'ExcludeInvoicePublish' )
ALTER TABLE [dbo].[Customer] ADD
 [ExcludeInvoicePublish]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'ExcludeInvoicePublish'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書発行対象外'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExcludeInvoicePublish'
GO

/* 得意先マスター 請求書発行対象外　項目追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]     = 60
       AND b.[FieldName]    = N'請求書発行対象外' )
BEGIN
    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 4 [FormatId], u.[seq], u.[fname], u.[target], u.[idev], u.[adev]
      FROM (
                SELECT 60 [seq], 12 [idev], 0 [adev], N'ExcludeInvoicePublish' [target], N'請求書発行対象外' [fname]
           ) u
     WHERE NOT EXISTS (
           SELECT *
             FROM [dbo].[ImporterSettingBase] b
            WHERE b.[FormatId]      = 4
              AND b.[Sequence]      = u.[seq] )

    INSERT INTO [dbo].[ImporterSettingDetail]
    SELECT s.[Id] [ImporterSettingId]
         , b.[Sequence]
         , 0 [ImportDivision]
         , 0 [FieldIndex]
         , N'' [Caption]
         , 0 [AttributeDivision]
         , 0 [ItemPriority]
         , 0 [DoOVerwrite]
         , 0 [IsUnique]
         , N'' [FixedValue]
         , 0 [UpdateKey]
         , 0 [CreateBy]
         , GETDATE() [CreateAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM [dbo].[ImporterSetting] s
     INNER JOIN [dbo].[ImporterSettingBase] b   ON b.[FormatId] = s.[FormatId]  AND s.[FormatId] = 4
     WHERE NOT EXISTS (
           SELECT 1
             FROM [dbo].[ImporterSettingDetail] d
            WHERE d.[ImporterSettingId]     = s.[Id]
              AND d.[Sequence]              = b.[Sequence] )

END
GO

/* 消費税属性マスター 新規項目追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[TaxClass] tc
     WHERE tc.[Id]     = 0
       AND tc.[Name]   = N'外税課税' )
BEGIN
    INSERT INTO [dbo].[TaxClass]
     ( [Id]
     , [Name] )
    select u.[iid]       [Id]
         , u.[iName]     [Name]
    from (
            select 0    [iid], N'外税課税'    [iName]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[TaxClass] t
       WHERE t.[Id]     = u.[iid] )

/* 消費税属性マスター 名称変更 課税⇒外税課税 */
UPDATE d
  SET d.[Name] = N'内税課税'
FROM [dbo].[TaxClass] d
  WHERE d.[Id]    = 1
    AND d.[Name]  = N'課税'

END
GO

/* 汎用設定項目 取込設定追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[Setting] s
     WHERE s.[ItemId]      = N'TRKM17'
       AND s.[ItemKey]    IN (3,4) )
BEGIN
INSERT INTO [dbo].[Setting]
     ( [ItemId]
     , [ItemKey]
     , [ItemValue] )
select u.[iid]       [ItemId]
     , u.[iKey]      [ItemKey]
     , u.[iValue]    [ItemValue]
  from (
            select N'TRKM17'    [iid], 3    [iKey], N'3：取込有(請求金額＋税額 の0円データはエラーにする)'    [iValue]
union all   select N'TRKM17'    [iid], 4    [iKey], N'4：取込有(請求金額＋税額 の0円データは取込まない)'      [iValue]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
        WHERE s.[ItemId]     = u.[iid] 
         AND  s.[ItemKey]    = u.iKey )

END
GO

/* 定期請求設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[PeriodicBillingSetting]'))
BEGIN
CREATE TABLE [dbo].[PeriodicBillingSetting]
([Id]                       BIGINT              NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[Name]                     NVARCHAR(40)        NOT NULL
,[CurrencyId]               INT                 NOT NULL
,[CustomerId]               INT                 NOT NULL
,[DestinationId]            INT                     NULL
,[DepartmentId]             INT                 NOT NULL
,[StaffId]                  INT                 NOT NULL
,[CollectCategoryId]        INT                 NOT NULL
,[BilledCycle]              INT                 NOT NULL
,[BilledDay]                INT                 NOT NULL
,[StartMonth]               DATE                NOT NULL
,[EndMonth]                 DATE                    NULL
,[InvoiceCode]              NVARCHAR(100)       NOT NULL
,[SetBillingNote1]          INT                 NOT NULL
,[SetBillingNote2]          INT                 NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkPeriodicBillingSetting] PRIMARY KEY NONCLUSTERED
([Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingCurrency]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingCurrency] FOREIGN KEY
([CurrencyId]) REFERENCES [dbo].[Currency] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingCustomer]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingCustomer] FOREIGN KEY
([CustomerId]) REFERENCES [dbo].[Customer] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingDestination]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingDestination] FOREIGN KEY
([DestinationId]) REFERENCES [dbo].[Destination] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingDepartment]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingDepartment] FOREIGN KEY
([DepartmentId]) REFERENCES [dbo].[Department] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingStaff]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingStaff] FOREIGN KEY
([StaffId]) REFERENCES [dbo].[Staff] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingCategory]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSetting]')
       )
ALTER TABLE [dbo].[PeriodicBillingSetting] ADD CONSTRAINT
 [FkPeriodicBillingSettingCategory] FOREIGN KEY
([CollectCategoryId]) REFERENCES [dbo].[Category] ([Id])

DECLARE @v sql_variant
SET @v = N'定期請求設定ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'CompanyId'
SET @v = N'パターン名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'Name'
SET @v = N'通貨ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'CurrencyId'
SET @v = N'得意先ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'CustomerId'
SET @v = N'送付先ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'DestinationId'
SET @v = N'請求部門ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'DepartmentId'
SET @v = N'営業担当者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'StaffId'
SET @v = N'回収区分ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'CollectCategoryId'
SET @v = N'請求サイクル'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'BilledCycle'
SET @v = N'請求日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'BilledDay'
SET @v = N'開始月'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'StartMonth'
SET @v = N'終了月'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'EndMonth'
SET @v = N'請求書番号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'InvoiceCode'
SET @v = N'備考1'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'SetBillingNote1'
SET @v = N'備考2'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'SetBillingNote2'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSetting', N'COLUMN', N'UpdateAt'

END
GO

/* 定期請求明細設定 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[PeriodicBillingSettingDetail]'))
BEGIN
CREATE TABLE [dbo].[PeriodicBillingSettingDetail]
([PeriodicBillingSettingId] BIGINT              NOT NULL
,[DisplayOrder]             INT                 NOT NULL
,[BillingCategoryId]        INT                 NOT NULL
,[TaxClassId]               INT                 NOT NULL
,[DebitAccountTitleId]      INT                     NULL
,[Quantity]                 NUMERIC(7,2)            NULL
,[UnitSymbol]               NVARCHAR(3)             NULL
,[UnitPrice]                NUMERIC(18,5)           NULL
,[Price]                    NUMERIC(18,5)           NULL
,[TaxAmount]                NUMERIC(18,5)       NOT NULL
,[BillingAmount]            NUMERIC(18,5)       NOT NULL
,[Note1]                    NVARCHAR(100)       NOT NULL
,[Note2]                    NVARCHAR(100)       NOT NULL
,[Note3]                    NVARCHAR(100)       NOT NULL
,[Note4]                    NVARCHAR(100)       NOT NULL
,[Note5]                    NVARCHAR(100)       NOT NULL
,[Note6]                    NVARCHAR(100)       NOT NULL
,[Note7]                    NVARCHAR(100)       NOT NULL
,[Note8]                    NVARCHAR(100)       NOT NULL
,[Memo]                     NVARCHAR(MAX)       NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,[UpdateBy]                 INT                 NOT NULL
,[UpdateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkPeriodicBillingSettingDetail] PRIMARY KEY CLUSTERED
([PeriodicBillingSettingId] ASC 
,[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingDetailPeriodicBillingSetting]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSettingDetail]')
       )
ALTER TABLE [dbo].[PeriodicBillingSettingDetail] ADD CONSTRAINT
 [FkPeriodicBillingSettingDetailPeriodicBillingSetting] FOREIGN KEY
([PeriodicBillingSettingId]) REFERENCES [dbo].[PeriodicBillingSetting] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingDetailCategory]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSettingDetail]')
       )
ALTER TABLE [dbo].[PeriodicBillingSettingDetail] ADD CONSTRAINT
 [FkPeriodicBillingSettingDetailCategory] FOREIGN KEY
([BillingCategoryId]) REFERENCES [dbo].[Category] ([Id])

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingSettingDetailDebitAccountTitle]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingSettingDetail]')
       )
ALTER TABLE [dbo].[PeriodicBillingSettingDetail] ADD CONSTRAINT
 [FkPeriodicBillingSettingDetailDebitAccountTitle] FOREIGN KEY
([DebitAccountTitleId]) REFERENCES [dbo].[AccountTitle] ([Id])


DECLARE @v sql_variant
SET @v = N'定期請求設定ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'PeriodicBillingSettingId'
SET @v = N'表示順'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'DisplayOrder'
SET @v = N'請求区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'BillingCategoryId'
SET @v = N'税区'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'TaxClassId'
SET @v = N'債権科目'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'DebitAccountTitleId'
SET @v = N'数量'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Quantity'
SET @v = N'単位'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'UnitSymbol'
SET @v = N'単価'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'UnitPrice'
SET @v = N'金額（抜）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Price'
SET @v = N'消費税'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'TaxAmount'
SET @v = N'請求額'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'BillingAmount'
SET @v = N'備考1'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note1'
SET @v = N'備考2'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note2'
SET @v = N'備考3'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note3'
SET @v = N'備考4'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note4'
SET @v = N'備考5'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note5'
SET @v = N'備考6'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note6'
SET @v = N'備考7'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note7'
SET @v = N'備考8'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Note8'
SET @v = N'メモ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'Memo'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingSettingDetail', N'COLUMN', N'UpdateAt'

END
GO

/* 定期請求作成済 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[PeriodicBillingCreated]'))
BEGIN
CREATE TABLE [dbo].[PeriodicBillingCreated]
([Id]                           INT                 NOT NULL    IDENTITY(1,1)
,[PeriodicBillingSettingId]     BIGINT              NOT NULL
,[CreateYearMonth]              DATE                NOT NULL
,[CreateBy]                     INT                 NOT NULL
,[CreateAt]                     DATETIME2(3)        NOT NULL
,[UpdateBy]                     INT                 NOT NULL
,[UpdateAt]                     DATETIME2(3)        NOT NULL
,CONSTRAINT [PkPeriodicBillingCreated] PRIMARY KEY NONCLUSTERED
([Id] ASC  )
,CONSTRAINT [UqPeriodicBillingCreated] UNIQUE CLUSTERED 
([PeriodicBillingSettingId] ASC
,[CreateYearMonth]      ASC )
)

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkPeriodicBillingCreatedPeriodicBillingSetting]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[PeriodicBillingCreated]')
       )
ALTER TABLE [dbo].[PeriodicBillingCreated] ADD CONSTRAINT
 [FkPeriodicBillingCreatedPeriodicBillingSetting] FOREIGN KEY
([PeriodicBillingSettingId]) REFERENCES [dbo].[PeriodicBillingSetting] ([Id]) ON DELETE CASCADE

DECLARE @v sql_variant
SET @v = N'定期請求作成済ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'Id'
SET @v = N'定期請求設定ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'PeriodicBillingSettingId'
SET @v = N'作成年月'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'CreateYearMonth'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PeriodicBillingCreated', N'COLUMN', N'UpdateAt'

END
GO


/* BillingInput 自動採番利用フラグ カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'UseInvoiceCodeNumbering'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'UseInvoiceCodeNumbering'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[BillingInput]')
 and name = N'UseInvoiceCodeNumbering' )
ALTER TABLE [dbo].BillingInput ADD
 [UseInvoiceCodeNumbering]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BillingInput', N'COLUMN',N'UseInvoiceCodeNumbering'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自動採番利用フラグ'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'UseInvoiceCodeNumbering'
GO

/* エクスポート設定 */
INSERT INTO [dbo].[ExportFieldSettingBase]
([ExportFileType]
,[ColumnName]
,[Caption]
,[ColumnOrder]
,[AllowExport]
,[DataType])
SELECT u.[ExportFileType]
      ,u.[ColumnName]
      ,u.[Caption]
      ,u.[ColumnOrder]
      ,u.[AllowExport]
      ,u.[DataType]
FROM(
          SELECT 1 [ExportFileType], 'RequireHeader' [ColumnName], '項目名を出力する' [Caption], 0  [ColumnOrder], 0 [AllowExport], 2 [DataType]
UNION ALL SELECT 2 [ExportFileType], 'RequireHeader' [ColumnName], '項目名を出力する' [Caption], 0  [ColumnOrder], 0 [AllowExport], 2 [DataType]
) u
WHERE NOT EXISTS (
 SELECT 1
   FROM [dbo].[ExportFieldSettingBase] b
  WHERE b.ExportFileType = u.ExportFileType
   AND  b.ColumnName     = u.ColumnName )
GO

/* 照合設定 消費税計算関連 追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[CollationSetting]')
 and name = N'CalculateTaxByInputId' )
BEGIN
    ALTER TABLE [dbo].[CollationSetting]
     ADD [CalculateTaxByInputId] INT NOT NULL CONSTRAINT DfCollationSettingCalculateTaxByInputId DEFAULT 0;

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書単位で消費税計算を行う'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'CalculateTaxByInputId'
END
GO

/* 出力項目設定・請求書発行 追加 */
INSERT INTO [dbo].[GridSettingBase]
([GridId]
,[ColumnName]
,[ColumnNameJp]
,[DisplayOrder]
,[DisplayWidth])
SELECT 6 AS [GridId]
     , u.[ColumnName]
     , u.[ColumnNameJp]
     , u.[DisplayOrder]
     , u.[DisplayWidth]
FROM(
          SELECT 6 [GridId], 'Checked'                    [ColumnName], '選択'           [ColumnNameJp], 1  [DisplayOrder], 50  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'InvoiceTemplateId'          [ColumnName], '文面'           [ColumnNameJp], 2  [DisplayOrder], 150 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'InvoiceCode'                [ColumnName], '請求書番号'     [ColumnNameJp], 3  [DisplayOrder], 100 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'DetailsCount'               [ColumnName], '明細件数'       [ColumnNameJp], 4  [DisplayOrder], 75  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'CustomerCode'               [ColumnName], '得意先コード'   [ColumnNameJp], 5  [DisplayOrder], 85  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'CustomerName'               [ColumnName], '得意先名'       [ColumnNameJp], 6  [DisplayOrder], 200 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'AmountSum'                  [ColumnName], '請求額'         [ColumnNameJp], 7  [DisplayOrder], 100 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'RemainAmountSum'            [ColumnName], '請求残'         [ColumnNameJp], 8  [DisplayOrder], 100 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'CollectCategoryCodeAndNeme' [ColumnName], '回収区分'       [ColumnNameJp], 9  [DisplayOrder], 70  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'ClosingAt'                  [ColumnName], '請求締日'       [ColumnNameJp], 10 [DisplayOrder], 75  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'BilledAt'                   [ColumnName], '請求日'         [ColumnNameJp], 11 [DisplayOrder], 75  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'DepartmentCode'             [ColumnName], '請求部門コード' [ColumnNameJp], 12 [DisplayOrder], 80  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'DepartmentName'             [ColumnName], '請求部門名'     [ColumnNameJp], 13 [DisplayOrder], 100 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'StaffCode'                  [ColumnName], '担当者コード'   [ColumnNameJp], 14 [DisplayOrder], 80  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'StaffName'                  [ColumnName], '担当者名'       [ColumnNameJp], 15 [DisplayOrder], 100 [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'DestnationCode'             [ColumnName], '送付先コード'   [ColumnNameJp], 16 [DisplayOrder], 70  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'PublishAt'                  [ColumnName], '発行日'         [ColumnNameJp], 17 [DisplayOrder], 75  [DisplayWidth]
UNION ALL SELECT 6 [GridId], 'PublishAt1st'               [ColumnName], '初回発行日'     [ColumnNameJp], 18 [DisplayOrder], 75  [DisplayWidth]
) u
WHERE NOT EXISTS (
 SELECT 1
   FROM [dbo].[GridSettingBase] b
  WHERE b.[GridId]     = u.[GridId]
   AND  b.[ColumnName] = u.[ColumnName] )

--督促管理関連
IF NOT EXISTS (SELECT * FROM sys.columns where object_id = object_id(N'[dbo].[Billing]') and name = N'ReminderId' )
ALTER TABLE [dbo].[Billing] ADD [ReminderId] INT NULL
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[Reminder]'))
BEGIN
    CREATE TABLE [dbo].[Reminder]
    ([Id]                       INT                 NOT NULL    IDENTITY(1,1)
    ,[CompanyId]                INT                 NOT NULL
    ,[CurrencyId]               INT                 NOT NULL
    ,[CustomerId]               INT                 NOT NULL
    ,[CalculateBaseDate]        DATE                NOT NULL
    ,[StatusId]                 INT                 NOT NULL
    ,[Memo]                     NVARCHAR(100)       NOT NULL
    ,[OutputAt]                 DATETIME2(3)            NULL
    ,CONSTRAINT [PkReminder] PRIMARY KEY NONCLUSTERED
    ([Id] ASC 
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    DECLARE @v sql_variant
    SET @v = N'会社督促データID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'Id'
    SET @v = N'会社ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'CompanyId'
    SET @v = N'得意先ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'CustomerId'
    SET @v = N'入金基準日'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'CalculateBaseDate'
    SET @v = N'ステータスID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'StatusId'
    SET @v = N'対応記録'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'Memo'
    SET @v = N'発行日'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Reminder', N'COLUMN', N'OutputAt'
END
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Reminder]')
       )
ALTER TABLE [dbo].[Reminder] ADD CONSTRAINT
 [FkReminderCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderStatus]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Reminder]')
       )
ALTER TABLE [dbo].[Reminder] ADD CONSTRAINT
 [FkReminderStatus] FOREIGN KEY
([StatusId]) REFERENCES [dbo].[StatusMaster] ([Id])
ON DELETE CASCADE

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkBillingReminder]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Billing]')
       )
ALTER TABLE [dbo].[Billing] ADD CONSTRAINT
 [FkBillingReminder] FOREIGN KEY
([ReminderId]) REFERENCES [dbo].[Reminder] ([Id])


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderHistory]'))
BEGIN
    CREATE TABLE [dbo].[ReminderHistory]
    ([Id]                       INT                 NOT NULL    IDENTITY(1,1)
    ,[ReminderId]               INT                 NOT NULL
    ,[StatusId]                 INT                 NOT NULL
    ,[Memo]                     NVARCHAR(100)       NOT NULL
    ,[OutputFlag]               INT                 NOT NULL
    ,[InputType]                INT                 NOT NULL
    ,[ReminderAmount]           NUMERIC(18, 5)      NOT NULL
    ,[CreateBy]                 INT                 NOT NULL
    ,[CreateAt]                 DATETIME2(3)        NOT NULL
    ,CONSTRAINT [PkReminderHistory] PRIMARY KEY NONCLUSTERED
    ([Id] ASC 
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    DECLARE @v sql_variant
    SET @v = N'督促履歴ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'Id'
    SET @v = N'督促データID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'ReminderId'
    SET @v = N'ステータスID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'StatusId'
    SET @v = N'対応記録'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'Memo'
    SET @v = N'督促状発行フラグ'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'OutputFlag'
    SET @v = N'入力区分'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'InputType'
    SET @v = N'滞留金額'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'ReminderAmount'
    SET @v = N'登録者'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'CreateBy'
    SET @v = N'登録日時'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderHistory', N'COLUMN', N'CreateAt'
END
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderHistoryReminder]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderHistory]')
       )
ALTER TABLE [dbo].[ReminderHistory] ADD CONSTRAINT
 [FkReminderHistoryReminder] FOREIGN KEY
([ReminderId]) REFERENCES [dbo].[Reminder] ([Id])
ON DELETE CASCADE


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderOutputed]'))
BEGIN
    CREATE TABLE [dbo].[ReminderOutputed]
    ([OutputNo]                 INT                 NOT NULL
    ,[BillingId]                BIGINT                 NOT NULL
    ,[RemainAmount]             NUMERIC(18,5)       NOT NULL
    ,[ReminderTemplateId]       INT                 NOT NULL
    ,[OutputAt]                 DATETIME2(3)        NOT NULL
    ,CONSTRAINT [PkReminderOutputed] PRIMARY KEY NONCLUSTERED
    ([OutputNo] ASC,
     [BillingId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    DECLARE @v sql_variant
    SET @v = N'発行番号'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderOutputed', N'COLUMN', N'OutputNo'
    SET @v = N'請求データID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderOutputed', N'COLUMN', N'BillingId'
    SET @v = N'滞留金額'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderOutputed', N'COLUMN', N'RemainAmount'
    SET @v = N'督促状パターンID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderOutputed', N'COLUMN', N'ReminderTemplateId'
    SET @v = N'発行日'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderOutputed', N'COLUMN', N'OutputAt'
END
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderOutputedBilling]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderOutputed]')
       )
ALTER TABLE [dbo].[ReminderOutputed] ADD CONSTRAINT
 [FkReminderOutputedBilling] FOREIGN KEY
([BillingId]) REFERENCES [dbo].[Billing] ([Id])


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderSummary]'))
BEGIN
    CREATE TABLE [dbo].[ReminderSummary]
    ([Id]                       INT                 NOT NULL    IDENTITY(1,1)
    ,[CustomerId]               INT                 NOT NULL
    ,[CurrencyId]               INT                 NOT NULL
    ,[Memo]                     NVARCHAR(100)       NOT NULL
    ,CONSTRAINT [PkReminderSummary] PRIMARY KEY NONCLUSTERED
    ([Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    DECLARE @v sql_variant
    SET @v = N'督促集計ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummary', N'COLUMN', N'Id'
    SET @v = N'得意先ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummary', N'COLUMN', N'CustomerId'
    SET @v = N'通貨ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummary', N'COLUMN', N'CurrencyId'
    SET @v = N'対応記録'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummary', N'COLUMN', N'Memo'
END
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderSummaryCustomer]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderSummary]')
       )
ALTER TABLE [dbo].[ReminderSummary] ADD CONSTRAINT
 [FkReminderSummaryCustomer] FOREIGN KEY
([CustomerId]) REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderSummaryCurrency]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderSummary]')
       )
ALTER TABLE [dbo].[ReminderSummary] ADD CONSTRAINT
 [FkReminderSummaryCurrency] FOREIGN KEY
([CurrencyId]) REFERENCES [dbo].[Currency] ([Id])
ON DELETE CASCADE


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ReminderSummaryHistory]'))
BEGIN
    CREATE TABLE [dbo].[ReminderSummaryHistory]
    ([Id]                       INT                 NOT NULL    IDENTITY(1,1)
    ,[ReminderSummaryId]        INT                 NOT NULL
    ,[Memo]                     NVARCHAR(100)       NOT NULL
    ,[InputType]                INT                 NOT NULL
    ,[ReminderAmount]           NUMERIC(18,5)       NOT NULL
    ,[CreateBy]                 INT                 NOT NULL
    ,[CreateAt]                 DATETIME2(3)        NOT NULL
    ,CONSTRAINT [PkReminderSummaryHistory] PRIMARY KEY NONCLUSTERED
    ([Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    DECLARE @v sql_variant
    SET @v = N'督促集計履歴ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'Id'
    SET @v = N'督促集計ID'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'ReminderSummaryId'
    SET @v = N'対応記録'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'Memo'
    SET @v = N'入力区分'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'InputType'
    SET @v = N'滞留金額'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'ReminderAmount'
    SET @v = N'登録者'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'CreateBy'
    SET @v = N'登録日時'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ReminderSummaryHistory', N'COLUMN', N'CreateAt'
END
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkReminderSummaryHistoryReminderSummary]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderSummaryHistory]')
       )
ALTER TABLE [dbo].[ReminderSummaryHistory] ADD CONSTRAINT
 [FkReminderSummaryHistoryReminderSummary] FOREIGN KEY
([ReminderSummaryId]) REFERENCES [dbo].[ReminderSummary] ([Id])
ON DELETE CASCADE
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 WHERE object_id = object_id(N'[dbo].[CollationSetting]')
 AND name = N'SetSystemDateToCreateAtFilter' )
BEGIN
    ALTER TABLE [dbo].[CollationSetting] ADD
     [SetSystemDateToCreateAtFilter]    int NOT NULL DEFAULT 0

    DECLARE @v sql_variant
    SET @v = N'消込済データ表示時、消込日時にシステム日時を設定'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CollationSetting', N'COLUMN', N'SetSystemDateToCreateAtFilter'
END
GO

/* 画面テーブル スキーマ変更 table の column での制御から Web Service 内での制御に修正 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'IsStandard'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IsStandard'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseScheduledPayment'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseScheduledPayment'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseReceiptSection'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseReceiptSection'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseAuthorization'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseAuthorization'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseLongTermAdvanceReceived'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseLongTermAdvanceReceived'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseCashOnDueDates'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseCashOnDueDates'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseDiscount'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseDiscount'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseForeignCurrency'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseForeignCurrency'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'NotUseForeignCurrency'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'NotUseForeignCurrency'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseBillingFilter'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseBillingFilter'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseDistribution'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseDistribution'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UseOperationLogging'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseOperationLogging'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UsePublishInvoice'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UsePublishInvoice'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'IsStandard' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  IsStandard
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseScheduledPayment' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseScheduledPayment
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseReceiptSection' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseReceiptSection
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseAuthorization' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseAuthorization
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseLongTermAdvanceReceived' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseLongTermAdvanceReceived
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseCashOnDueDates' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseCashOnDueDates
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseDiscount' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseDiscount
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseForeignCurrency' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseForeignCurrency
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'NotUseForeignCurrency' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  NotUseForeignCurrency
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseBillingFilter' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseBillingFilter
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseDistribution' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseDistribution
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UseOperationLogging' )
ALTER TABLE [dbo].[Menu] DROP COLUMN  UseOperationLogging
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[Menu]') AND name      = N'UsePublishInvoice' )
BEGIN
    DECLARE @query NVARCHAR(MAX)
    SET @query = N'ALTER TABLE [dbo].[Menu] DROP CONSTRAINT ['
    +  (SELECT TOP 1 df.name FROM sys.default_constraints df
      INNER JOIN sys.columns c ON df.parent_object_id = c.object_id AND df.parent_column_id = c.column_id
      WHERE df.parent_object_id = object_id(N'[dbo].[Menu]') and c.name = N'UsePublishInvoice')
    + N']'
    EXECUTE sp_executesql @query;
    ALTER TABLE [dbo].[Menu] DROP COLUMN  [UsePublishInvoice]
END
GO

INSERT INTO [dbo].[Menu]
     ( [MenuId]
     , [MenuName]
     , [MenuCategory]
     , [Sequence]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt] )
SELECT u.Id             [MenuId]
     , u.Name           [MenuName]
     , u.Category       [MenuCategory]
     , u.[Sequence]
     , 0                [CreateBy]
     , getdate()        [CreateAt]
     , 0                [UpdateBy]
     , getdate()        [UpdateAt]
  FROM (
            SELECT N'PH1101' [Id], N'PH' [Category], 6110 [Sequence], N'働くDB WebAPI 連携設定'       [Name]
 UNION ALL  SELECT N'PC1301' [Id], N'PC' [Category], 1130 [Sequence], N'働くDB 請求データ抽出'        [Name]
 UNION ALL  SELECT N'PE0801' [Id], N'PE' [Category], 3080 [Sequence], N'働くDB 消込結果連携'          [Name]
 UNION ALL  SELECT N'PB2201' [Id], N'PB' [Category],  230 [Sequence], N'送付先マスター'               [Name]
 UNION ALL  SELECT N'PB2301' [Id], N'PB' [Category],  240 [Sequence], N'ステータスマスター'           [Name]
 UNION ALL  SELECT N'PC0401' [Id], N'PC' [Category], 1040 [Sequence], N'請求書発行'                   [Name]
 UNION ALL  SELECT N'PC1401' [Id], N'PC' [Category], 1041 [Sequence], N'請求書再発行'                 [Name]
 UNION ALL  SELECT N'PC1501' [Id], N'PC' [Category], 1900 [Sequence], N'請求書設定'                   [Name]
 UNION ALL  SELECT N'PH1201' [Id], N'PH' [Category], 6120 [Sequence], N'PCA会計DX WebAPI 連携設定'    [Name]
 UNION ALL  SELECT N'PE0901' [Id], N'PE' [Category], 3090 [Sequence], N'PCA会計DX 消込結果連携'       [Name]
 UNION ALL  SELECT N'PC1601' [Id], N'PC' [Category], 1140 [Sequence], N'定期請求パターンマスター'     [Name]
 UNION ALL  SELECT N'PC1701' [Id], N'PC' [Category], 1150 [Sequence], N'定期請求データ登録'           [Name]
 UNION ALL  SELECT N'PI0101' [Id], N'PI' [Category], 7010 [Sequence], N'督促データ確定'               [Name]
 UNION ALL  SELECT N'PI0201' [Id], N'PI' [Category], 7020 [Sequence], N'督促データ管理'               [Name]
 --UNION ALL  SELECT N'PI0301' [Id], N'PI' [Category], 7030 [Sequence], N'督促管理帳票'                 [Name]
 UNION ALL  SELECT N'PI0401' [Id], N'PI' [Category], 7040 [Sequence], N'督促設定'                     [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Menu] m
        WHERE m.MenuId      = u.Id )
 ORDER BY
       u.Id;
GO

/* 請求書発行 画面名称変更 */
UPDATE Menu SET MenuName = '請求書発行' WHERE MenuId = 'PC0401'
GO


   /*
 既存のメニュー権限に追加 メニュー追加後、一度流せば良いので、修正クエリの最下部に配置すること
 メニューを利用したい場合は、各種設定＆セキュリティ 画面で別途有効化すること
 */
INSERT INTO [dbo].[MenuAuthority]
     ( [CompanyId]
     , [MenuId]
     , [AuthorityLevel]
     , [Available]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt])
SELECT cmp.[Id] [CompanyId]
     , m.MenuId
     , lvl.AuthorityLevel
     , 0 [Available]
     , 0            [CreateBy]
     , getdate()    [CreateAt]
     , 0            [UpdateBy]
     , getdate()    [UpdateAt]
  FROM [Company] cmp
 INNER JOIN (
SELECT 1 [AuthorityLevel] UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
       ) lvl
    ON cmp.[Id]     = cmp.[Id]
 INNER JOIN [dbo].[Menu] m
    ON m.MenuId = m.MenuId
   AND NOT EXISTS (
       SELECT 1
         FROM [dbo].[MenuAuthority] ma
        WHERE ma.CompanyId  = cmp.[Id]
          AND ma.MenuId     = m.MenuId
          AND ma.AuthorityLevel = lvl.AuthorityLevel );
GO


COMMIT
GO
