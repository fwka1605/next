using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;

namespace Rac.VOne.Import
{
    public class AdditionalValidationWorker<TModel> : IFieldVisitor<TModel>
        where TModel : class, new()
    {
        public AdditionalValidationWorker(ImportWorker<TModel> worker)
        {
            LoginCompanyId      = worker.LoginCompanyId;
            LoginCompanyCode    = worker.LoginCompanyCode;
            LoginUserId         = worker.LoginUserId;
            LoginUserCode       = worker.LoginUserCode;
            RecordCount         = worker.RecordCount;
            Models              = worker.Models;
        }

        // ユーザー情報
        public int LoginCompanyId { get; set; }
        public string LoginCompanyCode { get; set; }
        public int LoginUserId { get; set; }
        public string LoginUserCode { get; set; }

        public Dictionary<int, TModel> Models { get; private set; } = new Dictionary<int, TModel>();
        public int RecordCount { get; private set; }
        private object Params { get { return ImportMethod; } }

        public List<WorkingReport> Reports { get; } = new List<WorkingReport>();
        public ImportMethod? ImportMethod { get; set; }

        public bool AccountTitleCode(ForeignKeyFieldDefinition<TModel, int, AccountTitle> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool CategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool CustomerCode(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool LoginUserCodeField(ForeignKeyFieldDefinition<TModel, int, LoginUser> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool DepartmentCode(ForeignKeyFieldDefinition<TModel, int, Department> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool OwnCompanyCode(ForeignKeyFieldDefinition<TModel, int, Company> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool PaymentAgencyCode(ForeignKeyFieldDefinition<TModel, int, PaymentAgency> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool StaffCode(ForeignKeyFieldDefinition<TModel, int, Staff> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool AccountTitleCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }
        public bool SectionCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }
        public bool SectionName(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }
        public bool SectionNote(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        #region キー以外のフィールド

        public bool ContraAccountCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool ContraAccountName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool ContraAccountSubCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool FunctionLevelField(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool PayerName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool MailAddress(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool MenuLevelField(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool OwnLoginUserCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool OwnLoginUserName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool AccountTitleName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool StaffCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool StaffName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        /// <summary>初回パスワード</summary>
        public bool InitialPassword(StringFieldDefinition<TModel> def)
        {
            var reports = def.ValidateAdditional?.Invoke(Models, Params); // -> PB0301.ImportData
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool StandardNumber<TValue>(NumberFieldDefinition<TModel, TValue> def)
            where TValue : struct, IComparable<TValue>
        {
            return true;
        }

        public bool StandardString(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool UseClientField(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool UseWebViewerField(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool SourceBank(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool SourceBranch(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool DepartmentCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool DepartmentName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool DepartmentNote(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool CurrencyCode(ForeignKeyFieldDefinition<TModel, int, Currency> def)
        {
            return true;
        }

        public bool BankCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool BankName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool BranchCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool BranchName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool AccountTypeId(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool AccountNumber(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool UseValueDate(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool SectionCode(ForeignKeyFieldDefinition<TModel, int, Section> def)
        {
            return true;
        }

        public bool Fee(NullableNumberFieldDefinition<TModel, decimal> def, int foreignflg)
        {
            return true;
        }

        public bool CollectCategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return true;
        }

        //38
        public bool LessThanCollectCategoryId(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return true;
        }

        //39
        public bool GreaterThanCollectCategoryId1(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return true;
        }

        //43
        public bool GreaterThanCollectCategoryId2(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return true;
        }

        //47
        public bool GreaterThanCollectCategoryId3(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return true;
        }

        public bool CurrencyCodeForFee(ForeignKeyFieldDefinition<TModel, int, Currency> def, int foreignflg)
        {
            return true;
        }

        public bool CustomerCodeForDiscount(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            return true;
        }

        public bool MinValue(NumberFieldDefinition<TModel, decimal> def)
        {
            return true;
        }

        public bool Rate(NumberFieldDefinition<TModel, decimal> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool RoundingMode(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool SubCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool JuridicalPersonalityKana(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool HolidayCalendar(NumberFieldDefinition<TModel, DateTime> def)
        {
            return true;
        }

        public bool BankKanaAndBankBranchKana(StringFieldDefinition<TModel> def)
        {
            //return true;
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool NameForBankBranchMaster(StringFieldDefinition<TModel> def)
        {
            //return true;
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        #region CurrencyMaster
        public bool CurrencyCode(StringFieldDefinition<TModel> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        public bool CurrencyName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool CurrencySymbol(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool CurrencyPrecision(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool CurrencyDisplayOrder(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool CurrencyNote(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool CurrencyTolerance(NumberFieldDefinition<TModel, decimal> def)
        {
            IEnumerable<WorkingReport> reports = def.ValidateAdditional?.Invoke(Models, Params);
            if (reports != null && reports.Any())
            {
                Reports.AddRange(reports);
                return false;
            }
            return true;
        }

        #endregion

        public bool Tel(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool Fax(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        #endregion
    }
}
