--USE ScarletBranch01
--GO

DECLARE
  @dummyCode     varchar(100) = '9999'
, @productKey    varchar(100) = 'DUMMY99'
, @licenseKey    varchar(100) = 'xu+ZQAMvSOH4NyfjVApQpZV4wuY5QljU'
--'password' hasu
, @passHash      varchar(100) = '5E884898DA2847151D0E56F8DC6292773603DD6AABBDD62A11EF721D1542D8'
, @loginUserCode varchar(100) = 'ADMIN'

IF 0 = ( SELECT COUNT(*)
           FROM Company c
          WHERE c.Code = @dummyCode
       )
BEGIN

--ダミー会社
INSERT INTO Company (
  [Code]
, [Name]
, [Kana]
, [PostalCode]
, [Address1]
, [Address2]
, [Tel]
, [Fax]
, [BankAccountName]
, [BankAccountKana]
, [BankName1]
, [BranchName1]
, [AccountType1]
, [AccountNumber1]
, [BankName2]
, [BranchName2]
, [AccountType2]
, [AccountNumber2]
, [BankName3]
, [BranchName3]
, [AccountType3]
, [AccountNumber3]
, [ProductKey]
, [ShowConfirmDialog]
, [PresetCodeSearchDialog]
, [ShowWarningDialog]
, [ClosingDay]
, [CreateBy]
, [CreateAt]
, [UpdateBy]
, [UpdateAt]
, [TransferAggregate]
, [AutoCloseProgressDialog]
)
SELECT
  @dummyCode  [Code]
, @dummyCode  [Name]
, @dummyCode  [Kana]
, ''          [PostalCode]
, ''          [Address1]
, ''          [Address2]
, ''          [Tel]
, ''          [Fax]
, ''          [BankAccountName]
, ''          [BankAccountKana]
, ''          [BankName1]
, ''          [BranchName1]
, ''          [AccountType1]
, ''          [AccountNumber1]
, ''          [BankName2]
, ''          [BranchName2]
, ''          [AccountType2]
, ''          [AccountNumber2]
, ''          [BankName3]
, ''          [BranchName3]
, ''          [AccountType3]
, ''          [AccountNumber3]
, @productKey [ProductKey]
, 0           [ShowConfirmDialog]
, 1           [PresetCodeSearchDialog]
, 0           [ShowWarningDialog]
, 99          [ClosingDay]
, 0           [CreateBy]
, GETDATE()   [CreateAt]
, 0           [UpdateBy]
, GETDATE()   [UpdateAt]
, 0           [TransferAggregate]
, 0           [AutoCloseProgressDialog]

DECLARE @CompanyId int = (SELECT Id From Company WHERE Code = @dummyCode)

--請求部門
INSERT INTO Department (
 [CompanyId]
,[Code]
,[Name]
,[StaffId]
,[Note]
,[CreateBy]
,[CreateAt]
,[UpdateBy]
,[UpdateAt]
)
SELECT
 @companyId [CompanyId]
,@dummyCode [Code]
,@dummyCode [Name]
,NULL       [StaffId]
,''         [Note]
,0          [CreateBy]
,GETDATE()  [CreateAt]
,0          [UpdateBy]
,GETDATE()  [UpdateAt]

DECLARE @departmentId int = (SELECT Id From Department WHERE Code = @dummyCode)

--ログインユーザー
INSERT INTO LoginUser (
 [CompanyId]
,[Code]
,[Name]
,[DepartmentId]
,[Mail]
,[MenuLevel]
,[FunctionLevel]
,[UseClient]
,[UseWebViewer]
,[AssignedStaffId]
,[StringValue1]
,[StringValue2]
,[StringValue3]
,[StringValue4]
,[StringValue5]
,[CreateBy]
,[CreateAt]
,[UpdateBy]
,[UpdateAt]
)
SELECT
 @CompanyId       [CompanyId]
,@loginuserCode   [Code]
,'システム管理者' [Name]
,@departmentId    [DepartmentId]
,''               [Mail]
,1                [MenuLevel]
,1                [FunctionLevel]
,1                [UseClient]
,1                [UseWebViewer]
,NULL             [AssignedStaffId]
,''               [StringValue1]
,''               [StringValue2]
,''               [StringValue3]
,''               [StringValue4]
,''               [StringValue5]
,0                [CreateBy]
,GETDATE()        [CreateAt]
,0                [UpdateBy]
,GETDATE()        [UpdateAt]

DECLARE @loginUserId int = (SELECT Id From LoginUser WHERE Code = @loginusercode AND CompanyId = @CompanyId)

