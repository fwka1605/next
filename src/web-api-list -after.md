# Rac.VOne.Web.Api 修正後の

ASP.NET Core MVC で作成した Web API の Controller / Method 一覧

*   Web API のメソッド集約方針

    *   単数系は 複数形に

        *   登録系を複数形にするかは要検討

        *   操作感を統一するのであれば、すべて複数形が望ましい

    *   引数が異なるだけのものは、model化

        *   他テーブルを参照するようなものも 内容をよく確認し、集約できるか確認

    *   あまりにもメソッド名が長いなどは、rename を検討

    *   すべて非同期で実装し同期処理を削除

        *   WCF のサービスをすべて `Task<T> *Async` へと変更する

    *   Web API 側のメソッド名は、suffix の `Async` はつけない

        *   Web API のメソッドはすべて非同期とするので自明となる

        *   `Web.Common` や `Data.QueryProcessors` の interface で定義するメソッド名などは `*Async` を付ける

rename

MasterImportData<T> - ImportData<T>

## master 系

|controller|type|method name|arguments|remarks|
|----------|----|-----------|---------|-------|
|AccountTitle|`IEnumerable<AccountTitle>`|GetItems|AccountTitleSearch option|Get/GetByCodes はこれを使用する|
|AccountTitle|AccountTitle|Save|AccountTitle accountTitle||
|AccountTitle|int|Delete|int accountTitleId||
|AccountTitle|ImportResultAccountTitle|Import|`MasterImportData<AccountTitle>` importItem|インポート処理 戻り値を genericsへ変更可能|
|AccountTitle|`IEnumerable<MasterData>`|GetImportItemsForCategory|MasterSearchOption option|int CompanyId, string[] Codes を指定<br>インポート上書き処理の事前検証用<br>区分に登録されているのに、code で連携されていないものを返す |
|AccountTitle|`IEnumerable<MasterData>`|GetImportItemsForCustomerDiscount|MasterSarechOption option|歩引きマスターの確認用|
|AccountTitle|`IEnumerable<MasterData>`|GetImportItemsForDebitBilling|MasterSearchOption option|請求借方科目 確認用|
|AccountTitle|`IEnumerable<MasterData>`|GetImportItemsForCreditBilling|MasterSearchOption option|請求貸方科目 確認用|
|ApplicationControl|ApplicationControl|Get|ApplicationControl applicationControl||取得 会社IDを指定|
|ApplicationControl|int|Save|ApplicationControl applicationControl|ログ保存するかどうかの変更 **メソッド名を変更する**|
|ApplicationControl|ApplicationControl|Add|ApplicationControl applicationControl|会社作成時の処理 **必要**|
|BankAccount|`IEnumerable<BankAccount>`|GetItems|BankAccountSearch option|Get/GetByCode/GetByBranchname はすべてこれを利用すること|
|BankAccount|BankAccount|Save|BankAccount account|登録・更新|
|BankAccount|int|Delete|int id|削除|
|BankAccount|bool|ExistCategory|int categoryId|区分ID の登録確認|
|BankAccount|bool|ExistSection|int sectionId|入金部門IDの登録確認|
|BankAccount|ImportResult|Import|`MasterImportData<BankAccount>` importItem||
|BankAccountType|`IEnumerable<BankAccountType>`|GetItems|none||
|BankBranch|`IEnumerable<BankBranch>`|GetItems|BankBranchSearch option|Get はこれを利用|
|BankBranch|int|Delete|BankBranch branch|BankBranch branch|
|BankBranch|BankBranch|Save|BanckBranch branch|新規登録用|
|BankBranch|ImportResult|Import|`MasterImportData<BankBranch>` importItem|インポート|
|BillingDivisionContract|`IEnumerable<BillingDivisionContract>`|GetItems|`BillingDivisionContractSearchOption` option|検索用のmodelを作成し、集約 GetByBillingIds, GetByCustomerIds, GetByContractNumber|
|BillingDivisionSetting|BillingDivisionSetting|Get|int companyId|取得|
|Category|Category|Save|Category category|登録|
|Category|int|Delete|int categoryId|削除|
|Category|`IEnumerable<Category>`|GetItems|CategorySearch option|集約先 Get/GetByCode/GetInvoiceCollectCategories|
|Category|bool|ExistAccountTitle|int accountTitleId|存在確認|
|Category|bool|ExistPaymentAgency|int paymentAgencyId|存在確認|
|ClosingSetting|ClosingSetting|Get|int companyId|取得|
|ClosingSetting|ClosingSetting|Save|ClosingSetting setting|登録|
|CollationSetting|CollationSetting|Get|int companyId|取得 matching order なども併せて実施して すべて設定されたオブジェクトを返す|
|CollationSetting|`IEnumerable<MatchingOrder>`|GetMatchingBillingOrder|int companyId||
|CollationSetting|`IEnumerable<MatchingOrder>`|GetMatchingReceiptOrder|int companyId||
|CollationSetting|`IEnumerable<CollationOrder>`|GetCollationOrder|int comapnyId||
|CollationSetting|CollationSetting|Save|CollationSetting setting|登録 billing/receipt matching order および collation order を設定した状態で連携する|
|ColumnNameSetting|ColumnNameSetting|Get|ColumnNameSetting setting|obsolete|
|ColumnNameSetting|`IEnumerable<ColumnNameSetting>`|GetItems|int companyId|引数変える|
|ColumnNameSetting|`IEnumerable<ColumnNameSetting>`|Save|`IEnumerable<ColumnNameSetting>` settings|登録 複数件まとめて登録できるようにする|
|Company|int|Delete|int companyId|削除|
|Company|int|DeleteLog|int companyId|ロゴのIDも必要では？ obsolete or 変更 |
|Company|int|DeleteLogs|`IEnumerable<CompanyLog>` logs|削除したいモノだけ削除できる|
|Company|Company|Get|int companyId|取得 集約先|
|Company|Company|GetByCode|string code|obsolete|
|Company|`IEnumerable<CompnayLogo>`|GetLogos|int companyId|
|Company|Company|Save|Company company|使ってない obsolete|
|Company|CompanyLog|SaveLogo|CompanyLogo log|obsolete|
|Company|`IEnumerable<CompanyLog>`|SaveLogos|`IEnumerable<CompanyLogo>` logos||
|Company|`IEnumerable<Company>`|GetItems|string name|コード検索で？？|
|Company|Company|Create|CompanySource source|web common へ実装を移す|
|ControlColor|ControlColor|Get|ControlColor color|CompanyId, LoginUserId を設定|
|ControlColor|ControlColor|Save|ControlColor color||
|ControlColor|int|Delete|ControlColor color|未実装|
|Currency|`IEnumerable<Currency>`|GetItems|CurrencySearch option|取得 配列 Get/GetByCode|
|Currency|Currency|Save|Currency currency|登録|
|Currency|int|Delete|int id|削除|
|Currency|ImportResult|Import|`MasterImportData<Currency>` importData|インポート|
|Currency|`IEnumerable<MasterData>`|GetImportItemsBilling|MasterSearchOption option||
|Currency|`IEnumerable<MasterData>`|GetImportItemsReceipt|MasterSearchOption option||
|Currency|`IEnumerable<MasterData>`|GetImportItemsNetting|MasterSearchOption option||
|CustomerDiscount|bool|ExistAccountTitle|int accountTitleId||
|CustomerDiscount|ImportResult|Import|`MasterImportData<CustomerDiscount>` importData||
|CustomerFee|`IEnumerable<CustomerFee>`|Get|CustomerFee fee|CustomerId, CurrencyId を指定して検索, GetForPrint/GetForExport の集約先|
|CustomerFee|`IEnumerable<CustomerFee>`|Save|CustomerFee[] fees|登録|
|CustomerFee|ImportResult|Import|`MasterImportData<CustomerFee>`|インポート|
|CustomerGroup|`IEnumerable<CusotmerGroup>`|Save|`MasterImportData<CustomerGroup>` importData|登録 登録と削除を同時に実行するため|
|CustomerGroup|`IEnumerable<CustomerGroup>`|GetItems|CustomerGroupSearchOption option|検索系の集約先 GetByParent, GetByChildId, GetPrintCustomerData など 検索用model を定義する|
|CustomerGroup|ImportResult|Import|MasterImportData<CustomerGroup> importData|インポート|
|CsutomerGroup|bool|ExistCustomer|int customerId|存在確認 親・子どちらも確認|
|CustomerGroup|bool|HasChild|int parentCustomerId|子得意先がいるかどうかの確認|
|CustomerGroup|int|GetUniqueGroupCount|int[] ids|親・子とも検索対象として、ユニークなグループの数を返す|
|Customer|Customer|Save|Customer customer|登録|
|Customer|`IEnumerable<Customer>`|SaveItems|Customer[] customers|登録　複数|
|Customer|`IEnumerable<Customer>`|GetItems|CustomerSearch option|集約先 GetParentItems, GetCustomerDetailsItems, GetByCode, GetItemsWith, GetByChildDetails などはすべてこれを利用する|
|Customer|`IEnumerable<Customer>`|GetCustomerGroup|int companyId, int parentId|obsolete GetItems へ集約|
|Customer|`IEnumerable<Customer>`|GetCustomerWithList|MasterSearchOption option|obsolete GetItems へ集約|
|Customer|Customer|GetTopCustomer|Customer customer|obsolete `IEnumerable<Customer>` GetItems へ集約|
|Customer|bool|ExistStaff|int staffId|担当者存在確認|
|Customer|bool|ExistCategory|int categoryId|区分存在確認|
|Customer|bool|ExistCompany|int companyId|会社存在確認|
|Customer|int|Delete|int customerId|削除|
|Customer|`IEnumerable<MasterData>`|GetImportItemsForCustomerGroupParent|MasterSearchOption option||
|Customer|`IEnumerable<MasterData>`|GetImportItemsForCustomerGroupChild|MasterSearchOption option||
|Customer|`IEnumerable<MasterData>`|GetImportItemsForBilling|MasterSearchOption option||
|Customer|`IEnumerable<MasterData>`|GetImportItemsForReceipt|MasterSearchOption option||
|Customer|`IEnumerable<MasterData>`|GetImportItemsForNetting|MasterSearchOption option||
|Customer|ImportResult|Import|`MasterImportData<Customer>` importData||
|Customer|`IEnumerable<CustomerMin>`|GetMinItems|int companyId|パフォーマンス対応用 得意先最小モデル|
|Customer|CusotmerPaymentContract|SavePaymentContract|CustomerPaymentContract contract|約定条件（別controllerへ？）|
|Customer|`IEnumerable<CustomerPaymentContract>`|GetPaymentContract|int[] customerIds||
|Customer|int|DeletePaymentContract|int cusotmerId|約定条件だけ削除|
|Customer|CustomerDiscount|SaveDiscount|CustomerDiscount discount|登録|
|Customer|int|DeleteDiscount|int customerId, int sequesnce|削除|
|Customer|`IEnumerable<CustomerDiscount>`|GetDiscountItems|CustomerSearch option|歩引き配列取得 GetDiscount は、これを利用する|
|CustomerPaymentContract|bool|ExistCategory|int categoryId||
|Department|int|Delete|int id||
|Department|`IEnumerable<Department>`|GetItems|DepartmentSearchOption option|Get, GetByCode, GetByCodeAndStaff, GetDepartmentAndStaff すべて集約|
|Department|`IEnumerable<Department>`|DepartmentWithSection|int companyId, int sectionId, int[] departmentIds||
|Department|`IEnumerable<Department>`|GetWithoutSection|int companyId, int sectionId||
|Department|`IEnumerable<Department>`|GetByLoginUserId|int companyId, int loginUserId||
|Department|ImportResult|Import|`MasterImportData<Department>` importData||
|Department|`IEnumerable<MasterData>`|GetImportItemsStaff|MasterSearchOption option||
|Department|`IEnumerable<MasterData>`|GetImportItemsSectionWithDepartment|MasterSearchOption option||
|Department|`IEnumerable<MasterData>`|GetImportItemsBilling|MasterSearchOption option||
|Destination|`IEnumerable<Destination>`|GetItems|DestinationSearch option||
|Destination|Destination|Save|Destination destination||
|Destination|int|Delete|int destinationId||
|EBExcludeAccountSetting|`IEnumerable<EBExcludeAccountSetting>`|GetItems|int companyId||
|EBExcludeAccountSetting|EBExcludeAccountSetting|Save|EBExcludeAccountSetting setting||
|EBExcludeAccountSetting|int|Delete|EBExcludeAccountSetting setting||
|EBFileSetting|int|Delete|int id||
|EBFileSetting|`IEnumerable<EBFileSetting>`|GetItems|int companyId|GetItem を集約する|
|EBFileSetting|EBFileSetting|Save|EBFileSetting setting||
|EBFileSetting|int|UpdateIsUseable|int companyId, int loginUserId, int[] ids||
|EBFormat|`IEnumerable<EBFormat>`|GetItems|||
|ExportFieldSetting|`IEnumerable<ExportFieldSetting>`|GetItems|ExportFieldSetting setting||
|ExportFieldSetting|`IEnumerable<ExportFieldSetting>`|Save|ExportFieldSetting[] settings||
|FunctionAuthority|`IEnumerable<FunctionAuthority>`|GetItems|int companyId|GetByLoginUser を集約する|
|FunctionAuthority|`IEnumerable<FunctionAuthority>`|Save|FunctionAuthority[] functionAuthorities||
|GeneralSetting|`IEnumerable<GeneralSetting>`|GetItems|int companyId||
|GeneralSetting|GeneralSetting|Save|GeneralSetting setting||
|GeneralSetting|GeneralSetting|GetByCode|GeneralSetting setting||
|GridSetting|`IEnumerable<GridSetting>`|Save|GridSetting[] settings||
|GridSetting|`IEnumerable<GridSetting>`|GetItems|int companyId, int loginUserId, int? gridId||
|GridSetting|`IEnumerable<GridSetting>`|GetDefaultItems|int companyId, int loginUserId, int? gridId||
|HolidayCalendar|int|Delete|HolidayCalendar[] holidays||
|HolidayCalendar|`IEnumerable<HolidayCalendar>`|GetItems|HolidayCalendarSearch option||
|HolidayCalendar|HolidayCalendar|Save|HolidayCalendar holiday||
|HolidayCalendar|ImportResult|Import|`MasterImportData<HolidayCalendar>` importData||
|IgnoreKana|`IEnumerable<IgnoreKana>`|GetItems|int companyId||
|IgnoreKana|IgnoreKana|Save|IgnoreKana kana||
|IgnoreKana|int|Delete|IgnoreKana kana||
|IgnoreKana|ImportResult|Import|`MasterImportData<IgnoreKana>` importData||
|IgnoreKana|bool|ExistCategory|int id||
|IgnoreKana|IgnoreKana|Get|IgnoreKana kana||
|ImportSetting|ImportSetting|Get|ImportSetting setting||
|ImportSetting|`IEnumerable<ImportSetting>`|GetItems|int companyId||
|ImportSetting|int|Save|ImportSetting[] settings||
|InputControl|`IEnumerable<InputControl>`|Get|InputControl control||
|InputControl|`IEnumerable<InputControl>`|Save|InputControl[] inputControls||
|JuridicalPersonality|int|Delete|JuridicalPersonality personality||
|JuridicalPersonality|JuridicalPersonality|Save|JuridicalPersonality personality||
|JuridicalPersonality|`IEnumerable<JuridicalPersonality>`|GetItems|int companyId|Get|
|JuridicalPersonality|ImportResult|Import|`MasterImportData<JuridicalPersonality>` importData||
|KanaHistoryCustomer|bool|ExistCustomer|int customerId||
|KanaHistoryCustomer|`IEnumerable<KanaHistoryCustomer>`|GetList|int companyId, string payerName, string customerCodeFrom, string customerCodeTo||
|KanaHistoryCustomer|int|Delete|KanaHistoryCustomer history||
|KanaHistoryCustomer|ImportResult|Import|`MasterImportData<KanaHistoryCustomer>` importData||
|KanaHistoryPaymentAgency|`IEnumerable<KanaHistoryPaymentAgency>`|GetList|int companyId, string payerName, string paymentAgencyCodeFrom, string paymentAgencyCodeTo||
|KanaHistoryPaymentAgency|int|Delete|KanaHistoryPaymentAgency history||
|KanaHistoryPaymentAgency|ImportResult|Import|`MasterImportData<KanaHistoryPaymentAgency>` importData||
|LoginUserLicense|`IEnumerable<LoginUserLicense>`|GetItems|int companyId||
|LoginUserLicense|`IEnumerable<LoginUserLicense>`|Save|LoginUserLicense[] licenses||
|LoginUser|`IEnumerable<LoginUser>`|Get|int[] ids||
|LoginUser|`IEnumerable<LoginUser>`|GetItems|LoginUserSearch option||
|LoginUser|int|ResetPassword|int Id||
|LoginUser|int|Delete|int Id||
|LoginUser|`IEnumerable<LoginUser>`|GetByCode|MasterSearchOption option||
|LoginUser|`IEnumerable<LoginUser>`|GetItemsForGridLoader|int companyId||
|LoginUser|`IEnumerable<LoginUser>`|GetImportItemsForSection|MasterSearchOption option||
|LoginUser|LoginUser|Save|LoginUser loginUser||
|LoginUser|bool|ExitStaff|int staffId||
|LoginUser|ImportResultLoginUser|Import|`MasterImportData<LoginUser>` importData||
|LoginUserPassword|PasswordChangeResult|Change|int CompanyId, int LoginUserId, string OldPassword, string NewPassword||
|LoginUserPassword|LoginResult|Login|int CompanyId, int LoginUserId, string Password||
|LoginUserPassword|PasswordChangeResult|Save|int CompanyId, int LoginUserId, string Password||
|LongTermAdvanceReceivedContract|int|Delete|int id||
|LongTermAdvanceReceivedContract|LongTermAdvanceReceivedContract|Get|int id||
|LongTermAdvanceReceivedContract|LongTermAdvanceReceivedContract|GetByCode|int id, string code||
|LongTermAdvanceReceivedContract|`IEnumerable<LongTermAdvanceReceivedContract>`|GetItems|int companyId||
|LongTermAdvanceReceivedContract|LongTermAdvanceReceivedContract|Save|LongTermAdvanceReceivedContract contract||
|ImportSetting|int|Delete|int companyId||
|ImportSetting|MasterImportSetting|Get|int companyId||
|ImportSetting|MasterImportSetting|Save|MasterImportSetting setting||
|MenuAuthority|int|Delete|int companyId||
|MenuAuthority|`IEnumerable<MenuAuthority>`|GetItems|int? companyId, int? loginUserId||
|MenuAuthority|`IEnumerable<MenuAuthority>`|Save|MenuAuthority[] menuAuthorities||
|PasswordPolicy|PasswordPolicy|Get|int companyId||
|PasswordPolicy|PasswordPolicy|Save|PasswordPolicy policy||
|PaymentAgencyFee|`IEnumerable<PaymentAgencyFee>`|Get|int PaymentAgencyId, int CurrencyId||
|PaymentAgencyFee|`IEnumerable<PaymentAgencyFee>`|GetForExport|int companyId||
|PaymentAgencyFee|`IEnumerable<PaymentAgencyFee>`|Save|PaymentAgencyFee[] fees||
|PaymentAgency|`IEnumerable<PaymentAgency>`|GetItems|int companyId||
|PaymentAgency|`IEnumerable<PaymentAgency>`|GetByCode|MasterSearchOption option||
|PaymentAgency|`IEnumerable<PaymentAgency>`|Get|int[] ids||
|PaymentAgency|PaymentAgency|Save|PaymentAgency agency||
|PaymentAgency|int|Delete|int id||
|PdfOutputSetting|PdfOutputSetting|Get|PdfOutputSetting setting||
|PdfOutputSetting|PdfOutputSetting|Save|PdfOutputSetting setting||
|PeriodicBillingSetting|int|Delete|long id||
|PeriodicBillingSetting|`IEnumerable<PeriodicBillingSetting>`|GetItems|PeriodicBillingSettingSearch option||
|PeriodicBillingSetting|PeriodicBillingSetting|Save|PeriodicBillingSetting setting||
|ReportSetting|`IEnumerable<ReportSetting>`|GetItems|ReportSetting setting||
|ReportSetting|`IEnumerable<ReportSetting>`|Save|ReportSetting[] settings||
|Section|Section|Save|Section section||
|Section|int|Delete|int id||
|Section|`IEnumerable<Section>`|GetItems|SectionSearch option|GetByLoginUserId, GetByCode, GetimportItemsForSection, GetAsync, GetByCustomerId ほぼまとめる|
|Section|ImportResultSection|Import|`MasterImportData<Section>` importData||
|Section|`IEnumerable<MasterData>`|GetImportItemsForBankAccount|MasterSearchOption option||
|Section|`IEnumerable<MasterData>`|GetImportItemsForSectionWithDepartment|MasterSearchOption option||
|Section|`IEnumerable<MasterData>`|GetImportItemsForSectionWithLoginUser|MasterSearchOption option||
|Section|`IEnumerable<MasterData>`|GetImportItemsForReceipt|MasterSearchOption option||
|Section|`IEnumerable<MasterData>`|GetImportItemsForNetting|MasterSearchOption option||
|SectionWithDepartment|`IEnumerable<SectionWithDepartment>`|Save|`MasterImportData<SectionWithDepartment>` data||
|SectionWithDepartment|SectionWithDepartment|GetByDepartment|int companyId, int departmentId||
|SectionWithDepartment|`IEnumerable<SectionWithDepartment>`|GetBySection|int companyId, int sectionId||
|SectionWithDepartment|`IEnumerable<SectionWithDepartment>`|GetItems|SectionWithDepartmentSearch option||
|SectionWithDepartment|ImportResult|Import|`MasterImportData<SectionWithDepartment>` importData||
|SectionWithDepartment|bool|ExistSection|int sectionId||
|SectionWithDepartment|bool|ExistDepartment|int departmentId||
|SectionWithLoginUser|`IEnumerable<SectionWithLoginUser>`|Save|`MasterImportData<SectionWithLoginUser>` data||
|SectionWithLoginUser|`IEnumerable<SectionWithLoginUser>`|GetByLoginUser|int CompanyId, int LoginUserId||
|SectionWithLoginUser|`IEnumerable<SectionWithLoginUser>`|GetItems|SectionWithLoginUserSearch option||
|SectionWithLoginUser|ImportResultSectionWithLoginUser|Import|`MasterImportData<SectionWithLoginUser>` importData||
|SectionWithLoginUser|bool|ExistLoginUser|int loginUserId||
|SectionWithLoginUser|bool|ExistSection|int sectionId||
|Setting|`IEnumerable<Setting>`|GetItems|string[] ItemIds||
|Staff|Staff|Save|Staff staff||
|Staff|int|Delete|int id||
|Staff|`IEnumerable<Staff>`|GetItems|StaffSearch option||
|Staff|`IEnumerable<Staff>`|Get|int[] ids||
|Staff|`IEnumerable<Staff>`|GetByCode|MasterSearchOption option||
|Staff|bool|ExistDepartment|int departmentId||
|Staff|ImportResultStaff|Import|`MasterImportData<Staff>` importData||
|Staff|`IEnumerable<MasterData>`|GetImportItemsLoginUser|MasterSearchOption option||
|Staff|`IEnumerable<MasterData>`|GetImportItemsCustomer|MasterSearchOption option||
|Staff|`IEnumerable<MasterData>`|GetImportItemsBilling|MasterSearchOption option||
|Status|Status|GetStatusByCode|Status status||
|Status|`IEnumerable<Status>`|GetStatusesByStatusType|Status status||
|Status|Status|Save|Status Status||
|Status|int|Delete|int id||
|Status|bool|ExistReminder|int statusId||
|Status|bool|ExistReminderHistory|int statusId||
|TaskSchedule|`IEnumerable<TaskSchedule>`|GetItems|int companyId||
|TaskSchedule|TaskSchedule|Save|TaskSchedule TaskSchedule||
|TaskSchedule|int|Delete|TaskSchedule schedule||
|TaskSchedule|bool|Exists|TaskSchedule schedule||
|TaxClass|`IEnumerable<TaxClass>`|GetItems|||
|WebApiSetting|int|SaveAsync|WebApiSetting setting||
|WebApiSetting|int|DeleteAsync|WebApiSetting setting||
|WebApiSetting|WebApiSetting|GetByIdAsync|WebApiSetting setting||


