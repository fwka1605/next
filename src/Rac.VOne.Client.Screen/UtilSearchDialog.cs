using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Common;

namespace Rac.VOne.Client.Screen
{
    public static partial class Util
    {

        /// <summary>
        ///  IApplicationUsable への ShowSearchDialog 実装
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="appUsable"></param>
        /// <param name="formName"></param>
        /// <param name="gridLoader"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>GridLoader に設定されている TModel or null を返す</returns>
        /// <remarks>
        /// なぜ共通化したか
        /// → 画面のサイズなどを、各 Screen で呼び出して指定している
        /// 検索項目によって同一を期するため、 static なメソッドとしてまとめた
        /// 利用する場合に、this キーワードが必要になる
        /// </remarks>
        private static TModel ShowSearchDialog<TModel>(this IApplicationUsable appUsable,
            string formName, IGridLoader<TModel> gridLoader,
            int width = 700, int height = 600)
            where TModel : class
        {
            using (var form = appUsable.ApplicationContext.Create(nameof(PH9900)))
            {
                var screen = form.GetAll<PH9900>().FirstOrDefault();

                System.Diagnostics.Debug.Assert(screen != null);
                screen.InitializeParentForm(formName, width, height);
                var result = screen.ShowDialog(appUsable.ParentForm, gridLoader);
                if (result != null) return result;
            }
            return default(TModel);
        }

        public static Staff ShowStaffSearchDialog(this IApplicationUsable appUsable,
            string title = "担当者検索")
            => appUsable.ShowSearchDialog(title,
                new StaffGridLoader(appUsable.ApplicationContext), 800);

        public static Department ShowDepartmentSearchDialog(this IApplicationUsable appUsable,
            string title = "請求部門検索", DepartmentGridLoader loader = null)
            => appUsable.ShowSearchDialog(title,
                loader ?? new DepartmentGridLoader(appUsable.ApplicationContext), 500);

        public static Customer ShowCustomerSearchDialog(this IApplicationUsable appUsable,
            string title = "得意先検索", CustomerGridLoader loader = null)
            => appUsable.ShowSearchDialog(title,
                loader ?? new CustomerGridLoader(appUsable.ApplicationContext));

        public static CustomerMin ShowCustomerMinSearchDialog(this IApplicationUsable appUsable,
            string title = "得意先検索", CustomerMinGridLoader loader = null)
            => appUsable.ShowSearchDialog(title,
                loader ?? new CustomerMinGridLoader(appUsable.ApplicationContext));

        public static Destination ShowDestinationSearchDialog(this IApplicationUsable appUsable, int customerId,
            string title = "送付先検索", DestinationGridLoader loader = null)
            => appUsable.ShowSearchDialog(title,
                loader ?? new DestinationGridLoader(appUsable.ApplicationContext, new DestinationSearch {
                    CompanyId   = appUsable.ApplicationContext.Login.CompanyId,
                    CustomerId  = customerId
                }));

        public static Section ShowSectionSearchDialog(this IApplicationUsable appUsable,
            string title = "入金部門検索")
            => appUsable.ShowSearchDialog(title,
                new SectionGridLoader(appUsable.ApplicationContext), 500);

        public static AccountTitle ShowAccountTitleSearchDialog(this IApplicationUsable appUsable,
            string title = "科目検索")
            => appUsable.ShowSearchDialog(title,
                new AccountTitleGridLoader(appUsable.ApplicationContext), 500);

        public static PaymentAgency ShowPaymentAgencySearchDialog(this IApplicationUsable appUsable,
            string title = "決済代行会社検索")
            => appUsable.ShowSearchDialog(title,
                new PaymentAgencyGridLoader(appUsable.ApplicationContext), 700);

        public static LoginUser ShowLoginUserSearchDialog(this IApplicationUsable appUsable,
            string title = "ログインユーザー検索")
            => appUsable.ShowSearchDialog(title,
                new LoginUserGridLoader(appUsable.ApplicationContext), 800);

        public static BankAccount ShowBankAccountSearchDialog(this IApplicationUsable appUsable,
            string title = "銀行口座検索")
            => appUsable.ShowSearchDialog(title,
                new BankAccountGridLoader(appUsable.ApplicationContext), 950);

        public static BankBranch ShowBankBranchSearchDialog(this IApplicationUsable appUsable,
            string title = "銀行・支店検索")
            => appUsable.ShowSearchDialog(title,
                new BankBranchGridLoader(appUsable.ApplicationContext), 950);

        private static Category ShowCategorySearchDialog(IApplicationUsable appUsable,
            int categoryType, string title, CategorySearch search = null, bool? useInput = null)
            => appUsable.ShowSearchDialog(title,
                new CategoryGridLoader(appUsable.ApplicationContext,
                    search ?? new CategorySearch
                    {
                        CompanyId = appUsable.ApplicationContext.Login.CompanyId,
                        CategoryType = categoryType,
                        UseInput = (useInput.HasValue) ? (int?)(useInput.Value ? 1 : 0) : null,
                    }));

        public static Category ShowBillingCategorySearchDialog(this IApplicationUsable appUsable,
            string title = "請求区分検索", CategorySearch search = null, bool? useInput = null)
            => ShowCategorySearchDialog(appUsable, CategoryType.Billing, title, search, useInput);

