use G4_Empty  --移行先のG4のデータベース
GO

/*
データ移行
G4_Empty：移行先のG4のデータベース名
G3_Convert：移行元のG3のデータベース名

下記クエリを実行する場合は、
[G3_Convert]
を、
[移行元のG3のデータベース名]
に置換してから実行すること。
*/

--対象G3 ver1.8.2.4(rev2896)
--対象G4 ver4.0.3.2(rev2178)

SET IDENTITY_INSERT Company    ON;
INSERT INTO Company ( 
  Id
, Code
, Name
, Kana
, PostalCode
, Address1
, Address2
, Tel
, Fax
, BankAccountKana
, BankName1
, BranchName1
, AccountType1
, AccountNumber1
, BankName2
, BranchName2
, AccountType2
, AccountNumber2
, BankName3
, BranchName3
, AccountType3
, AccountNumber3
, ClosingDay
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, BankAccountName
, ShowConfirmDialog
, PresetCodeSearchDialog
, ShowWarningDialog
, ProductKey
, TransferAggregate
, AutoCloseProgressDialog
)
SELECT 
  ID
, k.KAISYACD
, KAISYAMEI
, KAISYAKANA
, COALESCE(ZIPCD      , '')
, COALESCE(JYUSYO_1   , '')
, COALESCE(JYUSYO_2   , '')
, COALESCE(k.TELNO    , '')
, COALESCE(k.FAXNO    , '')
, COALESCE(KOZAMEIKANA, '')
, COALESCE(GINKO_1    , '')
, COALESCE(SHITEN_1   , '')
, COALESCE(SYUBETU_1  , '')
, COALESCE(KOZANO_1   , '')
, COALESCE(GINKO_2    , '')
, COALESCE(SHITEN_2   , '')
, COALESCE(SYUBETU_2  , '')
, COALESCE(KOZANO_2   , '')
, COALESCE(GINKO_3    , '')
, COALESCE(SHITEN_3   , '')
, COALESCE(SYUBETU_3  , '')
, COALESCE(KOZANO_3   , '')
, SHIME
, COALESCE(tr.LoginUserID, 0)
, k.R_YMD
, COALESCE(tl.LoginUserID, 0)
, k.LR_YMD
, COALESCE(KOZAMEI, '')
, KAKUNIN
, VIEWALL
, SHOW_ALERT_DIALOG
, COALESCE(PRODUCT_KEY, '')
, KOUFURI_AGGR
, AUTO_CLOSE_PROGRESS
FROM [G3_Convert].[dbo].TBLKAISYA k
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON k.KAISYACD = tr.KAISYACD
AND k.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON k.KAISYACD = tl.KAISYACD
AND k.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Company    OFF;