## transaction 系

|controller|type|method name|arguments|remarks|
|----------|----|-----------|---------|-------|
|AccountTransfer|int|Cancel|AccountTransferLog[] logs||
|AccountTransfer|`IEnumerable<AccountTransferDetail>`|Extract|AccountTransferSearch option||
|AccountTransfer|`IEnumerable<AccountTransferLog>`|Get|int CompanyId||
|AccountTransfer|`IEnumerable<AccountTransferDetail>`|Save|AccountTransferDetail[] details||
|AdvanceReceivedBackup|AdvanceReceivedBackup|GetByOriginalReceiptId|long OriginalReceiptId||
|AdvanceReceivedBackup|`IEnumerable<AdvanceReceivedBackup>`|GetByIds|long[] ids||
|BillingAgingList|`IEnumerable<BillingAgingList>`|GetAsync|BillingAgingListSearch option||
|BillingAgingList|`IEnumerable<BillingAgingListDetail>`|GetDetailsAsync|BillingAgingListSearch option||
|Billing|`IEnumerable<Billing>`|Save|Billing[] billings||
|Billing|`IEnumerable<Billing>`|SaveForInput|Billing[] billings||
|Billing|int|SaveDiscount|BillingDiscount discount||
|Billing|int|SaveMemo|BillingMemo memo||
|Billing|`IEnumerable<Billing>`|Get|long[] ids||
|Billing|`IEnumerable<BillingDiscount>`|GetDiscount|long billingId||
|Billing|`IEnumerable<Billing>`|GetItems|BillingSearch option||
|Billing|[string|GetMemo|long id||
|Billing|bool|ExistAccountTitle|int accountTitleId||
|Billing|bool|ExistCollectCategory|int categoryId||
|Billing|bool|ExistCustomer|int customerId||
|Billing|bool|ExistBillingCategory|int categoryId||
|Billing|bool|ExistStaff|int staffId||
|Billing|bool|ExistCurrency|int currencyId||
|Billing|bool|ExistDepartment|int departmentId||
|Billing|bool|ExistDestination|int destinationId||
|Billing|int|Omit|int doDelete, int loginUserId, Transaction[] transactions||
|Billing|`IEnumerable<Billing>`|InputScheduledPayment|Billing[] billings||
|Billing|int|Delete|long[] Id, int UseLongTermAdvanceReceived, int RegisterContractInAdvance, int UseDiscount, int LoginUserId||
|Billing|int|DeleteByInputId|long InputId, int UseLongTermAdvanceReceived, int RegisterContractInAdvance, int UseDiscount, int LoginUserId||
|Billing|int|DeleteDiscount|long billingId||
|Billing|int|DeleteMemo|long Id||
|Billing|`IEnumerable<BillingDueAtModify>`|GetDueAtModifyItems|BillingSearch option||
|Billing|`IEnumerable<Billing>`|UpdateDueAt|BillingDueAtModify[] billings||
|Billing|`IEnumerable<ScheduledPaymentImport>`|ImportScheduledPayment|int CompanyId, int LoginUserId, int ImporterSettingId, ScheduledPaymentImport[] SchedulePayment||
|Billing|`IEnumerable<Billing>`|GetItemsForScheduledPaymentImport|int CompanyId, ScheduledPaymentImport[] SchedulePayment, ImporterSettingDetail[] details||
|Billing|BillingImportResult|Import|int CompanyId, int LoginUserId, int ImporterSettingId, BillingImport[] billingImport||
|Billing|`IEnumerable<int>`|BillingImportDuplicationCheck|int CompanyId, BillingImportDuplicationWithCode[] BillingImportDuplication, ImporterSettingDetail[] ImporterSettingDetail||
|Billing|`IEnumerable<Billing>`|GetAccountTransferMatchingTargetList|int companyId, int paymentAgencyId, DateTime transferDate||
|Billing|`IEnumerable<Billing>`|ImportAccountTransferResult|AccountTransferImportData[] importDataList, int? dueDateOffset, int? collectCategoryId||
|Billing|`IEnumerable<Billing>`|UpdateOutputAt|int CompanyId, DateTime? BilledAtFrom, DateTime? BilledAtTo, int CurrencyId, int LoginUserId||
|Billing|`IEnumerable<Billing>`|CancelBillingJournalizing|int CompanyId, int CurrencyId, DateTime[] OutputAt, int LoginUserId||
|Billing|`IEnumerable<JournalizingSummary>`|GetBillingJournalizingSummary|int OutputFlg, int CompanyId, int CurrencyId, DateTime? BilledAtFrom, DateTime? BilledAtTo||
|Billing|`IEnumerable<BillingJournalizing>`|ExtractBillingJournalizing|int CompanyId, DateTime? BilledAtFrom, DateTime? BilledAtTo, int CurrencyId, DateTime[] OutputAt||
|BillingInvoice|`IEnumerable<BillingInvoice>`|GetAsync|BillingInvoiceSearch option||
|BillingInvoice|BillingInputResult|PublishInvoicesAsync|string connectionId, BillingInvoiceForPublish[] invoices, int LoginUserId||
|BillingInvoice|`IEnumerable<BillingInvoiceDetailForPrint>`|GetDetailsForPrint|BillingInvoiceDetailSearch option||
|BillingInvoice|int|GetCountAsync|BillingInvoiceSearch searchOption, string connectionId||
|BillingInvoice|int|DeleteWorkTable|byte[] clientKey||
|BillingInvoice|int|CancelPublishAsync|string connectionId, long[] BilinngInputIds||
|BillingInvoice|`IEnumerable<BillingInvoiceDetailForExport>`|GetDetailsForExportAsync|long[] BillingInputIds, int CompanyId, string connectionId||
|BillingInvoice|int|UpdatePublishAtAsync|string connectionId, long[] BilinngInputIds||
|Closing|ClosingInformation|GetClosingInformation|int companyId||
|Closing|`IEnumerable<ClosingHistory>`|GetClosingHistory|int companyId||
|Closing|Closing|Save|Closing closing||
|Closing|int|Delete|int companyId||
|CollectionSchedule|`IEnumerable<CollectionSchedule>`|GetAsync|CollectionScheduleSearch option||
|CreditAgingList|`IEnumerable<CreditAgingList>`|GetAsync|CreditAgingListSearch option||
|CustomerLedger|`IEnumerable<CustomerLedger>`|GetAsync|CustomerLedgerSearch option||
|DataMaintenance|int|DeleteData|DateTime deleteDate||
|HatarakuDBJournalizing|`IEnumerable<JournalizingSummary>`|GetSummaryAsync|JournalizingOption option||
|HatarakuDBJournalizing|`IEnumerable<HatarakuDBData>`|ExtractAsync|JournalizingOption option||
|HatarakuDBJournalizing|int|UpdateAsync|JournalizingOption option||
|HatarakuDBJournalizing|int|CancelAsync|JournalizingOption option||
|ImporterSetting|`IEnumerable<ImporterSetting>`|GetHeader|ImporterSetting setting||
|ImporterSetting|ImporterSettingAndDetailResult|Save|ImporterSetting setting||
|ImporterSetting|ImporterSetting|GetHeaderByCode|ImporterSetting setting||
|ImporterSetting|`IEnumerable<ImporterSettingDetail>`|GetDetailByCode|ImporterSetting setting||
|ImporterSetting|`IEnumerable<ImporterSettingDetail>`|GetDetailById|int importerSettingId||
|ImporterSetting|int|Delete|int importerSettingId||
|ImportFileLog|`IEnumerable<ImportFileLog>`|GetHistory|int comapnyId||
|ImportFileLog|`IEnumerable<ImportFileLog>`|SaveImportFileLog|ImportFileLog[] logs||
|ImportFileLog|int|DeleteItems|int[] ids||
|InvoiceSetting|InvoiceCommonSetting|GetInvoiceCommonSetting|int companyId||
|InvoiceSetting|InvoiceCommonSetting|SaveInvoiceCommonSetting|InvoiceCommonSetting setting||
|InvoiceSetting|`IEnumerable<Category>`|UpdateCollectCategory|`IEnumerable<Category>` CollectCategories||
|InvoiceSetting|`IEnumerable<InvoiceNumberHistory>`|GetInvoiceNumberHistories|int companyId||
|InvoiceSetting|InvoiceNumberHistory|SaveInvoiceNumberHistory|InvoiceNumberHistory history||
|InvoiceSetting|int|DeleteInvoiceNumberHistories|int companyId||
|InvoiceSetting|InvoiceNumberSetting|GetInvoiceNumberSetting|int companyId||
|InvoiceSetting|InvoiceNumberSetting|SaveInvoiceNumberSetting|InvoiceNumberSetting setting||
|InvoiceSetting|bool|ExistCollectCategoryAtTemplate|int collectCategoryId||
|InvoiceSetting|InvoiceTemplateSetting|GetInvoiceTemplateSettingByCode|InvoiceTemplateSetting setting||
|InvoiceSetting|`IEnumerable<InvoiceTemplateSetting>`|GetInvoiceTemplateSettings|int companyId||
|InvoiceSetting|InvoiceTemplateSetting|SaveInvoiceTemplateSetting|InvoiceTemplateSetting setting||
|InvoiceSetting|int|DeleteInvoiceTemplateSetting|int id||
|LogData|`IEnumerable<LogData>`|GetItems|int CompanyId, DateTime? LoggedAtFrom, DateTime? LoggedAtTo, string LoginUserCode||
|LogData|int|Log|LogData log||
|LogData|LogData|GetStats|int companyId||
|LogData|int|DeleteAll|int companyId||
|Login|`IEnumerable<Company>`|GetCompanies|||
|Login|[string|Login|LoginParameters parameters||
|Logs|int|SaveErrorLog|Logs log||
|Matching|`IEnumerable<Collation>`|CollateAsync|CollationSearch option||
|Matching|MatchingResult|SequentialMatchingAsync|Collation[] Collations, CollationSearch option||
|Matching|`IEnumerable<int>`|SimulateAsync|Billing[] MatchingBilling, System.Decimal SearchValue||
|Matching|MatchingResult|MatchingIndividuallyAsync|MatchingSource source||
|Matching|`IEnumerable<MatchingHeader>`|SearchMatchedDataAsync|CollationSearch option||
|Matching|MatchingResult|CancelMatchingAsync|MatchingHeader[] MatchingHeader, int LoginUserId, string connectionId||
|Matching|`IEnumerable<Billing>`|SearchBillingData|MatchingBillingSearch option||
|Matching|`IEnumerable<Receipt>`|SearchReceiptData|MatchingReceiptSearch option||
|Matching|MatchingHeadersResult|Approve|MatchingHeader[] headers||
|Matching|MatchingHeadersResult|CancelApproval|MatchingHeader[] headers||
|Matching|`IEnumerable<Receipt>`|SearchReceiptById|long[] ids||
|Matching|CountResult|SaveWorkDepartmentTargetAsync|int CompanyId, byte[] ClientKey, int[] DepartmentIds||
|Matching|int|SaveWorkSectionTargetAsync|int CompanyId, byte[] ClientKey, int[] SectionIds||
|Matching|`IEnumerable<Matching>`|Get|long[] ids||
|Matching|`IEnumerable<MatchingHeader>`|GetHeaderItems|long[] ids||
|Matching|`IEnumerable<MatchingJournalizingDetail>`|GetMatchingJournalizingDetailAsync|JournalizingOption option||
|Matching|MatchingJournalizingProcessResult|CancelMatchingJournalizingDetailAsync|MatchingJournalizingDetail[] details||
|Matching|`IEnumerable<JournalizingSummary>`|GetMatchingJournalizingSummaryAsync|JournalizingOption option||
|Matching|`IEnumerable<MatchingJournalizing>`|ExtractMatchingJournalizingAsync|JournalizingOption option||
|Matching|`IEnumerable<GeneralJournalizing>`|ExtractGeneralJournalizingAsync|JournalizingOption option||
|Matching|`IEnumerable<MatchedReceipt>`|GetMatchedReceiptAsync|JournalizingOption option||
|Matching|bool|UpdateOutputAtAsync|JournalizingOption option||
|Matching|bool|CancelMatchingJournalizingAsync|JournalizingOption option||
|Matching|`IEnumerable<MatchingJournalizing>`|MFExtractMatchingJournalizingAsync|JournalizingOption option||
|MatchingHistory|`IEnumerable<MatchingHistory>`|GetAsync|MatchingHistorySearch option||
|MatchingHistory|bool|SaveOutputAtAsync|long[] ids||
|MFBilling|`IEnumerable<MFBilling>`|GetItems|int companyId||
|MFBilling|`IEnumerable<MFBilling>`|GetByBillingIds|`IEnumerable<long>` ids, bool isMatched||
|Netting|bool|ExistReceiptCategory|int categoryId||
|Netting|bool|ExistCustomer|int customerId||
|Netting|`IEnumerable<Netting>`|GetItems|Netting netting||
|Netting|`IEnumerable<Netting>`|Save|Netting[] nettings||
|Netting|int|Delete|long[] ids||
|Netting|bool|ExistSection|int sectionId||
|Netting|bool|ExistCurrency|int currencyId||
|PeriodicBilling|`IEnumerable<Billing>`|Create|`IEnumerable<PeriodicBillingSetting>` settings||
|Receipt|ReceiptInputsResult|Save|ReceiptInput[] ReceiptInput, byte[] ClientKey, int ParentCustomerId||
|Receipt|int|Delete|long Id||
|Receipt|`IEnumerable<Receipt>`|Get|long[] ids||
|Receipt|`IEnumerable<Receipt>`|GetItems|ReceiptSearch option||
|Receipt|AdvanceReceiptsResult|GetAdvanceReceipts|long originalReceiptId||
|Receipt|ReceiptExcludeResult|SaveExcludeAmount|ReceiptExclude[] excludes||
|Receipt|bool|SaveMemo|ReceiptMemo memo||
|Receipt|bool|DeleteMemo|long receiptId||
|Receipt|[string|GetMemo|long receiptId||
|Receipt|AdvanceReceivedResult|SaveAdvanceReceived|AdvanceReceived[] ReceiptInfo||
|Receipt|AdvanceReceivedResult|CancelAdvanceReceived|AdvanceReceived[] ReceiptInfo||
|Receipt|`IEnumerable<ReceiptHeader>`|GetHeaderItems|int companyId||
|Receipt|`IEnumerable<ReceiptApportion>`|GetApportionItems|long[] receiptHeaderId||
|Receipt|ReceiptApportionsResult|Apportion|ReceiptApportion[] receiptList||
|Receipt|bool|ExistCurrency|int CurrencyId||
|Receipt|bool|ExistReceiptCategory|int CategoryId||
|Receipt|bool|ExistCustomer|int CustomerId||
|Receipt|bool|ExistSection|int SectionId||
|Receipt|bool|ExistCompany|int CompanyId||
|Receipt|bool|ExistExcludeCategory|int CategoryId||
|Receipt|bool|ExistOriginalReceipt|long ReceiptId||
|Receipt|bool|ExistNonApportionedReceipt|int CompanyId, DateTime ClosingFrom, DateTime ClosingTo||
|Receipt|bool|ExistNonOutputedReceipt|int CompanyId, DateTime ClosingFrom, DateTime ClosingTo||
|Receipt|bool|ExistNonAssignmentReceipt|int CompanyId, DateTime ClosingFrom, DateTime ClosingTo||
|Receipt|CountResult|Omit|int doDelete, int loginUserId, Transaction[] transactions||
|Receipt|ReceiptSectionTransfersResult|SaveReceiptSectionTransfer|ReceiptSectionTransfer[] ReceiptSectionTransfer, int LoginUserId||
|Receipt|`IEnumerable<ReceiptSectionTransfer>`|GetReceiptSectionTransferForPrint|int companyId||
|Receipt|`IEnumerable<ReceiptSectionTransfer>`|UpdateReceiptSectionTransferPrintFlag|int companyId||
|Receipt|`IEnumerable<int>`|ReceiptImportDuplicationCheck|int CompanyId, ReceiptImportDuplication[] ReceiptImportDuplication, ImporterSettingDetail[] ImporterSettingDetail||
|Receipt|bool|AdvanceReceivedDataSplit|int CompanyId, int LoginUserId, int CurrencyId, long OriginalReceiptId, AdvanceReceivedSplit[] AdvanceReceivedSplitList||
|Receipt|bool|CancelAdvanceReceivedDataSplit|int CompanyId, int LoginUserId, int CurrencyId, long OriginalReceiptId||
|Receipt|`IEnumerable<JournalizingSummary>`|GetReceiptJournalizingSummaryAsync|JournalizingOption option||
|Receipt|`IEnumerable<ReceiptJournalizing>`|ExtractReceiptJournalizingAsync|JournalizingOption option||
|Receipt|`IEnumerable<GeneralJournalizing>`|ExtractReceiptGeneralJournalizingAsync|JournalizingOption option||
|Receipt|int|UpdateOutputAtAsync|JournalizingOption option||
|Receipt|int|CancelReceiptJournalizingAsync|JournalizingOption option||
|ReceiptExclude|bool|ExistExcludeCategory|int categoryId||
|ReceiptExclude|`IEnumerable<ReceiptExclude>`|GetByReceiptId|long receiptId||
|ReceiptExclude|`IEnumerable<ReceiptExclude>`|GetByIdsAsync|`IEnumerable<long>` ids, CancellationToken token||
|ReceiptHeader|`IEnumerable<ReceiptHeader>`|Get|`IEnumerable<long>` ids||
|ReceiptHeader|int|UpdateReceiptHeader|ReceiptHeader header||
|ReceiptMemo|`IEnumerable<ReceiptMemo>`|GetItems|`IEnumerable<long>` receiptIds||
|Reminder|bool|Exist|int companyId||
|Reminder|`IEnumerable<ReminderHistory>`|GetHistoryItemsByReminderId|int reminderId||
|Reminder|`IEnumerable<ReminderSummaryHistory>`|GetSummaryHistoryItemsByReminderSummaryId|int reminderSummaryId||
|Reminder|`IEnumerable<Reminder>`|Create|int companyId, int loginUserId, int useForeignCurrency, Reminder[] Reminder, ReminderCommonSetting setting, ReminderSummarySetting[] summary||
|Reminder|int|UpdateStatus|int loginUserId, Reminder[] Reminder||
|Reminder|int|UpdateSummaryStatus|int loginUserId, ReminderSummary[] ReminderSummary||
|Reminder|`IEnumerable<ReminderBilling>`|GetReminderBillingForPrint|int companyId, int[] reminderIds||
|Reminder|`IEnumerable<ReminderBilling>`|GetReminderBillingForPrintByDestinationCode|`IEnumerable<Reminder>` reminders||
|Reminder|`IEnumerable<ReminderBilling>`|GetReminderBillingForSummaryPrint|int companyId, int[] customerIds||
|Reminder|`IEnumerable<ReminderBilling>`|GetReminderBillingForSummaryPrintByDestinationCode|`IEnumerable<ReminderSummary>` reminderSummaries||
|Reminder|`IEnumerable<ReminderBilling>`|GetReminderBillingForReprint|int companyId, ReminderOutputed reminderOutputed||
|Reminder|`IEnumerable<ReminderBilling>`|GetReminderBillingForReprintByDestination|int companyId, ReminderOutputed reminderOutputed||
|Reminder|int|UpdateReminderOutputed|int loginUserId, ReminderOutputed[] ReminderOutputed||
|Reminder|int|UpdateReminderSummaryOutputed|int loginUserId, ReminderOutputed[] reminderOutputed, ReminderSummary[] ReminderSummary||
|Reminder|`IEnumerable<ReminderOutputed>`|GetOutputedItems|ReminderOutputedSearch search||
|Reminder|int|GetMaxOutputNo|int companyId||
|Reminder|ReminderHistory|UpdateReminderHistory|ReminderHistory reminderHistory||
|Reminder|ReminderSummaryHistory|UpdateReminderSummaryHistory|ReminderSummaryHistory reminderSummaryHistory||
|Reminder|int|DeleteHistory|ReminderHistory reminderHistory||
|Reminder|int|DeleteSummaryHistory|ReminderSummaryHistory reminderSummaryHistory||
|Reminder|int|CancelReminders|`IEnumerable<Reminder>` reminders||
|Reminder|bool|ExistDestination|int DestinationId||
|ReminderSetting|ReminderCommonSetting|GetReminderCommonSetting|int CompanyId||
|ReminderSetting|ReminderCommonSetting|SaveReminderCommonSetting|ReminderCommonSetting ReminderCommonSetting||
|ReminderSetting|`IEnumerable<ReminderTemplateSetting>`|GetReminderTemplateSettings|int CompanyId||
|ReminderSetting|ReminderTemplateSetting|GetReminderTemplateSettingByCode|int CompanyId, string Code||
|ReminderSetting|ReminderTemplateSetting|SaveReminderTemplateSetting|ReminderTemplateSetting ReminderTemplateSetting||
|ReminderSetting|int|DeleteReminderTemplateSetting|int Id||
|ReminderSetting|bool|ExistReminderTemplateSetting|int ReminderTemplateId||
|ReminderSetting|`IEnumerable<ReminderLevelSetting>`|GetReminderLevelSettings|int CompanyId||
|ReminderSetting|ReminderLevelSetting|GetReminderLevelSettingByLevel|int CompanyId, int ReminderLevel||
|ReminderSetting|bool|ExistTemplateAtReminderLevel|int ReminderTemplateId||
|ReminderSetting|int|GetMaxReminderLevel|int CompanyId||
|ReminderSetting|ReminderLevelSetting|SaveReminderLevelSetting|ReminderLevelSetting ReminderLevelSetting||
|ReminderSetting|int|DeleteReminderLevelSetting|int CompanyId, int ReminderLevel||
|ReminderSetting|`IEnumerable<ReminderSummarySetting>`|GetReminderSummarySettings|int CompanyId||
|ReminderSetting|`IEnumerable<ReminderSummarySetting>`|SaveReminderSummarySetting|ReminderSummarySetting[] ReminderSummarySettings||
|Report|`IEnumerable<ArrearagesList>`|ArrearagesList|ArrearagesListSearch option||
|Report|`IEnumerable<ScheduledPaymentList>`|ScheduledPaymentList|ScheduledPaymentListSearch option||
|TaskScheduleHistory|`IEnumerable<TaskScheduleHistory>`|GetItems|TaskScheduleHistorySearch searchConditions||
|TaskScheduleHistory|int|Delete|int CompanyId||
|TaskScheduleHistory|TaskScheduleHistory|Save|TaskScheduleHistory history||
