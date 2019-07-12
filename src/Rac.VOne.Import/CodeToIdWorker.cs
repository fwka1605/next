using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Import
{
    public class CodeToIdWorker<TModel> : IFieldVisitor<TModel>
            where TModel : class, new()
    {
        public CodeToIdWorker(ImportWorker<TModel> import)
        {
            LoginCompanyId      = import.LoginCompanyId;
            LoginCompanyCode    = import.LoginCompanyCode;
            LoginUserId         = import.LoginUserId;
            LoginUserCode       = import.LoginUserCode;
            RecordCount         = import.RecordCount;
            Models              = import.Models;
            CodeResolutionQueue = import.CodeResolutionQueue;
        }

        // ユーザー情報
        public int LoginCompanyId { get; set; }
        public string LoginCompanyCode { get; set; }
        public int LoginUserId { get; set; }
        public string LoginUserCode { get; set; }
        private DataExpression DataExpression { get; set; }

        public int RecordCount { get; private set; }

        public Dictionary<int, TModel> Models { get; private set; } = new Dictionary<int, TModel>();
        public Dictionary<int, Dictionary<string, List<int>>> CodeResolutionQueue { get; private set; }
        public List<WorkingReport> Reports { get; } = new List<WorkingReport>();

        private bool ResolveId<TValue, TForeign>(ForeignKeyFieldDefinition<TModel, TValue, TForeign> def)
            where TValue : struct, IComparable<TValue>
        {
            if (def.GetModelsByCode == null
                || CodeResolutionQueue == null
                || !CodeResolutionQueue.ContainsKey(def.FieldIndex)) return true;

            var queueOfField = CodeResolutionQueue[def.FieldIndex]; // 対象の列に対応するIdマッピング待ちモデル
            var codeKeys = def.GetModelsByCode(queueOfField.Keys.ToArray());
            def.ForeignModels = codeKeys.Values.ToList();
            bool result = true;

            foreach (var queueByCode in queueOfField) // <code, List<lineNo>>
            {
                var code = queueByCode.Key;
                if (codeKeys.ContainsKey(code)) // DB有無チェック
                {
                    var foreign = codeKeys[code]; // コードに対応するキー
                    foreach (var lineNo in queueByCode.Value.Where(l => Models.ContainsKey(l)))
                    {
                        def.SetValue(Models[lineNo], def.GetForeignKey(foreign));
                    }
                }
                else
                {
                    foreach (var lineNo in queueByCode.Value)
                    {
                        Reports.Add(new WorkingReport
                        {
                            LineNo = lineNo,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = code,
                            Message = $"存在しないため、インポートできません。",
                        });
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool AccountTitleCode(ForeignKeyFieldDefinition<TModel, int, AccountTitle> def)
        {
            return ResolveId(def);
        }

        public bool CategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveId(def);
        }

        public bool CustomerCode(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            return ResolveId(def);
        }

        public bool PaymentAgencyCode(ForeignKeyFieldDefinition<TModel, int, PaymentAgency> def)
        {
            return ResolveId(def);
        }

        public bool DepartmentCode(ForeignKeyFieldDefinition<TModel, int, Department> def)
        {
            return ResolveId(def);
        }

        public bool PayerName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool MailAddress(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool OwnCompanyCode(ForeignKeyFieldDefinition<TModel, int, Company> def)
        {
            return true;
        }

        public bool StaffCode(ForeignKeyFieldDefinition<TModel, int, Staff> def)
        {
            return ResolveId(def);
        }

        /// <summary>初回パスワード</summary>
        public bool InitialPassword(StringFieldDefinition<TModel> def)
        {
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

        //For Login User
        public bool OwnLoginUserCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SectionCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SectionName(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SectionNote(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool OwnLoginUserName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool MenuLevelField(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool FunctionLevelField(NumberFieldDefinition<TModel, int> def)
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

        public bool StaffCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool StaffName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool AccountTitleCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool AccountTitleName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

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

        //For LogDataについて
        public bool SourceBank(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SourceBranch(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool DepartmentName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool DepartmentCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool DepartmentNote(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        //PE0102
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
            return true;
        }

        public bool UseValueDate(NumberFieldDefinition<TModel, int> def)
        {
            return true;
        }

        public bool SectionCode(ForeignKeyFieldDefinition<TModel, int, Section> def)
        {
            return ResolveId(def);
        }

        //Customer[Import/Export]
        public bool CollectCategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveId(def);
        }

        public bool LessThanCollectCategoryId(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveId(def);
        }

        public bool GreaterThanCollectCategoryId1(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveId(def);
        }

        public bool GreaterThanCollectCategoryId2(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveId(def);
        }

        public bool GreaterThanCollectCategoryId3(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveId(def);
        }

        public bool Fee(NullableNumberFieldDefinition<TModel, decimal> def, int foreignflg)
        {
            return true;
        }

        public bool CurrencyCodeForFee(ForeignKeyFieldDefinition<TModel, int, Currency> def, int foreignflg)
        {
            return ResolveId(def);
        }

        public bool CustomerCodeForDiscount(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            return ResolveId(def);
        }

        public bool MinValue(NumberFieldDefinition<TModel, decimal> def)
        {
            return true;
        }

        public bool Rate(NumberFieldDefinition<TModel, decimal> def)
        {
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

        public bool LoginUserCodeField(ForeignKeyFieldDefinition<TModel, int, LoginUser> def)
        {
            return ResolveId(def);
        }

        public bool HolidayCalendar(NumberFieldDefinition<TModel, DateTime> def)
        {
            return true;
        }

        public bool BankKanaAndBankBranchKana(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool NameForBankBranchMaster(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool CurrencyCode(StringFieldDefinition<TModel> def)
        {
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
            return true;
        }

        public bool Tel(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool Fax(StringFieldDefinition<TModel> def)
        {
            return true;
        }
    }
}