INSERT INTO CompanyLogo ( 
  CompanyId
, Logo
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, LOGO_DATA
, COALESCE(tr.LoginUserID, 0)
, l.R_YMD
, COALESCE(tl.LoginUserID, 0)
, l.LR_YMD
FROM [G3_Convert].[dbo].TBLLOGO l
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON l.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  k.KAISYACD = tr.KAISYACD
AND k.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  k.KAISYACD = tl.KAISYACD
AND k.LR_ID = tl.TANTOCD

INSERT INTO ApplicationControl ( 
  CompanyId
, UseDepartment
, DepartmentCodeLength
, DepartmentCodeType
, SectionCodeLength
, SectionCodeType
, AccountTitleCodeLength
, AccountTitleCodeType
, CustomerCodeLength
, CustomerCodeType
, LoginUserCodeLength
, LoginUserCodeType
, StaffCodeLength
, StaffCodeType
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, UseScheduledPayment
, UseReceiptSection
, UseAuthorization
, UseLongTermAdvanceReceived
, RegisterContractInAdvance
, UseCashOnDueDates
, UseDeclaredAmount
, UseDiscount
, UseForeignCurrency
, UseBillingFilter
, UseDistribution
, UseOperationLogging
, ApplicationEdition
, LimitAccessFolder
, RootPath
)
SELECT 
  ID
, BUMONKANRI_FLG
, BUMONCD_LEN
, BUMONCD_TYPE
, BUMONCD_LEN
, BUMONCD_TYPE
, KAMOKUCD_LEN
, KAMOKUCD_TYPE
, TOKUCD_LEN
, TOKUCD_TYPE
, TANTOCD_LEN
, TANTOCD_TYPE
, TANTOCD_LEN
, TANTOCD_TYPE
, COALESCE(tr.LoginUserID, 0)
, s.R_YMD
, COALESCE(tl.LoginUserID, 0)
, s.LR_YMD
, NYUKIN_YOTEI_INPUT_FLG
, NYUKINBUMONKANRI_FLG
, SYONIN_FLG
, TYOKIMAEUKEKANRI_FLG
, JIZEN_FLG
, KIJITSU_GENKIN_FLG
, USE_YOTEI_KESHIKOMI
, BUBIKI_FLG
, USE_FOREIGN_CURRENCY
, USE_SEIKYU_NARROWING
, MAIL_DELIVERY_FLG
, COALESCE(p.DATA, 0)
, MODEL_FLG
, LIMIT_ACCESS_FOLDER
, ROOT_PATH
FROM [G3_Convert].[dbo].TBLSEIGYO s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLACCESSLOGPROP p
ON s.KAISYACD = p.KAISYACD
AND p.SETTINGCODE = 'ログ採取'
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  s.KAISYACD = tr.KAISYACD
AND s.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  s.KAISYACD = tl.KAISYACD
AND s.LR_ID = tl.TANTOCD

INSERT INTO PasswordPolicy ( 
  CompanyId
, MinLength
, MaxLength
, UseAlphabet
, MinAlphabetUseCount
, UseNumber
, MinNumberUseCount
, UseSymbol
, MinSymbolUseCount
, SymbolType
, CaseSensitive
, MinSameCharacterRepeat
, ExpirationDays
, HistoryCount
)
SELECT 
  ID
, PW_LENMIN
, PW_LENMAX
, ALPHA_FLG
, ALPHA_MIN
, NUMBER_FLG
, NUMBER_MIN
, SYMBOL_FLG
, SYMBOL_MIN
, SYMBOL_TYPE
, DAISYO_KBT
, PW_RENZOKU
, PW_KIKAN
, PW_RIREKI
FROM [G3_Convert].[dbo].TBLPASSWORD p
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON p.KAISYACD = k.KAISYACD

INSERT INTO LoginUserLicense ( 
  CompanyId
, LicenseKey
)
SELECT 
  ID
, LICENSE
FROM [G3_Convert].[dbo].TBLTANTO_LICENSE l
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON l.KAISYACD = k.KAISYACD

INSERT INTO HolidayCalendar ( 
  CompanyId
, Holiday
)
SELECT 
  ID
, HOLIDAY
FROM [G3_Convert].[dbo].TBLCALENDAR c
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON c.KAISYACD = k.KAISYACD

SET IDENTITY_INSERT Department ON;
INSERT INTO Department ( 
  Id
, CompanyId
, Code
, Name
, StaffId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, Note
)
SELECT 
  b.ID
, k.ID
, b.BUMONCD
, b.BUMONMEI
, t.StaffID
, COALESCE(tr.LoginUserID, 0)
, b.R_YMD
, COALESCE(tl.LoginUserID, 0)
, b.LR_YMD
, COALESCE(BIKO, '')
FROM [G3_Convert].[dbo].TBLBUMON b
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON b.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO t
ON b.KAISYACD = t.KAISYACD
AND b.KAISYU_SEKININ_CD = t.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  b.KAISYACD = tr.KAISYACD
AND b.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  b.KAISYACD = tl.KAISYACD
AND b.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Department OFF;

SET IDENTITY_INSERT Staff ON;
INSERT INTO Staff ( 
  Id
, CompanyId
, Code
, Name
, DepartmentId
, Mail
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, Tel
, Fax
)
SELECT 
  t.StaffID
, k.ID
, t.TANTOCD
, t.TANTOMEI
, b.ID
, COALESCE(t.MAIL, '')
, COALESCE(tr.LoginUserID, 0)
, t.R_YMD
, COALESCE(tl.LoginUserID, 0)
, t.LR_YMD
, COALESCE(CONVERT(nvarchar(20), t.TELNO), '')
, COALESCE(CONVERT(nvarchar(20), t.FAXNO), '')
FROM [G3_Convert].[dbo].TBLTANTO t
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON t.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLBUMON b
ON t.KAISYACD = b.KAISYACD
AND t.BUMONCD = b.BUMONCD
AND t.KAISYU = 1
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  t.KAISYACD = tr.KAISYACD
AND t.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  t.KAISYACD = tl.KAISYACD
AND t.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Staff OFF;

SET IDENTITY_INSERT LoginUser ON;
INSERT INTO LoginUser ( 
  Id
, CompanyId
, Code
, Name
, DepartmentId
, Mail
, MenuLevel
, FunctionLevel
, UseClient
, UseWebViewer
, AssignedStaffId
, StringValue1
, StringValue2
, StringValue3
, StringValue4
, StringValue5
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  t.LoginUserID
, k.ID
, t.TANTOCD
, t.TANTOMEI
, b.ID
, COALESCE(t.MAIL, '')
, t.MENU
, t.SECURITY_LV
, t.RIYO
, t.WEBVIEWER
, t.StaffID
, ''
, ''
, ''
, ''
, ''
, COALESCE(tr.LoginUserID, 0)
, t.R_YMD
, COALESCE(tl.LoginUserID, 0)
, t.LR_YMD
FROM [G3_Convert].[dbo].TBLTANTO t
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON t.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLBUMON b
ON t.KAISYACD = b.KAISYACD
AND t.BUMONCD = b.BUMONCD
AND t.RIYO = 1
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  t.KAISYACD = tr.KAISYACD
AND t.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  t.KAISYACD = tl.KAISYACD
AND t.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT LoginUser OFF;

INSERT INTO LoginUserPassword (
  LoginUserId
, PasswordHash
, UpdateAt
, PasswordHash0
, PasswordHash1
, PasswordHash2
, PasswordHash3
, PasswordHash4
, PasswordHash5
, PasswordHash6
, PasswordHash7
, PasswordHash8
, PasswordHash9
)
SELECT
  u.Id
, 'BE64AE89DDD24E225434DE95D501711339BAEEE18F09BA9B4369AF27D3D60' --「PASSWORD」
, GETDATE()
, ''
, ''
, ''
, ''
, ''
, ''
, ''
, ''
, ''
, ''
FROM LoginUser u

INSERT INTO BankBranch ( 
  CompanyId
, BankCode
, BranchCode
, BankName
, BankKana
, BranchName
, BranchKana
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  ID
, GINKOCD
, SHITENCD
, GINKOMEI
, GINKOKANA
, SHITENMEI
, SHITENKANA
, COALESCE(tr.LoginUserID, 0)
, b.R_YMD
, COALESCE(tl.LoginUserID, 0)
, b.LR_YMD
FROM [G3_Convert].[dbo].TBLBANK_INFO b
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON b.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  b.KAISYACD = tr.KAISYACD
AND b.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  b.KAISYACD = tl.KAISYACD
AND b.LR_ID = tl.TANTOCD

SET IDENTITY_INSERT Section ON;
INSERT INTO Section ( 
  Id
, CompanyId
, Code
, Name
, Note
, PayerCode
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  nb.ID
, k.ID
, NYUKINBUMONCD
, NYUKINBUMONMEI
, COALESCE(BIKO, '')
, IRAICD
, COALESCE(tr.LoginUserID, 0)
, nb.R_YMD
, COALESCE(tl.LoginUserID, 0)
, nb.LR_YMD
FROM [G3_Convert].[dbo].TBLNYUKINBUMON nb
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON k.KAISYACD = nb.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  nb.KAISYACD = tr.KAISYACD
AND nb.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  nb.KAISYACD = tl.KAISYACD
AND nb.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Section OFF;

INSERT INTO SectionWithDepartment ( 
  SectionId
, DepartmentId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  nb.ID
, b.ID
, COALESCE(tr.LoginUserID, 0)
, nbk.R_YMD
, COALESCE(tl.LoginUserID, 0)
, nbk.LR_YMD
FROM [G3_Convert].[dbo].TBLNYUKINBUMON_KANKEI nbk
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON nbk.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb
ON nbk.KAISYACD = nb.KAISYACD
AND nbk.NYUKINBUMONCD = nb.NYUKINBUMONCD
INNER JOIN [G3_Convert].[dbo].TBLBUMON b
ON nbk.KAISYACD = b.KAISYACD
AND nbk.BUMONCD = b.BUMONCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  nbk.KAISYACD = tr.KAISYACD
AND nbk.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  nbk.KAISYACD = tl.KAISYACD
AND nbk.LR_ID = tl.TANTOCD

INSERT INTO SectionWithLoginUser ( 
  LoginUserId
, SectionId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  t.LoginUserID
, nb.ID
, COALESCE(tr.LoginUserID, 0)
, nbt.R_YMD
, COALESCE(tl.LoginUserID, 0)
, nbt.LR_YMD
FROM [G3_Convert].[dbo].TBLNYUKINBUMON_TANTO nbt
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON nbt.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb
ON nbt.KAISYACD = nb.KAISYACD
AND nbt.NYUKINBUMONCD = nb.NYUKINBUMONCD
INNER JOIN [G3_Convert].[dbo].TBLTANTO t
ON nbt.KAISYACD = t.KAISYACD
AND nbt.TANTOCD = t.TANTOCD
AND t.LoginUserID IS NOT NULL
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  nbt.KAISYACD = tr.KAISYACD
AND nbt.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  nbt.KAISYACD = tl.KAISYACD
AND nbt.LR_ID = tl.TANTOCD

SET IDENTITY_INSERT GeneralSetting ON;
INSERT INTO GeneralSetting ( 
  Id
, CompanyId
, Code
, Value
, Length
, Precision
, Description
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  kn.ID
, k.ID
, KANRICD
, KANRIDATA
, KETASU
, SYOSU
, SETUMEI
, COALESCE(tr.LoginUserId, 0)
, kn.R_YMD
, COALESCE(tl.LoginUserId, 0)
, kn.LR_YMD
FROM [G3_Convert].[dbo].TBLKANRI kn
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON kn.KAISYACD = k.KAISYACD
INNER JOIN GeneralSettingBase gb
ON kn.KANRICD = gb.Code COLLATE JAPANESE_CI_AS
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  kn.KAISYACD = tr.KAISYACD
AND kn.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  kn.KAISYACD = tl.KAISYACD
AND kn.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT GeneralSetting OFF;

INSERT INTO MenuAuthority (
  CompanyId
, MenuId
, AuthorityLevel
, Available
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT
  k.ID
, gc.GAMENID_NEW
, KENGEN
, SYUBETU
, COALESCE(tr.LoginUserID, 0)
, ke.R_YMD
, COALESCE(tl.LoginUserID, 0)
, ke.LR_YMD
FROM [G3_Convert].[dbo].TBLKENGEN ke
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON ke.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLGAMEN_CONVERT gc
ON ke.GAMEN_ID = 'frm' + gc.GAMENID_OLD
--INNER JOIN Menu m
--ON gc.GAMENID_NEW = m.MenuId COLLATE JAPANESE_CI_AS
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  ke.KAISYACD = tr.KAISYACD
AND ke.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  ke.KAISYACD = tl.KAISYACD
AND ke.LR_ID = tl.TANTOCD

INSERT INTO FunctionAuthority ( 
  CompanyId
, AuthorityLevel
, FunctionType
, Available
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, KENGEN_LV
, CASE SECURITY 
    WHEN 'マスターインポート'   THEN 0
    WHEN 'マスターエクスポート' THEN 1
    WHEN '請求データ修正・削除' THEN 2
    WHEN '請求データ復活'       THEN 3
    WHEN '入金データ修正・削除' THEN 4
    WHEN '入金データ復活'       THEN 5
    WHEN '消込解除'             THEN 6
    ELSE NULL END [SECURITY]
, SYUBETU
, COALESCE(tr.LoginUserID, 0)
, ke.R_YMD
, COALESCE(tl.LoginUserId, 0)
, ke.LR_YMD
FROM [G3_Convert].[dbo].TBLKENGEN_LV ke
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON ke.KAISYACD = k.KAISYACD
AND ke.[SECURITY] <> '請求書発行取消'
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  ke.KAISYACD = tr.KAISYACD
AND ke.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  ke.KAISYACD = tl.KAISYACD
AND ke.LR_ID = tl.TANTOCD

SET IDENTITY_INSERT PaymentAgency ON;
INSERT INTO PaymentAgency ( 
  Id
, CompanyId
, Code
, Name
, Kana
, ConsigneeCode
, ShareTransferFee
, UseFeeTolerance
, UseFeeLearning
, UseKanaLearning
, DueDateOffset
, BankCode
, BankName
, BranchCode
, BranchName
, AccountTypeId
, AccountNumber
, FileFormatId
, ConsiderUncollected
, CollectCategoryId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, OutputFileName
, AppendDate
)
SELECT 
  kd.ID
, k.ID
, kd.KESSAI_DAIKOUCD
, KESSAI_DAIKOUMEI
, KESSAI_DAIKOUKANA
, ITAKUCD
, FUTAN
, TESURYO_GAKUSYU
, 1
, 1
, YOTEIBI_OFFSET
, kd.GINKOCD
, COALESCE(b.GINKOMEI, '')
, kd.SHITENCD
, COALESCE(b.SHITENMEI, '')
, kd.YOKINSYU
, kd.KOUZANO
, CASE [FORMAT]
    WHEN 0 THEN 1 --全銀（口座振替 カンマ区切り）
    WHEN 1 THEN 3 --みずほファクター（Web伝送）
    WHEN 2 THEN 2 --全銀（口座振替 固定長）
    WHEN 3 THEN 4 --三菱UFJファクター
    WHEN 4 THEN 5 --SMBC（口座振替 固定長）
    WHEN 5 THEN 6 --三菱UFJニコス
    WHEN 6 THEN 7 --みずほファクター（ASPサービス）
    WHEN 7 THEN 8 --リコーリースコレクト！
    ELSE 0 END
, KOUSHIN
, kb.Id
, COALESCE(tr.LoginUserID, 0)
, kd.R_YMD
, COALESCE(tl.LoginUserID, 0)
, kd.LR_YMD
, FILENAME
, DATE_F
FROM [G3_Convert].[dbo].TBLKESSAI_DAIKOU kd
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON kd.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLBANK b
ON kd.KAISYACD = b.KAISYACD
AND kd.GINKOCD = b.GINKOCD
AND kd.SHITENCD = b.SHITENCD
AND kd.YOKINSYU = b.YOKINSYU
AND kd.KOUZANO = b.KOUZANO
LEFT JOIN [G3_Convert].[dbo].TBLKUBUN kb
ON kd.KAISYACD = kb.KAISYACD
AND kd.FURIKAE_KBN = kb.KUBUNCD
AND kb.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  kd.KAISYACD = tr.KAISYACD
AND kd.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  kd.KAISYACD = tl.KAISYACD
AND kd.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT PaymentAgency OFF;

SET IDENTITY_INSERT AccountTitle ON;
INSERT INTO AccountTitle ( 
  Id
, CompanyId
, Code
, Name
, ContraAccountCode
, ContraAccountName
, ContraAccountSubCode
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  km.ID
, k.KAISYACD
, KAMOKUCD
, KAMOKUMEI
, COALESCE(AITECD, '')
, COALESCE(AITEMEI, '')
, COALESCE(AITEHOJO, '')
, COALESCE(tr.LoginUserID, 0)
, km.R_YMD
, COALESCE(tr.LoginUserID, 0)
, km.LR_YMD
FROM [G3_Convert].[dbo].TBLKAMOKU km
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON km.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  km.KAISYACD = tr.KAISYACD
AND km.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  km.KAISYACD = tl.KAISYACD
AND km.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT AccountTitle OFF;

SET IDENTITY_INSERT Category ON;
INSERT INTO Category ( 
  Id
, CompanyId
, CategoryType
, Code
, Name
, AccountTitleId
, SubCode
, Note
, TaxClassId
, UseLimitDate
, UseLongTermAdvanceReceived
, UseCashOnDueDates
, UseAccountTransfer
, PaymentAgencyId
, UseDiscount
, UseAdvanceReceived
, ForceMatchingIndividually
, UseInput
, MatchingOrder
, StringValue1
, StringValue2
, StringValue3
, StringValue4
, StringValue5
, IntValue1
, IntValue2
, IntValue3
, IntValue4
, IntValue5
, NumericValue1
, NumericValue2
, NumericValue3
, NumericValue4
, NumericValue5
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, ExternalCode
)
SELECT 
  kb.ID
, k.ID
, SIKIBETU
, KUBUNCD
, KUBUNMEI
, km.ID
, COALESCE(EDABANCD, '')
, COALESCE(kb.BIKO, '')
, CASE WHEN TAX_ZOKUSEI = 0 THEN NULL ELSE TAX_ZOKUSEI END
, COALESCE(KAGEN, 0)
, COALESCE(TYOMAEKANRI_FLG, 0)
, COALESCE(KIJITSU_KANRI, 0)
, COALESCE(KOUZA_FURIKAE, 0)
, kd.ID
, COALESCE(BUBIKI_FLG, 0)
, COALESCE(CASE WHEN SIKIBETU = 2 AND KUBUNCD = '99' THEN 1 ELSE 0 END, 0)
, COALESCE(IKKATSU_TAISYOGAI_FLG, 0)
, COALESCE(USE_INPUT, 0)
, COALESCE(KESHIKOMI_ORDER, 0)
, ''
, ''
, ''
, ''
, ''
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, COALESCE(tr.LoginUserID, 0)
, kb.R_YMD
, COALESCE(tl.LoginUserID, 0)
, kb.LR_YMD
, COALESCE(EXTERNALCD, '')
FROM [G3_Convert].[dbo].TBLKUBUN kb
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON kb.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLKAMOKU km
ON kb.KAISYACD = km.KAISYACD
AND kb.KAMOKUCD = km.KAMOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLKESSAI_DAIKOU kd
ON kb.KAISYACD = kd.KAISYACD
AND kb.KESSAI_DAIKOUCD = kd.KESSAI_DAIKOUCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  kb.KAISYACD = tr.KAISYACD
AND kb.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  kb.KAISYACD = tl.KAISYACD
AND kb.LR_ID = tl.TANTOCD

UNION ALL

SELECT 
  tg.ID
, k.ID
, 4
, KUBUNCD
, KUBUNMEI
, km.ID
, COALESCE(EDABANCD, '')
, COALESCE(TEKIYO, '')
, CASE WHEN TAX_ZOKUSEI = 0 THEN NULL ELSE TAX_ZOKUSEI END
, 0
, 0
, 0
, 0
, NULL
, 0
, 0
, 0
, 0
, 0
, ''
, ''
, ''
, ''
, ''
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, COALESCE(tr.LoginUserID, 0)
, tg.R_YMD
, COALESCE(tl.LoginUserID, 0)
, tg.LR_YMD
, ''
FROM [G3_Convert].[dbo].TBLTAIGAI tg
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON tg.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLKAMOKU km
ON tg.KAISYACD = km.KAISYACD
AND tg.KAMOKUCD = km.KAMOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  tg.KAISYACD = tr.KAISYACD
AND tg.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  tg.KAISYACD = tl.KAISYACD
AND tg.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Category OFF;

SET IDENTITY_INSERT Currency ON;
INSERT INTO Currency ( 
  Id
, CompanyId
, Code
, Name
, Symbol
, Precision
, Note
, DisplayOrder
, Tolerance
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  c.ID
, k.ID
, CCY
, CNAME
, LEFT(UNIT, 1)
, DECIMAL_DIGITS
, COALESCE(BIKO, '')
, DISP_ORDER
, TESURYO_GOSA
, 0
, GETDATE()
, 0
, GETDATE()
FROM [G3_Convert].[dbo].TBLCURRENCY c
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON c.KAISYACD = k.KAISYACD
SET IDENTITY_INSERT Currency OFF;

SET IDENTITY_INSERT BankAccount ON;
INSERT INTO BankAccount ( 
  Id
, CompanyId
, BankCode
, BranchCode
, AccountTypeId
, AccountNumber
, BankName
, BranchName
, ReceiptCategoryId
, SectionId
, ImportSkipping
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  b.ID
, k.ID
, GINKOCD
, SHITENCD
, YOKINSYU
, KOUZANO
, GINKOMEI
, SHITENMEI
, kb.ID
, nb.ID
, TORIKOMI_TAISYOGAI_FLG
, COALESCE(tr.LoginUserID, 0)
, b.R_YMD
, COALESCE(tl.LoginUserID, 0)
, b.LR_YMD
FROM [G3_Convert].[dbo].TBLBANK b
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON b.KAISYACD = k.KAISYACD
INNER JOIN (
SELECT
  KAISYACD
, MIN(ID) ID
FROM [G3_Convert].[dbo].TBLKUBUN
WHERE SIKIBETU = '2'
GROUP BY KAISYACD
) kb
ON b.KAISYACD = kb.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb
ON b.KAISYACD = nb.KAISYACD
AND b.NYUKINBUMONCD = nb.NYUKINBUMONCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  b.KAISYACD = tr.KAISYACD
AND b.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  b.KAISYACD = tl.KAISYACD
AND b.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT BankAccount OFF;

SET IDENTITY_INSERT Customer ON;
INSERT INTO Customer ( 
  Id
, CompanyId
, Code
, Name
, Kana
, PostalCode
, Address1
, Address2
, Tel
, Fax
, CustomerStaffName
, ExclusiveBankCode
, ExclusiveBankName
, ExclusiveBranchCode
, ExclusiveBranchName
, ExclusiveAccountNumber
, ExclusiveAccountTypeId
, ShareTransferFee
, CreditLimit
, ClosingDay
, CollectCategoryId
, CollectOffsetMonth
, CollectOffsetDay
, StaffId
, IsParent
, Note
, SightOfBill
, DensaiCode
, CreditCode
, CreditRank
, TransferBankCode
, TransferBankName
, TransferBranchCode
, TransferBranchName
, TransferAccountNumber
, TransferAccountTypeId
, ReceiveAccountId1
, ReceiveAccountId2
, ReceiveAccountId3
, UseFeeLearning
, UseKanaLearning
, HolidayFlag
, UseFeeTolerance
, StringValue1
, StringValue2
, StringValue3
, StringValue4
, StringValue5
, IntValue1
, IntValue2
, IntValue3
, IntValue4
, IntValue5
, NumericValue1
, NumericValue2
, NumericValue3
, NumericValue4
, NumericValue5
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, TransferCustomerCode
, TransferNewCode
, TransferAccountName
, PrioritizeMatchingIndividually
, CollationKey
)
SELECT 
  tk.ID
, k.ID
, TOKUCD
, TOKUMEI
, TOKUKANA
, COALESCE(tk.ZIPCD, '')
, COALESCE(ADDRESS1, '')
, COALESCE(ADDRESS2, '')
, COALESCE(tk.TELNO, '')
, COALESCE(tk.FAXNO, '')
, COALESCE(AITESAKI_TANTO, '')
, COALESCE(SEN_GINKOCD , '')
, COALESCE(SEN_GINKOMEI, '')
, COALESCE(SEN_SITENCD  , '')
, COALESCE(SEN_SHITENMEI, '')
, COALESCE(SEN_KOZANO, '')
, YOKINSYU
, CASE FUTAN WHEN '1' THEN 1 ELSE 0 END
, COALESCE(YOSIN, 0)
, tk.SHIME
, kb.ID
, LEFT(KAISYUHI, 1)  KAISYUGETSU
, RIGHT(KAISYUHI, 2) KAISYUHI
, tn.StaffID E_TANTOID
, SAIKEN_FLG
, COALESCE(tk.BIKO, '')
, KAISYUSIGHT
, COALESCE(DEN_KAISYACD , '')
, COALESCE(SHIN_KAISYACD, '')
, COALESCE(YOSHINRANK, '')
, COALESCE(FURI_GINKOCD  , '')
, COALESCE(FURI_GINKOMEI , '')
, COALESCE(FURI_SHITENCD , '')
, COALESCE(FURI_SHITENMEI, '')
, COALESCE(FURI_KOZANO   , '')
, FURI_YOKINSYU
, HIFURIKOZA_1
, HIFURIKOZA_2
, HIFURIKOZA_3
, TESURYO_GAKUSYU
, JIDOU_GAKUSYU
, HOLIDAY
, TESURYO_GOSA
, ''
, ''
, ''
, ''
, ''
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, NULL
, COALESCE(tr.LoginUserID, 0)
, tk.R_YMD
, COALESCE(tl.LoginUserID, 0)
, tk.LR_YMD
, COALESCE(KOUFURICUSTOMERCODE, '')
, COALESCE(NEWCODE, '')
, COALESCE(FURI_KOZAMEI, '')
, IKKATSU_TAISYOGAI
, COALESCE(SYOUGOU_NUMBER, '')
FROM [G3_Convert].[dbo].TBLTOKUI tk
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON tk.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTANTO tn
ON tk.KAISYACD = tn.KAISYACD
AND tk.E_TANTOCD = tn.TANTOCD
INNER JOIN [G3_Convert].[dbo].TBLKUBUN kb
ON tk.KAISYACD = kb.KAISYACD
AND tk.KAISYU = kb.KUBUNCD
AND kb.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  tk.KAISYACD = tr.KAISYACD
AND tk.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  tk.KAISYACD = tl.KAISYACD
AND tk.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Customer OFF;

INSERT INTO CustomerPaymentContract ( 
  CustomerId
, ThresholdValue
, LessThanCollectCategoryId
, GreaterThanCollectCategoryId1
, GreaterThanRate1
, GreaterThanRoundingMode1
, GreaterThanSightOfBill1
, GreaterThanCollectCategoryId2
, GreaterThanRate2
, GreaterThanRoundingMode2
, GreaterThanSightOfBill2
, GreaterThanCollectCategoryId3
, GreaterThanRate3
, GreaterThanRoundingMode3
, GreaterThanSightOfBill3
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  tk.ID
, Y_KINGAKU
, kb0.ID
, kb1.ID
, BUNKATSU_1
, HASU_1
, COALESCE(SIGHT_1, 0)
, kb2.ID
, BUNKATSU_2
, HASU_2
, SIGHT_2
, kb3.ID
, BUNKATSU_3
, HASU_3
, SIGHT_3
, COALESCE(tr.LoginUserID, 0)
, y.R_YMD
, COALESCE(tl.LoginUserID, 0)
, y.LR_YMD
FROM [G3_Convert].[dbo].TBLYAKUJO y
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON y.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON y.KAISYACD = tk.KAISYACD
AND y.TOKUCD = tk.TOKUCD
INNER JOIN [G3_Convert].[dbo].TBLKUBUN kb0
ON y.KAISYACD = kb0.KAISYACD
AND y.MIMAN = kb0.KUBUNCD
AND kb0.SIKIBETU = '3'
INNER JOIN [G3_Convert].[dbo].TBLKUBUN kb1
ON y.KAISYACD = kb1.KAISYACD
AND y.IJOU_1  = kb1.KUBUNCD
AND kb1.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLKUBUN kb2
ON y.KAISYACD = kb2.KAISYACD
AND y.IJOU_2  = kb2.KUBUNCD
AND kb2.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLKUBUN kb3
ON y.KAISYACD = kb3.KAISYACD
AND y.IJOU_3  = kb3.KUBUNCD
AND kb3.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  y.KAISYACD = tr.KAISYACD
AND y.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  y.KAISYACD = tl.KAISYACD
AND y.LR_ID = tl.TANTOCD

INSERT INTO CustomerGroup ( 
  ParentCustomerId
, ChildCustomerId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  tp.ID
, tc.ID
, COALESCE(tr.LoginUserID, 0)
, s.R_YMD
, COALESCE(tl.LoginUserID, 0)
, s.LR_YMD
FROM [G3_Convert].[dbo].TBLSAIKEN s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tp
ON s.KAISYACD = tp.KAISYACD
AND s.SAIKENCD = tp.TOKUCD
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tc
ON s.KAISYACD = tc.KAISYACD
AND s.TOKUCD = tc.TOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  s.KAISYACD = tr.KAISYACD
AND s.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  s.KAISYACD = tl.KAISYACD
AND s.LR_ID = tl.TANTOCD

INSERT INTO CustomerFee ( 
  CustomerId
, CurrencyId
, Fee
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  tk.ID
, c.ID
, TESURYO
, COALESCE(tr.LoginUserID, 0)
, tg.R_YMD
, COALESCE(tl.LoginUserID, 0)
, tg.LR_YMD
FROM [G3_Convert].[dbo].TBLTESURYO_GAKUSYU tg
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON tg.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON tg.KAISYACD = tk.KAISYACD
AND tg.TOKUCD = tk.TOKUCD
AND tg.KESSAI_DAIKOU = 0
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY c
ON tg.KAISYACD = c.KAISYACD
AND tg.CCY = c.CCY
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  tg.KAISYACD = tr.KAISYACD
AND tg.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  tg.KAISYACD = tl.KAISYACD
AND tg.LR_ID = tl.TANTOCD

INSERT INTO PaymentAgencyFee ( 
  PaymentAgencyId
, CurrencyId
, Fee
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  kd.ID
, c.ID
, TESURYO
, COALESCE(tr.LoginUserID, 0)
, tg.R_YMD
, COALESCE(tl.LoginUserID, 0)
, tg.LR_YMD
FROM [G3_Convert].[dbo].TBLTESURYO_GAKUSYU tg
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON tg.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLKESSAI_DAIKOU kd
ON tg.KAISYACD = kd.KAISYACD
AND tg.TOKUCD = kd.KESSAI_DAIKOUCD
AND tg.KESSAI_DAIKOU = 1
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY c
ON tg.KAISYACD = c.KAISYACD
AND tg.CCY = c.CCY
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  tg.KAISYACD = tr.KAISYACD
AND tg.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  tg.KAISYACD = tl.KAISYACD
AND tg.LR_ID = tl.TANTOCD

INSERT INTO KanaHistoryCustomer ( 
  CompanyId
, PayerName
, SourceBankName
, SourceBranchName
, CustomerId
, HitCount
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, FURIMEI
, SHIMU_GINKO
, SHIMU_SHITEN
, tk.ID
, KAISU
, COALESCE(tr.LoginUserID, 0)
, kn.R_YMD
, COALESCE(tl.LoginUserID, 0)
, kn.LR_YMD
FROM [G3_Convert].[dbo].TBLKANA kn
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON kn.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON kn.KAISYACD = tk.KAISYACD
AND kn.TOKUCD = tk.TOKUCD
AND kn.KESSAI_DAIKOU = 0
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  kn.KAISYACD = tr.KAISYACD
AND kn.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  kn.KAISYACD = tl.KAISYACD
AND kn.LR_ID = tl.TANTOCD

INSERT INTO KanaHistoryPaymentAgency ( 
  CompanyId
, PayerName
, SourceBankName
, SourceBranchName
, PaymentAgencyId
, HitCount
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, FURIMEI
, SHIMU_GINKO
, SHIMU_SHITEN
, kd.ID
, KAISU
, COALESCE(tr.LoginUserID, 0)
, kn.R_YMD
, COALESCE(tl.LoginUserID, 0)
, kn.LR_YMD
FROM [G3_Convert].[dbo].TBLKANA kn
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON kn.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLKESSAI_DAIKOU kd
ON  kn.KAISYACD = kd.KAISYACD
AND kn.TOKUCD = kd.KESSAI_DAIKOUCD
AND kn.KESSAI_DAIKOU = 1
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  kn.KAISYACD = tr.KAISYACD
AND kn.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  kn.KAISYACD = tl.KAISYACD
AND kn.LR_ID = tl.TANTOCD

INSERT INTO IgnoreKana ( 
  CompanyId
, Kana
, ExcludeCategoryId
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, KANAMEI
, tg.ID
, COALESCE(tr.LoginUserID, 0)
, tk.R_YMD
, COALESCE(tl.LoginUserID, 0)
, tk.LR_YMD
FROM [G3_Convert].[dbo].TBLTAIGAIKANA tk
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON tk.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTAIGAI tg
ON tk.KAISYACD = tg.KAISYACD
AND tk.KUBUNCD = tg.KUBUNCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  tk.KAISYACD = tr.KAISYACD
AND tk.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  tk.KAISYACD = tl.KAISYACD
AND tk.LR_ID = tl.TANTOCD

INSERT INTO JuridicalPersonality ( 
  CompanyId
, Kana
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, HOUJINKAKU
, COALESCE(tr.LoginUserID, 0)
, h.R_YMD
, COALESCE(tl.LoginUserID, 0)
, h.LR_YMD
FROM [G3_Convert].[dbo].TBLHOUJINKAKU h
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON h.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  h.KAISYACD = tr.KAISYACD
AND h.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  h.KAISYACD = tl.KAISYACD
AND h.LR_ID = tl.TANTOCD

INSERT INTO MasterImportSetting ( 
  CompanyId
, ImportFileType
, ImportFileName
, ImportMode
, ExportErrorLog
, ErrorLogDestination
, Confirm
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, b.ImportFileType
, b.ImportFileName
, IMPORT_MODE
, IS_OUTPUT_LOG
, OUTPUT_LOG_MODE
, IS_NEED_CONFIRM
, COALESCE(tr.LoginUserID, 0)
, im.R_YMD
, COALESCE(tl.LoginUserID, 0)
, im.LR_YMD
FROM [G3_Convert].[dbo].TBLIMPORTSETTING im
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON im.KAISYACD = k.KAISYACD
INNER JOIN MasterImportSettingBase b
ON im.TARGET_SEQ = CASE b.ImportFileType
    WHEN  1 THEN 0
    WHEN  2 THEN 1
    WHEN  3 THEN 1
    WHEN  4 THEN 2
    WHEN  5 THEN 2
    WHEN  6 THEN 2
    WHEN  7 THEN 3
    WHEN  8 THEN 4
    WHEN  9 THEN 5
    WHEN 10 THEN 6
    WHEN 11 THEN 7
    WHEN 12 THEN 8
    WHEN 13 THEN 9
    WHEN 14 THEN 10
    WHEN 15 THEN 11
    WHEN 16 THEN 12
    WHEN 17 THEN 15
    WHEN 18 THEN 16
    WHEN 19 THEN 17 ELSE -1 END
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  im.KAISYACD = tr.KAISYACD
AND im.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  im.KAISYACD = tl.KAISYACD
AND im.LR_ID = tl.TANTOCD

SET IDENTITY_INSERT ImporterSetting ON;
INSERT INTO ImporterSetting ( 
  Id
, CompanyId
, FormatId
, Code
, Name
, InitialDirectory
, EncodingCodePage
, StartLineCount
, IgnoreLastLine
, AutoCreationCustomer
, PostAction
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  m.ID
, k.ID
, CASE PATTERN_KBN 
    WHEN '01' THEN 1
    WHEN '02' THEN 2
    WHEN '03' THEN 3
    WHEN '50' THEN 4
    ELSE NULL END
, PATTERN_NO
, PATTERN_MEI
, MOVE_FOLDER
, 932 EncodingCodePage
, TORIKOMI_START_GYO
, LAST_GYO_FLG
, COALESCE(TOKUCD_AUTO_CREATE, 0)
, CASE MOVE_FLG
    WHEN 1 THEN 0
    WHEN 2 THEN 1
    WHEN 3 THEN 2
    ELSE 0 END
, COALESCE(tr.LoginUserID, 0)
, m.R_YMD
, COALESCE(tl.LoginUserID, 0)
, m.LR_YMD
FROM [G3_Convert].[dbo].TBLPATTERN_M m
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON m.KAISYACD = k.KAISYACD
AND m.PATTERN_KBN IN ('01', '02', '03', '50')
AND m.ID IS NOT NULL
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  m.KAISYACD = tr.KAISYACD
AND m.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  m.KAISYACD = tl.KAISYACD
AND m.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT ImporterSetting OFF;

INSERT INTO ImporterSettingDetail ( 
  ImporterSettingId
, Sequence
, ImportDivision
, FieldIndex
, Caption
, AttributeDivision
, ItemPriority
, DoOverwrite
, IsUnique
, FixedValue
, UpdateKey
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  m.ID
, bc.GYO_NO_NEW
, TORIKOMI_UMU
, COALESCE(ITEM_NO, 0)
, COALESCE(ITEM_MEI, '')
, ZOKUSEI_INFO
, EXT_INT1
, UWAGAKI
, TORIKOMI_KEY
, COALESCE(CASE WHEN d.PATTERN_KBN = '50' and d.PATTERN_GYO_NO = 15 AND b.DISP_ITEM_MEI = '回収予定' THEN LEFT(KOTEITI, 1) ELSE KOTEITI END, '')
, COALESCE(KOUSIN, 0)
, COALESCE(tr.LoginUserID, 0)
, d.R_YMD
, COALESCE(tl.LoginUserID, 0)
, d.LR_YMD
FROM [G3_Convert].[dbo].TBLPATTERN_D d
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON d.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLPATTERN_M m
ON d.KAISYACD = m.KAISYACD
AND d.PATTERN_KBN = m.PATTERN_KBN
AND d.PATTERN_NO = m.PATTERN_NO
AND m.PATTERN_KBN IN ('01', '02', '03')
AND m.ID IS NOT NULL
INNER JOIN [G3_Convert].[dbo].TBLPATTERN_B b
ON d.PATTERN_KBN = b.PATTERN_KBN
AND d.PATTERN_GYO_NO = b.GYO_NO
INNER JOIN [G3_Convert].[dbo].TBLPATTERN_B_CONVERT bc
ON d.PATTERN_KBN = bc.PATTERN_KBN_OLD
AND d.PATTERN_GYO_NO = bc.GYO_NO_OLD
AND b.DISP_ITEM_MEI = bc.ITEM_MEI_OLD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON d.KAISYACD = tr.KAISYACD
AND d.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON d.KAISYACD = tl.KAISYACD
AND d.LR_ID = tl.TANTOCD

UNION ALL

SELECT
  m.ID
, 16
, TORIKOMI_UMU
, COALESCE(ITEM_NO, 0)
, COALESCE(ITEM_MEI, '')
, ZOKUSEI_INFO
, EXT_INT1
, UWAGAKI
, TORIKOMI_KEY
, RIGHT(KOTEITI, 2)
, COALESCE(KOUSIN, 0)
, COALESCE(tr.LoginUserID, 0)
, d.R_YMD
, COALESCE(tl.LoginUserID, 0)
, d.LR_YMD
FROM [G3_Convert].[dbo].TBLPATTERN_D d
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON d.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLPATTERN_M m
ON d.KAISYACD = m.KAISYACD
AND d.PATTERN_KBN = m.PATTERN_KBN
AND d.PATTERN_NO = m.PATTERN_NO
AND m.PATTERN_KBN = '50'
AND m.PATTERN_NO = '01'
ANd d.PATTERN_GYO_NO = 15
INNER JOIN [G3_Convert].[dbo].TBLPATTERN_B b
ON d.PATTERN_KBN = b.PATTERN_KBN
AND d.PATTERN_GYO_NO = b.GYO_NO
AND b.DISP_ITEM_MEI = '回収予定'
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON d.KAISYACD = tr.KAISYACD
AND d.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON d.KAISYACD = tl.KAISYACD
AND d.LR_ID = tl.TANTOCD

INSERT INTO EBExcludeAccountSetting ( 
  CompanyId
, BankCode
, BranchCode
, AccountTypeId
, PayerCode
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, GINKOCD
, SHITENCD
, YOKINSYU
, IRAICD
, COALESCE(tr.LoginUserID, 0)
, e.R_YMD
, COALESCE(tl.LoginUserID, 0)
, e.LR_YMD
FROM [G3_Convert].[dbo].TBLEB_TAIGAI_ACCOUNT e
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON e.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  e.KAISYACD = tr.KAISYACD
AND e.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  e.KAISYACD = tl.KAISYACD
AND e.LR_ID = tl.TANTOCD

INSERT INTO ExportFieldSetting ( 
  CompanyId
, ExportFileType
, ColumnName
, ColumnOrder
, AllowExport
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, DataFormat
)
SELECT 
  k.ID
, SYUBETSU
, c.COLUMN_NAME_NEW
, COLUMN_ORDER
, IS_EXPORT
, COALESCE(tr.LoginUserID, 0)
, s.R_YMD
, COALESCE(tr.LoginUserID, 0)
, GETDATE()
, DATA_FORMAT
FROM [G3_Convert].[dbo].TBLEXPORT_SETTING s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLEXPORT_SETTING_CONVERT c
ON s.COLUMN_NAME = c.COLUMN_NAME_OLD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  s.KAISYACD = tr.KAISYACD
AND s.R_ID = tr.TANTOCD

INSERT INTO MatchingOrder ( 
  CompanyId
, TransactionCategory
, ItemName
, ExecutionOrder
, Available
, SortOrder
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, c.SYUBETU
, c.PARAM_NAME_NEW
, SORT_NUM
, [ENABLE]
, SORT_ORDER
, COALESCE(tr.LoginUserID, 0)
, ko.R_YMD
, COALESCE(tr.LoginUserID, 0)
, GETDATE()
FROM [G3_Convert].[dbo].TBLKESHIKOMI_ORDER ko
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON ko.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLKESHIKOMI_ORDER_CONVERT c
ON ko.SYUBETU = c.SYUBETU
AND ko.PARAM_NAME = c.PARAM_NAME_OLD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  ko.KAISYACD = tr.KAISYACD
AND ko.R_ID = tr.TANTOCD

INSERT INTO CollationOrder ( 
  CompanyId
, CollationTypeId
, ExecutionOrder
, Available
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, CASE SYOUGOU_NAME
    WHEN 'SP_IRAICD_SYOUGOU'   THEN 0
    WHEN 'SP_TOKUCD_SYOUGOU'   THEN 1
    WHEN 'SP_GAKUSYU_SYOUGOU'  THEN 2
    WHEN 'SP_TOKUKANA_SYOUGOU' THEN 3
    WHEN 'SP_NUMBER_SYOUGOU'   THEN 4
    ELSE NULL END
, SYOUGOU_ORDER
, [ENABLE]
, COALESCE(tr.LoginUserID, 0)
, so.R_YMD
, COALESCE(tr.LoginUserID, 0)
, GETDATE()
FROM [G3_Convert].[dbo].TBLSYOUGOU_ORDER so
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON so.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  so.KAISYACD = tr.KAISYACD
AND so.R_ID = tr.TANTOCD

INSERT INTO CollationSetting ( 
  CompanyId
, RequiredCustomer
, AutoAssignCustomer
, LearnKanaHistory
, UseApportionMenu
, ReloadCollationData
, UseAdvanceReceived
, AdvanceReceivedRecordedDateType
, AutoMatching
, AutoSortMatchingEnabledData
, PrioritizeMatchingIndividuallyMultipleReceipts
, PrioritizeMatchingIndividuallyTaxTolerance
, ForceShareTransferFee
, LearnSpecifiedCustomerKana
, MatchingSilentSortedData
, BillingReceiptDisplayOrder
, RemoveSpaceFromPayerName
, JournalizingPattern
, CalculateTaxByInputId
)
SELECT
  k.ID
, TOKUCD_SET
, AUTO_SET
, USE_GAKUSYU
, USE_FURIWAKE
, KESHI_AUTO_REFRESH
, USE_MAEUKE_FURIKAE
, DENPYOUHIZUKE_SET
, AUTO_IKKATSU_KESHIKOMI
, KESHIKOMI_SORT
, FUKUSU_NYUKIN_IKKATSU_CHECK_OFF
, SHOUHI_GOSA_IKKATSU_CHECK_OFF
, JISYA_FUTAN_IKKATSU_CHECK_OFF
, USE_GAKUSHU_AT_NYUKIN_SYUSEI
, KOBETSU_KESHIKOMI_ORDER
, SHOUHI_GOSA_IKKATSU_CHECK_OFF
, NYUKIN_TORIKOMI_REMOVE_SPACE
, SHIWAKE_PATTERN
, CALC_TAX_BY_INVOICE
FROM [G3_Convert].[dbo].TBLSYOUGOU_SETTING s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD

INSERT INTO ColumnNameSetting ( 
  CompanyId
, TableName
, ColumnName
, OriginalName
, Alias
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  k.ID
, CASE TABLE_NAME
    WHEN 'TBLSEIKYU' THEN 'Billing'
    WHEN 'TBLNYUKIN' THEN 'Receipt'
    ELSE NULL END
, CASE TABLE_NAME
    WHEN 'TBLSEIKYU' THEN
        CASE COLUMN_NAME
            WHEN 'BIKO'  THEN 'Note1'
            WHEN 'BIKO2' THEN 'Note2'
            WHEN 'BIKO3' THEN 'Note3'
            WHEN 'BIKO4' THEN 'Note4'
            WHEN 'BIKO5' THEN 'Note5'
            WHEN 'BIKO6' THEN 'Note6'
            WHEN 'BIKO7' THEN 'Note7'
            WHEN 'BIKO8' THEN 'Note8'
            ELSE NULL END
    WHEN 'TBLNYUKIN' THEN
        CASE COLUMN_NAME
            WHEN 'TEKIYO' THEN 'Note1'
            WHEN 'BIKO2'  THEN 'Note2'
            WHEN 'BIKO3'  THEN 'Note3'
            WHEN 'BIKO4'  THEN 'Note4'
            ELSE NULL END
    ELSE NULL END
, ORIGINAL_CAPTION
, ALTER_CAPTION
, COALESCE(tr.LoginUserID, 0)
, c.R_YMD
, COALESCE(tl.LoginUserID, 0)
, c.LR_YMD
FROM [G3_Convert].[dbo].TBLCOLUMN_NAME c
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON c.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  c.KAISYACD = tr.KAISYACD
AND c.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  c.KAISYACD = tl.KAISYACD
AND c.LR_ID = tl.TANTOCD
WHERE c.TABLE_NAME IN ('TBLSEIKYU', 'TBLNYUKIN')

SET IDENTITY_INSERT BillingInput ON;
INSERT INTO BillingInput
(
 Id
,PublishAt
,PublishAt1st
)
SELECT
 s.InputID
,CONVERT(datetime2(3), MAX(s.HAKKOUBI))
,CONVERT(datetime2(3), MAX(s.HAKKOUBI_1ST))
FROM [G3_Convert].[dbo].TBLSEIKYU s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
WHERE s.InputID IS NOT NULL
GROUP BY s.InputID
SET IDENTITY_INSERT BillingInput OFF;

SET IDENTITY_INSERT Billing ON;
INSERT INTO Billing ( 
  Id
, CompanyId
, CurrencyId
, CustomerId
, DepartmentId
, StaffId
, BillingCategoryId
, InputType
, BillingInputId
, BilledAt
, ClosingAt
, SalesAt
, DueAt
, BillingAmount
, TaxAmount
, AssignmentAmount
, RemainAmount
, OffsetAmount
, AssignmentFlag
, Approved
, CollectCategoryId
, OriginalCollectCategoryId
, DebitAccountTitleId
, CreditAccountTitleId
, OriginalDueAt
, OutputAt
, PublishAt
, InvoiceCode
, TaxClassId
, Note1
, Note2
, Note3
, Note4
, Note5
, Note6
, Note7
, Note8
, DeleteAt
, RequestDate
, ResultCode
, TransferOriginalDueAt
, ScheduledPaymentKey
, Quantity
, UnitPrice
, UnitSymbol
, Price
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, AccountTransferLogId
)
SELECT 
  s.ID
, k.ID
, ccy.ID
, tk.ID
, b.ID
, t.StaffID
, skb.ID
, DATAKBN
, s.InputID
, CONVERT(date, SEIKYUBI)
, CONVERT(date, SHIMEBI)
, CONVERT(date, URIBI)
, CONVERT(date, YOTEIBI)
, SEIKYUGAKU
, TAX
, sz.SEIKYUSUMI
, sz.SEIKYUZAN
, TAIGAI_AMT
, sz.KESHI_FLG
, SYONIN_F
, kkb.ID
, kkbbk.ID
, dkm.ID
, ckm.ID
, CONVERT(date, YOTEIBI_BK)
, CONVERT(datetime2(3), SIWAKEBI)
, CONVERT(datetime2(3), HAKKOUBI)
, COALESCE(SEIKYUNO, '') SEIKYUNO
, TAX_KUBUN
, COALESCE(s.BIKO , '')
, COALESCE(s.BIKO2, '')
, COALESCE(s.BIKO3, '')
, COALESCE(s.BIKO4, '')
, COALESCE(s.BIKO5, '')
, COALESCE(s.BIKO6, '')
, COALESCE(s.BIKO7, '')
, COALESCE(s.BIKO8, '')
, CONVERT(datetime2(3), sz.SAKUJYOBI)
, CONVERT(datetime2(3), KOUFURI_SAKUSEIBI)
, KOUFURI_KEKKA
, CONVERT(date, KOHURIYOTEIBI_BK)
, COALESCE(NYUKIN_YOTEI_KEY, '')
, SURYO
, TANKA
, TANNI
, KINGAKU
, COALESCE(tr.LoginUserID, 0)
, s.R_YMD
, COALESCE(tl.LoginUserID, 0)
, s.LR_YMD
, NULL
FROM [G3_Convert].[dbo].TBLSEIKYU s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLSEIKYUZAN sz
ON s.KAISYACD = sz.KAISYACD
AND s.CCY = sz.CCY
AND s.SEQNO = sz.SEQNO
AND s.GYONO = sz.GYONO
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY ccy
ON s.KAISYACD = ccy.KAISYACD
AND s.CCY = ccy.CCY
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON s.KAISYACD = tk.KAISYACD
AND s.TOKUCD = tk.TOKUCD
INNER JOIN [G3_Convert].[dbo].TBLBUMON b
ON s.KAISYACD = b.KAISYACD
AND s.URI_BUMON = b.BUMONCD
INNER JOIN [G3_Convert].[dbo].TBLTANTO t
ON s.KAISYACD = t.KAISYACD
AND s.TANCD = t.TANTOCD
INNER JOIN [G3_Convert].[dbo].TBLKUBUN skb
ON s.KAISYACD = skb.KAISYACD
AND s.SEIKYU_KBN = skb.KUBUNCD
AND skb.SIKIBETU = '1'
INNER JOIN [G3_Convert].[dbo].TBLKUBUN kkb
ON s.KAISYACD = kkb.KAISYACD
AND s.KAISYU_KBN = kkb.KUBUNCD
AND kkb.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLKUBUN kkbbk
ON s.KAISYACD = kkbbk.KAISYACD
AND s.KAISYU_KBN_BK = kkbbk.KUBUNCD
AND kkbbk.SIKIBETU = '3'
LEFT JOIN [G3_Convert].[dbo].TBLKAMOKU dkm
ON s.KAISYACD = dkm.KAISYACD
AND s.DB_CD = dkm.KAMOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLKAMOKU ckm
ON s.KAISYACD = ckm.KAISYACD
AND s.CR_CD = ckm.KAMOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  s.KAISYACD = tr.KAISYACD
AND s.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  s.KAISYACD = tl.KAISYACD
AND s.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Billing OFF;

INSERT INTO BillingMemo
SELECT s.ID, s.SEIKYU_MEMO
FROM [G3_Convert].[dbo].TBLSEIKYU s
WHERE s.SEIKYU_MEMO <> ''

SET IDENTITY_INSERT ImportFileLog ON;
INSERT INTO ImportFileLog ( 
  Id
, CompanyId
, [FileName]
, FileSize
, ReadCount
, ImportCount
, ImportAmount
, CreateBy
, CreateAt
)
SELECT 
  r.ID
, k.ID
, [FILE_NAME]
, CONVERT(decimal, REPLACE(REPLACE(REPLACE(FILE_SIZE, 'B', ''), 'K', ''), ' ', ''))
, KENSU
, KENSU
, KINGAKU
, COALESCE(tr.LoginUserID, 0)
, r.R_YMD
FROM [G3_Convert].[dbo].TBLEBFILE_RIREKI r
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON r.KAISYACD = k.KAISYACD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  r.KAISYACD = tr.KAISYACD
AND r.R_ID = tr.TANTOCD
SET IDENTITY_INSERT ImportFileLog OFF;

SET IDENTITY_INSERT ReceiptHeader ON;
INSERT INTO ReceiptHeader ( 
  Id
, CompanyId
, FileType
, CurrencyId
, ImportFileLogId
, AssignmentFlag
, ImportCount
, ImportAmount
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  e.ID
, k.ID
, 0
, ccy.ID
, r.ID
, KESHI_FLG
, COALESCE(INSCNT, 0)
, COALESCE(TOTKINGAKU, 0)
, COALESCE(tr.LoginUserID, 0)
, e.R_YMD
, COALESCE(tl.LoginUserID, 0)
, e.LR_YMD
FROM [G3_Convert].[dbo].TBLEBRIREKI e
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON e.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY ccy
ON e.KAISYACD = ccy.KAISYACD
AND e.CCY = ccy.CCY
INNER JOIN [G3_Convert].[dbo].TBLEBFILE_RIREKI r
ON e.KAISYACD = r.KAISYACD
AND e.FILE_TORIKOMI_SEQ = r.TORIKOMI_SEQ
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  e.KAISYACD = tr.KAISYACD
AND e.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  e.KAISYACD = tl.KAISYACD
AND e.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT ReceiptHeader OFF;

SET IDENTITY_INSERT Receipt ON;
INSERT INTO Receipt ( 
  Id
, CompanyId
, CurrencyId
, ReceiptHeaderId
, ReceiptCategoryId
, CustomerId
, SectionId
, InputType
, Apportioned
, Approved
, Workday
, RecordedAt
, OriginalRecordedAt
, ReceiptAmount
, AssignmentAmount
, RemainAmount
, AssignmentFlag
, PayerCode
, PayerName
, PayerNameRaw
, SourceBankName
, SourceBranchName
, OutputAt
, DueAt
, MailedAt
, OriginalReceiptId
, ExcludeFlag
, ExcludeCategoryId
, ExcludeAmount
, ReferenceNumber
, RecordNumber
, DensaiRegisterAt
, Note1
, Note2
, Note3
, Note4
, BillNumber
, BillBankCode
, BillBranchCode
, BillDrawAt
, BillDrawer
, DeleteAt
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
, ProcessingAt
, StaffId
, CollationKey
, BankCode
, BankName
, BranchCode
, BranchName
, AccountTypeId
, AccountNumber
, AccountName
)
SELECT 
  n.ID
, k.ID
, ccy.ID
, eb.ID
, nkb.ID
, tk.ID
, nb.ID
, n.DATAKBN
, n.FURIWAKE_FLG
, n.SYONIN_F
, CONVERT(date, n.SAKUSEIBI)
, CONVERT(date, n.NYUKINBI)
, CONVERT(date, n.NYUKINBI_BK)
, n.KINGAKU
, nz.HIKIATEGAKU
, nz.MIHIKIATEGAKU
, nz.KESHI_FLG
, COALESCE(n.IRAICD, '')
, COALESCE(n.IRAIMEI, '')
, COALESCE(n.IRAIMEI_FULL, '')
, COALESCE(n.SHIMU_GINKO, '')
, COALESCE(n.SHIMU_SHITEN, '')
, CONVERT(datetime2(3), n.SIWAKEBI)
, CONVERT(date, n.KIJITU)
, CONVERT(date, n.MAIL_HAISHINBI)
, nbk.ID
, n.TAIGAI_F
, tg.ID
, n.TAIGAI_AMT
, COALESCE(n.REF_NO, '')
, COALESCE(n.KIROKUNO, '')
, CONVERT(date, n.DENSHIKIROKUNENGAPPI)
, COALESCE(n.TEKIYO, '')
, COALESCE(n.BIKO2, '')
, COALESCE(n.BIKO3, '')
, COALESCE(n.BIKO4, '')
, COALESCE(n.TEGATA_NO, '')
, COALESCE(n.TEGATA_GINKOCD, '')
, COALESCE(n.TEGATA_SHITENCD, '')
, CONVERT(date, n.FURIDASHIBI)
, COALESCE(n.FURIDASHININ, '')
, CONVERT(datetime2(3), nz.SAKUJYOBI)
, COALESCE(tr.LoginUserID, 0)
, n.R_YMD
, COALESCE(tl.LoginUserID, 0)
, n.LR_YMD
, CONVERT(date, n.SYORI_YOTEIBI)
, t.StaffID
, COALESCE(n.SYOUGOU_NUMBER, '')
, COALESCE(n.GINKOCD, '')
, COALESCE(n.GINKOMEI, '')
, COALESCE(n.SHITENCD, '')
, COALESCE(n.SHITENMEI, '')
, n.SYUMOKU
, COALESCE(n.KOUZANO, '')
, COALESCE(n.KOUZAMEI, '')
FROM [G3_Convert].[dbo].TBLNYUKIN n
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON n.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY ccy
ON n.KAISYACD = ccy.KAISYACD
AND n.CCY = ccy.CCY
INNER JOIN [G3_Convert].[dbo].TBLNYUKINZAN nz
ON n.KAISYACD = nz.KAISYACD
AND n.CCY = nz.CCY
AND n.SEQNO = nz.SEQNO
AND n.GYONO = nz.GYONO
INNER JOIN [G3_Convert].[dbo].TBLKUBUN nkb
ON n.KAISYACD = nkb.KAISYACD
AND n.NYU_KBN = nkb.KUBUNCD
AND nkb.SIKIBETU = '2'
LEFT JOIN [G3_Convert].[dbo].TBLEBRIREKI eb
ON n.KAISYACD = eb.KAISYACD
AND n.GINKOCD = eb.GINKOCD
AND n.SHITENCD = eb.SHITENCD
AND n.SYUMOKU = eb.YOKINSYU
AND n.KOUZANO = eb.KOUZANO
AND n.SAKUSEIBI = eb.SAKUSEIBI
AND n.R_YMD = eb.R_YMD
LEFT JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON n.KAISYACD = tk.KAISYACD
AND n.TOKUCD = tk.TOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb
ON n.KAISYACD = nb.KAISYACD
AND n.NYUKINBUMONCD = nb.NYUKINBUMONCD
LEFT JOIN [G3_Convert].[dbo].TBLNYUKIN nbk
ON n.KAISYACD = nbk.KAISYACD
AND n.CCY = nbk.CCY
AND n.SEQNO_BK = nbk.SEQNO
AND n.GYONO_BK = nbk.GYONO
LEFT JOIN [G3_Convert].[dbo].TBLTANTO t
ON n.KAISYACD = t.KAISYACD
AND n.TANTOCD = t.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTAIGAI tg
ON n.KAISYACD = tg.KAISYACD
AND n.TAIGAI_KBN = tg.KUBUNCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  n.KAISYACD = tr.KAISYACD
AND n.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  n.KAISYACD = tl.KAISYACD
AND n.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Receipt OFF;

INSERT INTO ReceiptMemo
SELECT n.ID, n.NYUKIN_MEMO
FROM [G3_Convert].[dbo].TBLNYUKIN n
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON n.KAISYACD = k.KAISYACD
WHERE n.NYUKIN_MEMO <> ''

SET IDENTITY_INSERT ReceiptExclude ON;
INSERT INTO ReceiptExclude ( 
  Id
, ReceiptId
, ExcludeAmount
, ExcludeCategoryId
, OutputAt
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  nt.ID
, n.ID
, TAIGAI_KINGAKU
, tg.ID
, CONVERT(datetime2(3), SHIWAKEBI)
, COALESCE(tr.LoginUserID, 0)
, nt.R_YMD
, COALESCE(tl.LoginUserID, 0)
, nt.LR_YMD
FROM [G3_Convert].[dbo].TBLNYUKIN_TAISYOGAI nt
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON nt.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLNYUKIN n
ON nt.KAISYACD = n.KAISYACD
AND nt.CCY = n.CCY
AND nt.SEQNO = n.SEQNO
AND nt.GYONO = n.GYONO
INNER JOIN [G3_Convert].[dbo].TBLTAIGAI tg
ON nt.KAISYACD = tg.KAISYACD
AND nt.TAIGAI_KBN = tg.KUBUNCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  nt.KAISYACD = tr.KAISYACD
AND nt.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  nt.KAISYACD = tl.KAISYACD
AND nt.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT ReceiptExclude OFF;

INSERT INTO ReceiptSectionTransfer ( 
  SourceReceiptId
, DestinationReceiptId
, SourceSectionId
, DestinationSectionId
, SourceAmount
, DestinationAmount
, PrintFlag
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  n_f.ID
, n_t.ID
, nb_f.ID
, nb_t.ID
, KINGAKU_FROM
, KINGAKU_TO
, P_FLG
, COALESCE(tr.LoginUserID, 0)
, nbf.R_YMD
, COALESCE(tl.LoginUserID, 0)
, nbf.LR_YMD
FROM [G3_Convert].[dbo].TBLNYUKINBUMON_FURIKAE nbf
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON nbf.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLNYUKIN n_f
ON nbf.KAISYACD = n_f.KAISYACD
AND nbf.CCY = n_f.CCY
AND nbf.SEQNO_FROM = n_f.SEQNO
AND nbf.GYONO_FROM = n_f.GYONO
INNER JOIN [G3_Convert].[dbo].TBLNYUKIN n_t
ON nbf.KAISYACD = n_t.KAISYACD
AND nbf.CCY = n_t.CCY
AND nbf.SEQNO_TO = n_t.SEQNO
AND nbf.GYONO_TO = n_t.GYONO
INNER JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb_f
ON nbf.KAISYACD = nb_f.KAISYACD
AND nbf.NBUMON_FROM = nb_f.NYUKINBUMONCD
INNER JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb_t
ON nbf.KAISYACD = nb_t.KAISYACD
AND nbf.NBUMON_TO = nb_t.NYUKINBUMONCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  nbf.KAISYACD = tr.KAISYACD
AND nbf.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  nbf.KAISYACD = tl.KAISYACD
AND nbf.LR_ID = tl.TANTOCD

SET IDENTITY_INSERT Netting ON;
INSERT INTO Netting ( 
  Id
, CompanyId
, CurrencyId
, CustomerId
, ReceiptCategoryId
, SectionId
, ReceiptId
, RecordedAt
, DueAt
, Amount
, AssignmentFlag
, Note
, ReceiptMemo
)
SELECT 
  s.ID
, k.ID
, ccy.ID
, tk.ID
, nkb.ID
, nb.ID
, n.ID
, CONVERT(date, s.NYUKINBI)
, CONVERT(date, s.KIJITU)
, s.KINGAKU
, s.KESHI_FLG
, COALESCE(s.TEKIYO, '')
, COALESCE(s.NYUKIN_MEMO, '')
FROM [G3_Convert].[dbo].TBLNYUKINYOTEI_SOUSAI s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY ccy
ON s.KAISYACD = ccy.KAISYACD
AND s.CCY = ccy.CCY
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON s.KAISYACD = tk.KAISYACD
AND s.TOKUCD = tk.TOKUCD
INNER JOIN [G3_Convert].[dbo].TBLKUBUN nkb
ON s.KAISYACD = nkb.KAISYACD
AND s.NYU_KBN = nkb.KUBUNCD
AND nkb.SIKIBETU = '2'
LEFT JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb
ON s.KAISYACD = nb.KAISYACD
AND s.NYUKINBUMONCD = nb.NYUKINBUMONCD
LEFT JOIN [G3_Convert].[dbo].TBLNYUKIN n
ON s.KAISYACD = n.KAISYACD
AND s.CCY = n.CCY
AND s.NYU_SEQNO = n.SEQNO
AND s.NYU_GYONO = n.GYONO
SET IDENTITY_INSERT Netting OFF;

INSERT INTO AdvanceReceivedBackup ( 
  Id
, CompanyId
, CurrencyId
, ReceiptHeaderId
, ReceiptCategoryId
, CustomerId
, SectionId
, InputType
, Apportioned
, Approved
, Workday
, RecordedAt
, OriginalRecordedAt
, ReceiptAmount
, AssignmentAmount
, RemainAmount
, AssignmentFlag
, PayerCode
, PayerName
, PayerNameRaw
, SourceBankName
, SourceBranchName
, OutputAt
, DueAt
, MailedAt
, OriginalReceiptId
, ExcludeFlag
, ExcludeCategoryId
, ExcludeAmount
, ReferenceNumber
, RecordNumber
, DensaiRegisterAt
, Note1
, Note2
, Note3
, Note4
, BillNumber
, BillBankCode
, BillBranchCode
, BillDrawAt
, BillDrawer
, DeleteAt
, CreateBy
, CreateAt
, Memo
, TransferOutputAt
, StaffId
, CollationKey
, BankCode
, BankName
, BranchCode
, BranchName
, AccountTypeId
, AccountNumber
, AccountName
)
SELECT 
  bk.ID
, k.ID
, ccy.ID
, eb.ID
, nkb.ID
, tk.ID
, nb.ID
, bk.DATAKBN
, bk.FURIWAKE_FLG
, bk.SYONIN_F
, CONVERT(date, bk.SAKUSEIBI)
, CONVERT(date, bk.NYUKINBI)
, CONVERT(date, bk.NYUKINBI_BK)
, bk.KINGAKU
, HIKIATEGAKU
, MIHIKIATEGAKU
, bk.KESHI_FLG
, COALESCE(bk.IRAICD, '')
, COALESCE(bk.IRAIMEI, '')
, COALESCE(bk.IRAIMEI_FULL, '')
, COALESCE(bk.SHIMU_GINKO, '')
, COALESCE(bk.SHIMU_SHITEN, '')
, CONVERT(datetime2(3), bk.SIWAKEBI)
, CONVERT(date, bk.KIJITU)
, CONVERT(date, bk.MAIL_HAISHINBI)
, org.ID
, bk.TAIGAI_F
, bk.TAIGAI_KBN
, bk.TAIGAI_AMT
, COALESCE(bk.KIROKUNO, '')
, COALESCE(bk.REF_NO, '')
, COALESCE(bk.DENSHIKIROKUNENGAPPI, '')
, COALESCE(bk.TEKIYO, '')
, COALESCE(bk.BIKO2, '')
, COALESCE(bk.BIKO3, '')
, COALESCE(bk.BIKO4, '')
, COALESCE(bk.TEGATA_NO, '')
, COALESCE(bk.TEGATA_GINKOCD, '')
, COALESCE(bk.TEGATA_SHITENCD, '')
, CONVERT(date, bk.FURIDASHIBI)
, COALESCE(bk.FURIDASHININ, '')
, CONVERT(datetime2(3), bk.SAKUJYOBI)
, COALESCE(tr.LoginUserID, 0)
, bk.NYU_R_YMD
, COALESCE(bk.NYUKIN_MEMO, '')
, CONVERT(datetime2(3), bk.FURIKAE_SIWAKEBI)
, t.StaffID
, COALESCE(bk.SYOUGOU_NUMBER, '')
, COALESCE(bk.GINKOCD, '')
, COALESCE(bk.GINKOMEI, '')
, COALESCE(bk.SHITENCD, '')
, COALESCE(bk.SHITENMEI, '')
, bk.SYUMOKU
, COALESCE(bk.KOUZANO, '')
, COALESCE(bk.KOUZAMEI, '')
FROM [G3_Convert].[dbo].TBLMAEUKE_BACKUP bk
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON bk.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY ccy
ON bk.KAISYACD = ccy.KAISYACD
AND bk.CCY = ccy.CCY
INNER JOIN [G3_Convert].[dbo].TBLKUBUN nkb
ON bk.KAISYACD = nkb.KAISYACD
AND bk.NYU_KBN = nkb.KUBUNCD
AND nkb.SIKIBETU = '2'
INNER JOIN [G3_Convert].[dbo].TBLNYUKIN org
ON bk.KAISYACD = org.KAISYACD
AND bk.CCY = org.CCY
AND bk.SEQNO_BK = org.SEQNO
AND bk.GYONO_BK = org.GYONO
LEFT JOIN [G3_Convert].[dbo].TBLEBRIREKI eb
ON bk.KAISYACD = eb.KAISYACD
AND bk.CCY = eb.CCY
AND bk.GINKOCD = eb.GINKOCD
AND bk.SHITENCD = eb.SHITENCD
AND bk.SYUMOKU = eb.YOKINSYU
AND bk.KOUZANO = eb.KOUZANO
AND bk.SAKUSEIBI = eb.SAKUSEIBI
AND bk.NYU_R_YMD = eb.R_YMD
LEFT JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON bk.KAISYACD = tk.KAISYACD
AND bk.TOKUCD = tk.TOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLNYUKINBUMON nb
ON bk.KAISYACD = nb.KAISYACD
AND bk.NYUKINBUMONCD = nb.NYUKINBUMONCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO t
ON bk.KAISYACD = t.KAISYACD
AND bk.TANTOCD = t.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON bk.KAISYACD = tr.KAISYACD
AND bk.NYU_R_ID = tr.TANTOCD

SET IDENTITY_INSERT MatchingHeader ON;
INSERT INTO MatchingHeader ( 
  Id
, CompanyId
, CurrencyId
, CustomerId
, PaymentAgencyId
, Approved
, ReceiptCount
, BillingCount
, Amount
, BankTransferFee
, TaxDifference
, MatchingProcessType
, Memo
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  ks.MatchingHeaderID
, k.ID
, ccy.ID
, tk.ID
, kd.ID
, SYONIN_FLG
, ks.NYUKIN_KENSU
, ks.SEIKYU_KENSU
, ks.KESIKOMIGAKU
, ks.TESURYO
, ks.SHOUHIGOSA
, ks.RIYU
, ks.KESHI_MEMO
, ks.R_ID
, ks.R_YMD
, ks.LR_ID
, ks.LR_YMD
FROM (
SELECT
  ks.KAISYACD
, ks.MatchingHeaderID
, ks.CCY
, CASE WHEN MIN(kb.KESSAI_DAIKOUCD) IS NOT NULL THEN NULL ELSE COALESCE(MIN(sa.SAIKENCD), MIN(tk.TOKUCD)) END TOKUCD
, MIN(kb.KESSAI_DAIKOUCD) KESSAI_DAIKOUCD
, MAX(ks.SYONIN_FLG) SYONIN_FLG
, COUNT(distinct ks.SEQNO_NYUKIN * 100 + ks.GYO_NYUKIN) NYUKIN_KENSU
, COUNT(distinct ks.SEQNO_SEIKYU * 100 + ks.GYO_SEIKYU) SEIKYU_KENSU
, SUM(ks.KESIKOMIGAKU) KESIKOMIGAKU
, SUM(ks.TESURYO) TESURYO
, SUM(ks.SHOUHIGOSA) SHOUHIGOSA
, CASE WHEN MAX(ks.RIYU) LIKE '%一括消込%' THEN 0 ELSE 1 END RIYU
, COALESCE(MAX(km.KESHI_MEMO), '') KESHI_MEMO
, COALESCE(tr.LoginUserID, 0) R_ID
, MAX(ks.R_YMD) R_YMD
, COALESCE(tl.LoginUserID, 0) LR_ID
, MAX(ks.LR_YMD) LR_YMD
FROM [G3_Convert].[dbo].TBLKESHI ks
INNER JOIN [G3_Convert].[dbo].TBLSEIKYU s
ON ks.KAISYACD = s.KAISYACD
AND ks.CCY = s.CCY
AND ks.SEQNO_SEIKYU = s.SEQNO
AND ks.GYO_SEIKYU = s.GYONO
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON s.KAISYACD = tk.KAISYACD
AND s.TOKUCD = tk.TOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLSAIKEN sa
ON tk.KAISYACD = sa.KAISYACD
AND tk.TOKUCD = sa.TOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLKUBUN kb
ON s.KAISYACD = kb.KAISYACD
AND s.KAISYU_KBN = kb.KUBUNCD
AND kb.SIKIBETU = '3'
AND kb.KOUZA_FURIKAE = 1
LEFT JOIN [G3_Convert].[dbo].TBLKESHI_MEMO km
ON ks.KAISYACD = km.KAISYACD
ANd ks.CCY = km.CCY
AND ks.KESHIKOMIKEY = km.KESHIKOMIKEY
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  ks.KAISYACD = tr.KAISYACD
AND ks.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  ks.KAISYACD = tl.KAISYACD
AND ks.LR_ID = tl.TANTOCD
GROUP BY ks.KAISYACD, ks.MatchingHeaderID, ks.CCY, tr.LoginUserID, tl.LoginUserID
) ks
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON ks.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLCURRENCY ccy
ON ks.KAISYACD = ccy.KAISYACD
AND ks.CCY = ccy.CCY
LEFT JOIN [G3_Convert].[dbo].TBLTOKUI tk
ON ks.KAISYACD = tk.KAISYACD
AND ks.TOKUCD = tk.TOKUCD
LEFT JOIN [G3_Convert].[dbo].TBLKESSAI_DAIKOU kd
ON ks.KAISYACD = kd.KAISYACD
AND ks.KESSAI_DAIKOUCD = kd.KESSAI_DAIKOUCD
SET IDENTITY_INSERT MatchingHeader OFF;

SET IDENTITY_INSERT Matching ON;
INSERT INTO Matching ( 
  Id
, ReceiptId
, BillingId
, MatchingHeaderId
, BankTransferFee
, Amount
, BillingRemain
, ReceiptRemain
, AdvanceReceivedOccured
, RecordedAt
, TaxDifference
, OutputAt
, CreateBy
, CreateAt
, UpdateBy
, UpdateAt
)
SELECT 
  ks.ID
, n.ID
, s.ID
, ks.MatchingHeaderID
, TESURYO
, KESIKOMIGAKU
, SEIKYUZAN_LOG
, NYUKINZAN_LOG
, CASE NYUKINZAN_KBN WHEN '99' THEN 1 ELSE 0 END
, CONVERT(date, DENPYOUHIDUKE)
, SHOUHIGOSA
, CONVERT(datetime2(3), SHIWAKEBI)
, COALESCE(tr.LoginUserID, 0)
, ks.R_YMD
, COALESCE(tl.LoginUserID, 0)
, ks.LR_YMD
FROM [G3_Convert].[dbo].TBLKESHI ks
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON ks.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLNYUKIN n
ON ks.KAISYACD = n.KAISYACD
AND ks.CCY = n.CCY
AND ks.SEQNO_NYUKIN = n.SEQNO
AND ks.GYO_NYUKIN = n.GYONO
INNER JOIN [G3_Convert].[dbo].TBLSEIKYU s
ON ks.KAISYACD = s.KAISYACD
AND ks.CCY = s.CCY
AND ks.SEQNO_SEIKYU = s.SEQNO
AND ks.GYO_SEIKYU = s.GYONO
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON  ks.KAISYACD = tr.KAISYACD
AND ks.R_ID = tr.TANTOCD
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
ON  ks.KAISYACD = tl.KAISYACD
AND ks.LR_ID = tl.TANTOCD
SET IDENTITY_INSERT Matching OFF;

INSERT INTO MatchingOutputed ( 
  MatchingHeaderId
, OutputAt
)
SELECT 
  ks.MatchingHeaderID
, GETDATE()
FROM [G3_Convert].[dbo].TBL_P_KESHI p
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON p.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLKESHI ks
ON p.KAISYACD = ks.KAISYACD
AND p.CCY = ks.CCY
AND p.SEQNO_NYUKIN = ks.SEQNO_NYUKIN
AND p.GYO_NYUKIN = ks.GYO_NYUKIN
AND p.KAISU = ks.KAISU
AND p.SEQNO_SEIKYU = ks.SEQNO_SEIKYU
AND p.GYO_SEIKYU = ks.GYO_SEIKYU
GROUP BY ks.MatchingHeaderID

INSERT INTO BillingScheduledIncome
(
  BillingId
, MatchingId
, CreateBy
, CreateAt
)
SELECT 
  s.ID
, ks.ID
, COALESCE(tr.LoginUserID, 0)
, s.R_YMD
FROM [G3_Convert].[dbo].TBLSEIKYU s
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
ON s.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLKESHI ks
ON s.KAISYACD = ks.KAISYACD
AND s.CCY = ks.CCY
AND s.SEQNO_N = ks.SEQNO_NYUKIN
AND s.GYONO_N = ks.GYO_NYUKIN
AND s.KESHI_KAISU = ks.KAISU
AND s.SEQNO_BK = ks.SEQNO_SEIKYU
AND s.GYONO_BK = ks.GYO_SEIKYU
AND s.KESHIKOMIKEY = ks.KESHIKOMIKEY
LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
ON s.KAISYACD = tr.KAISYACD
AND s.R_ID = tr.TANTOCD

SET Identity_insert Destination ON
insert into Destination
(
 [Id]
,[CompanyId]
,[CustomerId]
,[Code]
,[PostalCode]
,[Address1]
,[Address2]
,[DepartmentName]
,[Addressee]
,[Honorific]
,[CreateBy]
,[CreateAt]
,[UpdateBy]
,[UpdateAt]
)
SELECT
 a.Id
,k.id
,tk.id
,a.[ADDRESSCD]
,a.[ZIPCD]
,a.[ADDRESS1]
,a.[ADDRESS2]
,a.[DEPARTMENT]
,a.[ADDRESSEE]
,a.[HONORIFIC]
, COALESCE(tr.LoginUserID, 0)
, a.R_YMD
, COALESCE(tl.LoginUserID, 0)
, a.LR_YMD
 from [G3_Convert].[dbo].TBLADDRESS a
INNER JOIN [G3_Convert].[dbo].TBLKAISYA k
   on a.KAISYACD = k.KAISYACD
INNER JOIN [G3_Convert].[dbo].TBLTOKUI tk
   ON a.KAISYACD = tk.KAISYACD
  AND a.TOKUCD   = tk.TOKUCD
 LEFT JOIN [G3_Convert].[dbo].TBLTANTO tr
   ON a.KAISYACD = tr.KAISYACD
  AND a.R_ID     = tr.TANTOCD
 LEFT JOIN [G3_Convert].[dbo].TBLTANTO tl
   ON a.KAISYACD = tl.KAISYACD
  AND a.LR_ID    = tl.TANTOCD
SET Identity_insert Destination OFF
