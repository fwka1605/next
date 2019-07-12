using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    /// <summary>
    /// Import / Export に利用する Field の visitor pattern 用 interface
    /// TODO: refacotr
    /// interface に項目が多すぎる import で特殊処理を行うためだけ
    /// ExportWorker に項目を追加する目的で 当 interface の項目追加は禁止
    /// 各Definition で 汎用の StandardString, StandardNumber を利用すること
    /// ImportWorker や 他 Worker で特殊な処理をする場合のみ追加
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IFieldVisitor<TModel>
            where TModel : class, new()
    {
        int LoginCompanyId { get; set; }
        string LoginCompanyCode { get; set; }
        int LoginUserId { get; set; }
        string LoginUserCode { get; set; }

        int RecordCount { get; }
        /// <summary>収集されたログ</summary>
        List<WorkingReport> Reports { get; }

        /// <summary>勘定科目コード(外部キー)</summary>
        bool AccountTitleCode(ForeignKeyFieldDefinition<TModel, int, AccountTitle> def);
        /// <summary>区分コード(外部キー)</summary>
        bool CategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def);
        /// <summary>ログインユーザーの会社コード(外部キー)</summary>
        bool OwnCompanyCode(ForeignKeyFieldDefinition<TModel, int, Company> def);
        /// <summary>得意先コード(外部キー)</summary>
        bool CustomerCode(ForeignKeyFieldDefinition<TModel, int, Customer> def);
        /// <summary>決済代行会社コード(外部キー)</summary>
        bool PaymentAgencyCode(ForeignKeyFieldDefinition<TModel, int, PaymentAgency> def);
        /// <summary>請求部門コード(外部キー)</summary>
        bool DepartmentCode(ForeignKeyFieldDefinition<TModel, int, Department> def);
        /// <summary>振込依頼人名</summary>
        bool PayerName(StringFieldDefinition<TModel> def);
        /// <summary>メールアドレス</summary>
        bool MailAddress(StringFieldDefinition<TModel> def);
        /// <summary>営業担当者コード(外部キー)</summary>
        bool StaffCode(ForeignKeyFieldDefinition<TModel, int, Staff> def);
        /// <summary>初回パスワード</summary>
        bool InitialPassword(StringFieldDefinition<TModel> def);
        /// <summary>仕向銀行名</summary>
        bool SourceBank(StringFieldDefinition<TModel> def);
        /// <summary>仕向支店名</summary>
        bool SourceBranch(StringFieldDefinition<TModel> def);

        /// <summary>汎用値型(数値, 日付)</summary>
        /// <typeparam name="TValue">型(数値、日付)</typeparam>
        bool StandardNumber<TValue>(NumberFieldDefinition<TModel, TValue> def)
                where TValue : struct, IComparable<TValue>;
        /// <summary>汎用文字列</summary>
        bool StandardString(StringFieldDefinition<TModel> def);

        /// <summary>ログインユーザーコード</summary>
        bool OwnLoginUserCode(StringFieldDefinition<TModel> def);
        /// <summary>ログインユーザー名</summary>
        bool OwnLoginUserName(StringFieldDefinition<TModel> def);
        /// <summary>権限</summary>
        bool MenuLevelField(NumberFieldDefinition<TModel, int> def);
        /// <summary>機能制限レベル</summary>
        bool FunctionLevelField(NumberFieldDefinition<TModel, int> def);
        /// <summary>クライアント利用可否</summary>
        bool UseClientField(NumberFieldDefinition<TModel, int> def);
        // TODO : UseClientFieldとUseWebViewerFieldを一本化→Flag(NumberFieldDefinition<TModel, int> def)
        /// <summary>Web Viewer利用可否</summary>
        bool UseWebViewerField(NumberFieldDefinition<TModel, int> def);

        /// <summary>ログインユーザーコード(外部キー)</summary>
        bool LoginUserCodeField(ForeignKeyFieldDefinition<TModel, int, LoginUser> def);

        /// <summary>営業担当者コード</summary>
        bool StaffCode(StringFieldDefinition<TModel> def);
        /// <summary>営業担当者名</summary>
        bool StaffName(StringFieldDefinition<TModel> def);

        /// <summary>勘定科目コード</summary>
        bool AccountTitleCode(StringFieldDefinition<TModel> def);
        /// <summary>勘定科目名</summary>
        bool AccountTitleName(StringFieldDefinition<TModel> def);
        /// <summary>相手科目コード</summary>
        bool ContraAccountCode(StringFieldDefinition<TModel> def);
        /// <summary>相手科目名称</summary>
        bool ContraAccountName(StringFieldDefinition<TModel> def);
        /// <summary>相手科目補助コード</summary>
        bool ContraAccountSubCode(StringFieldDefinition<TModel> def);

        /// <summary>請求部門名</summary>
        bool DepartmentName(StringFieldDefinition<TModel> def);
        /// <summary>請求部門備考</summary>
        bool DepartmentNote(StringFieldDefinition<TModel> def);
        /// <summary>請求部門コード</summary>
        bool DepartmentCode(StringFieldDefinition<TModel> def);

        /// <summary>通貨コード(外部キー)</summary>
        bool CurrencyCode(ForeignKeyFieldDefinition<TModel, int, Currency> def);

        /// <summary>預金種別</summary>
        bool AccountTypeId(NumberFieldDefinition<TModel, int> def);
        /// <summary>口座番号</summary>
        bool AccountNumber(StringFieldDefinition<TModel> def);
        /// <summary>フラグ(0,1)</summary>
        bool UseValueDate(NumberFieldDefinition<TModel, int> def);
        /// <summary>入金部門コード(外部キー)</summary>
        bool SectionCode(ForeignKeyFieldDefinition<TModel, int, Section> def);
        /// <summary>銀行コード</summary>
        bool BankCode(StringFieldDefinition<TModel> def);
        /// <summary>銀行名</summary>
        bool BankName(StringFieldDefinition<TModel> def);
        /// <summary>支店コード</summary>
        bool BranchCode(StringFieldDefinition<TModel> def);
        /// <summary>支店名</summary>
        bool BranchName(StringFieldDefinition<TModel> def);
        bool CollectCategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def);
        bool LessThanCollectCategoryId(ForeignKeyFieldDefinition<TModel, int, Category> def);
        bool GreaterThanCollectCategoryId1(ForeignKeyFieldDefinition<TModel, int, Category> def);

        bool GreaterThanCollectCategoryId2(ForeignKeyFieldDefinition<TModel, int, Category> def);

        bool GreaterThanCollectCategoryId3(ForeignKeyFieldDefinition<TModel, int, Category> def);


        /// <summary>手数料通貨コード</summary>
        bool CurrencyCodeForFee(ForeignKeyFieldDefinition<TModel, int, Currency> def, int foreignflg);
        /// <summary>手数料金額</summary>
        bool Fee(NullableNumberFieldDefinition<TModel, decimal> def, int foreignflg);

        /// <summary>得意先コード歩引設定用(外部キー)</summary>
        bool CustomerCodeForDiscount(ForeignKeyFieldDefinition<TModel, int, Customer> def);
        /// <summary>最低実行金額</summary>
        bool MinValue(NumberFieldDefinition<TModel, decimal> def);
        /// <summary>歩引率</summary>
        bool Rate(NumberFieldDefinition<TModel, decimal> def);
        /// <summary>端数処理</summary>
        bool RoundingMode(NumberFieldDefinition<TModel, int> def);
        /// <summary>補助コード</summary>
        bool SubCode(StringFieldDefinition<TModel> def);

        /// <summary>法人格カナ</summary>
        bool JuridicalPersonalityKana(StringFieldDefinition<TModel> def);

        /// <summary>休業日</summary>
        bool HolidayCalendar(NumberFieldDefinition<TModel, DateTime> def);

        /// <summary>銀行カナ／支店カナ</summary>
        bool BankKanaAndBankBranchKana(StringFieldDefinition<TModel> def);
        /// <summary>銀行名／支店名</summary>
        bool NameForBankBranchMaster(StringFieldDefinition<TModel> def);

        /// <summary>入金部門コード</summary>
        bool SectionCode(StringFieldDefinition<TModel> def);
        /// <summary>入金部門名</summary>
        bool SectionName(StringFieldDefinition<TModel> def);
        /// <summary>入金部門備考</summary>
        bool SectionNote(StringFieldDefinition<TModel> def);

        /// <summary>通貨コード</summary>
        bool CurrencyCode(StringFieldDefinition<TModel> def);
        /// <summary>通貨名称</summary>
        bool CurrencyName(StringFieldDefinition<TModel> def);
        /// <summary>通貨記号</summary>
        bool CurrencySymbol(StringFieldDefinition<TModel> def);
        /// <summary>小数点以下桁数</summary>
        bool CurrencyPrecision(NumberFieldDefinition<TModel, int> def);
        /// <summary>表示順</summary>
        bool CurrencyDisplayOrder(NumberFieldDefinition<TModel, int> def);
        /// <summary>通貨備考</summary>
        bool CurrencyNote(StringFieldDefinition<TModel> def);
        /// <summary>誤差許容値</summary>
        bool CurrencyTolerance(NumberFieldDefinition<TModel, decimal> def);
        /// <summary>電話番号</summary>
        bool Tel(StringFieldDefinition<TModel> def);
        /// <summary>FAX番号</summary>
        bool Fax(StringFieldDefinition<TModel> def);
    }
}