        public static Category ShowReceiptCategorySearchDialog(this IApplicationUsable appUsable,
            string title = "入金区分検索", CategorySearch search = null, bool? useInput = null)
            => ShowCategorySearchDialog(appUsable, CategoryType.Receipt, title, search, useInput);

        public static Category ShowCollectCategroySearchDialog(this IApplicationUsable appUsable,
            string title = "回収区分検索", CategorySearch search = null, bool? useInput = null)
            => ShowCategorySearchDialog(appUsable, CategoryType.Collect, title, search, useInput);

        public static Category ShowExcludeCategorySearchDialog(this IApplicationUsable appUsable,
            string title = "対象外区分検索", CategorySearch search = null)
            => ShowCategorySearchDialog(appUsable, CategoryType.Exclude, title, search);

        public static Currency ShowCurrencySearchDialog(this IApplicationUsable appUsable,
            string title = "通貨検索")
            => appUsable.ShowSearchDialog(title,
                new CurrencyGridLoader(appUsable.ApplicationContext), 500);

        public static BillingDivisionContract ShowBillingDivisionContractSearchDialog(this IApplicationUsable appUsable,
            string title = "契約番号検索")
            => appUsable.ShowSearchDialog(title,
                new BillingDivisionContractGridLoader(appUsable.ApplicationContext));

        public static TaxClass ShowTaxClassSearchDialog(this IApplicationUsable appUsable,
            string title = "税区分検索")
            => appUsable.ShowSearchDialog(title,
                new TaxClassGridLoader(appUsable.ApplicationContext), 500);

        private static ImporterSetting ShowImporterSettingSearchDialog(IApplicationUsable appUsable,
            Rac.VOne.Common.Constants.FreeImporterFormatType formatType, string title)
            => appUsable.ShowSearchDialog(title,
                new ImporterSettingGridLoader(appUsable.ApplicationContext) { FormatType = formatType }, 700);

        public static ImporterSetting ShowBillingImporterSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "請求取込パターン検索")
            => ShowImporterSettingSearchDialog(appUsable, Constants.FreeImporterFormatType.Billing, title);

        public static ImporterSetting ShowReceiptImporterSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "入金取込パターン検索")
            => ShowImporterSettingSearchDialog(appUsable, Constants.FreeImporterFormatType.Receipt, title);

        public static ImporterSetting ShowScheduledPaymentImporterSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "入金予定取込パターン検索")
            => ShowImporterSettingSearchDialog(appUsable, Constants.FreeImporterFormatType.PaymentSchedule, title);

        public static ImporterSetting ShowCustomerImporterSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "得意先取込パターン検索")
            => ShowImporterSettingSearchDialog(appUsable, Constants.FreeImporterFormatType.Customer, title);

        public static FixedValue ShowShareTransferFeeSearchDialog(this IApplicationUsable appUsable,
            string title = "手数料負担区分検索")
            => appUsable.ShowSearchDialog(title,
                new ShareTransferFeeGridLoader(appUsable.ApplicationContext));

        public static FixedValue ShowHolidaySettingSearchDialog(this IApplicationUsable appUsable,
            string title = "休業日の設定")
            => appUsable.ShowSearchDialog(title,
                new HolidayCalendarGridLoader(appUsable.ApplicationContext));

        public static FixedValue ShowIsParentSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "債権代表者フラグ")
            => appUsable.ShowSearchDialog(title,
                new IsParentGridLoader(appUsable.ApplicationContext));

        public static ReminderTemplateSetting ShowReminderTemplateSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "文面パターン検索", ReminderTemplateSettingGridLoader loader = null)
            => appUsable.ShowSearchDialog(title,
                loader ?? new ReminderTemplateSettingGridLoader(appUsable.ApplicationContext));

        public static InvoiceTemplateSetting ShowInvoiceTemplateSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "文面パターン検索", InvoiceTemplateSettingGridLoader loader = null)
            => appUsable.ShowSearchDialog(title,
                loader ?? new InvoiceTemplateSettingGridLoader(appUsable.ApplicationContext));

        /// <summary>
        /// 0 : しない, 1 : する の 固定値 検索ダイアログ
        /// title の指定が必須のため、初期値は設定しない
        /// </summary>
        /// <param name="appUsable"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static FixedValue ShowYesOrNoSettingSearchDialog(this IApplicationUsable appUsable,
            string title)
            => appUsable.ShowSearchDialog(title,
                new YesOrNoGridLoader(appUsable.ApplicationContext));

        public static PeriodicBillingSetting ShowPeriodicBillingSettingSearchDialog(this IApplicationUsable appUsable,
            string title = "定期請求パターン検索", PeriodicBillingSettingGridLoader loader = null)
            => appUsable.ShowSearchDialog(title, loader ?? new PeriodicBillingSettingGridLoader(appUsable.ApplicationContext));

        /// <summary>預金種別検索</summary>
        /// <param name="useReceipt">入金データ用 default</param>
        /// <param name="useTransfer">口座振替用</param>
        /// <returns></returns>
        public static BankAccountType ShowBankAccountTypeSearchDialog(this IApplicationUsable appUsable,
            string title = "預金種別検索", BankAccountTypeGridLoader loader = null,
            bool useReceipt = true, bool useTransfer = false)
            => appUsable.ShowSearchDialog(title, loader ?? new BankAccountTypeGridLoader(appUsable.ApplicationContext, useReceipt, useTransfer));
    }
}