--ユーザーパスワード
INSERT INTO LoginUserPassword (
 [LoginUserId]
,[PasswordHash]
,[UpdateAt]
,[PasswordHash0]
,[PasswordHash1]
,[PasswordHash2]
,[PasswordHash3]
,[PasswordHash4]
,[PasswordHash5]
,[PasswordHash6]
,[PasswordHash7]
,[PasswordHash8]
,[PasswordHash9]
)
SELECT
 @loginUserId [LoginUserId]
,@passHash    [PasswordHash]
,GETDATE()    [UpdateAt]
,''           [PasswordHash0]
,''           [PasswordHash1]
,''           [PasswordHash2]
,''           [PasswordHash3]
,''           [PasswordHash4]
,''           [PasswordHash5]
,''           [PasswordHash6]
,''           [PasswordHash7]
,''           [PasswordHash8]
,''           [PasswordHash9]

--画面権限
INSERT INTO MenuAuthority(
 [CompanyId]
,[MenuId]
,[AuthorityLevel]
,[Available]
,[CreateBy]
,[CreateAt]
,[UpdateBy]
,[UpdateAt]
)
SELECT
 @CompanyId [CompanyId]
,'PH0101'   [MenuId]
,1          [AuthorityLevel]
,1          [Available]
,0          [CreateBy]
,GETDATE()  [CreateAt]
,0          [UpdateBy]
,GETDATE()  [UpdateAt]

--ユーザーライセンス
INSERT INTO loginuserlicense
(
 [CompanyId]
,[LicenseKey]
)
select
 @CompanyId  [CompanyId]
,@licenseKey [LicenseKey]

--パスワードポリシー
INSERT INTO PasswordPolicy
(
 [CompanyId]
,[MinLength]
,[MaxLength]
,[UseAlphabet]
,[MinAlphabetUseCount]
,[UseNumber]
,[MinNumberUseCount]
,[UseSymbol]
,[MinSymbolUseCount]
,[SymbolType]
,[CaseSensitive]
,[MinSameCharacterRepeat]
,[ExpirationDays]
,[HistoryCount]
)
SELECT
 @companyId         [CompanyId]
,1                  [MinLength]
,15                 [MaxLength]
,1                  [UseAlphabet]
,0                  [MinAlphabetUseCount]
,1                  [UseNumber]
,0                  [MinNumberUseCount]
,1                  [UseSymbol]
,0                  [MinSymbolUseCount]
,'!#%+-*/$_~\;:@&?^'[SymbolType]
,1                  [CaseSensitive]
,0                  [MinSameCharacterRepeat]
,0                  [ExpirationDays]
,0                  [HistoryCount]

--アプリケーションコントロール
INSERT INTO ApplicationControl
(
 [CompanyId]
,[DepartmentCodeLength]
,[DepartmentCodeType]
,[SectionCodeLength]
,[SectionCodeType]
,[AccountTitleCodeLength]
,[AccountTitleCodeType]
,[CustomerCodeLength]
,[CustomerCodeType]
,[LoginUserCodeLength]
,[LoginUserCodeType]
,[StaffCodeLength]
,[StaffCodeType]
,[UseDepartment]
,[UseScheduledPayment]
,[UseReceiptSection]
,[UseAuthorization]
,[UseLongTermAdvanceReceived]
,[RegisterContractInAdvance]
,[UseCashOnDueDates]
,[UseDeclaredAmount]
,[UseDiscount]
,[UseForeignCurrency]
,[UseBillingFilter]
,[UseDistribution]
,[UseOperationLogging]
,[ApplicationEdition]
,[CreateBy]
,[CreateAt]
,[UpdateBy]
,[UpdateAt]
,[LimitAccessFolder]
,[RootPath])
SELECT
 @CompanyId [CompanyId]
,10         [DepartmentCodeLength]
,1          [DepartmentCodeType]
,10         [SectionCodeLength]
,1          [SectionCodeType]
,10         [AccountTitleCodeLength]
,1          [AccountTitleCodeType]
,10         [CustomerCodeLength]
,1          [CustomerCodeType]
,10         [LoginUserCodeLength]
,1          [LoginUserCodeType]
,10         [StaffCodeLength]
,1          [StaffCodeType]
,0          [UseDepartment]
,0          [UseScheduledPayment]
,0          [UseReceiptSection]
,0          [UseAuthorization]
,0          [UseLongTermAdvanceReceived]
,0          [RegisterContractInAdvance]
,0          [UseCashOnDueDates]
,0          [UseDeclaredAmount]
,0          [UseDiscount]
,0          [UseForeignCurrency]
,0          [UseBillingFilter]
,0          [UseDistribution]
,0          [UseOperationLogging]
,0          [ApplicationEdition]
,0          [CreateBy]
,GETDATE()  [CreateAt]
,0          [UpdateBy]
,GETDATE()  [UpdateAt]
,0          [LimitAccessFolder]
,''         [RootPath]
END