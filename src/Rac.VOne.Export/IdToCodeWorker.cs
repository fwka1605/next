using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;

namespace Rac.VOne.Export
{
    public class IdToCodeWorker<TModel> : IFieldVisitor<TModel>
            where TModel : class, new()
    {
        // ユーザー情報
        public int LoginCompanyId { get; set; }
        public string LoginCompanyCode { get; set; }
        public int LoginUserId { get; set; }
        public string LoginUserCode { get; set; }
        private DataExpression DataExpression { get; set; }

        //For Login User
        public int MenuLevel { get; set; }
        public int FunctionLevel { get; set; }
        public int RecordCount { get; private set; }

        public Dictionary<int, List<string>> Records { get; private set; } = new Dictionary<int, List<string>>();
        public Dictionary<int, Dictionary<int, List<int>>> IdResolutionQueue { get; private set; }
        public List<WorkingReport> Reports { get; } = new List<WorkingReport>();

        public IdToCodeWorker(ExportWorker<TModel> export)
        {
            LoginCompanyId = export.LoginCompanyId;
            LoginCompanyCode = export.LoginCompanyCode;
            LoginUserId = export.LoginUserId;
            LoginUserCode = export.LoginUserCode;

            Records = export.Records;
            RecordCount = export.RecordCount;
            IdResolutionQueue = export.IdResolutionQueue;
        }

        private bool ResolveCode<TForeign>(ForeignKeyFieldDefinition<TModel, int, TForeign> def)
        {
            if (def.GetModelsById == null) return true;
            if (!IdResolutionQueue.ContainsKey(def.FieldIndex)) return true;

            var queueOfField = IdResolutionQueue[def.FieldIndex]; // 対象の列に対応するIdマッピング待ちモデル
            var keyCodes = def.GetModelsById(queueOfField.Keys.ToArray());
            def.ForeignModels = keyCodes.Values.ToList();
            var result = true;

            foreach (var queueById in queueOfField) // <id, List<lineNo>>
            {
                var id = queueById.Key;
                if (keyCodes.ContainsKey(id)) // DB有無チェック
                {
                    var foreign = keyCodes[id]; // キーに対応するコード
                    foreach (var lineNo in queueById.Value)
                    {
                        // TODO : コードのタイプに合わせてpadding
                        SetField(lineNo, def.FieldIndex - 1, def.GetForeignCode(foreign));
                    }
                }
                else
                {
                    foreach (var lineNo in queueById.Value)
                    {
                        Reports.Add(new WorkingReport
                        {
                            LineNo = lineNo,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = id.ToString(),
                            Message = $"存在しないため、エクスポートできません。",
                        });
                        result = false;
                    }
                }
            }

            return result;
        }

        private void SetField(int lineNo, int index, string value)
        {
            if (!Records.ContainsKey(lineNo))
            {
                Records.Add(lineNo, new List<string>());
            }
            if (Records[lineNo].Count <= index)
            {
                Records[lineNo].AddRange(Enumerable.Repeat(
                        string.Empty, index - Records[lineNo].Count + 1));
            }
            Records[lineNo][index] = value;
        }

        public bool AccountTitleCode(ForeignKeyFieldDefinition<TModel, int, AccountTitle> def)
        {
            return ResolveCode(def);
        }

        public bool CategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveCode(def);
        }

        public bool OwnCompanyCode(ForeignKeyFieldDefinition<TModel, int, Company> def)
        {
            return true;
        }

        public bool LoginUserCodeField(ForeignKeyFieldDefinition<TModel, int, LoginUser> def)
        {
            return ResolveCode(def);
        }

        public bool CustomerCode(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            return ResolveCode(def);
        }

        public bool PaymentAgencyCode(ForeignKeyFieldDefinition<TModel, int, PaymentAgency> def)
        {
            return ResolveCode(def);
        }

        public bool DepartmentCode(ForeignKeyFieldDefinition<TModel, int, Department> def)
        {
            return ResolveCode(def);
        }

        public bool PayerName(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool MailAddress(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool StaffCode(ForeignKeyFieldDefinition<TModel, int, Staff> def)
        {
            return ResolveCode(def);
        }

        /// <summary>初回パスワード</summary>
        public bool InitialPassword(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool StandardNumber<TValue>(NumberFieldDefinition<TModel, TValue> def) where TValue : struct, IComparable<TValue>
        {
            return true;
        }

        public bool StandardString(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SectionCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SectionNote(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool SectionName(StringFieldDefinition<TModel> def)
        {
            return true;
        }
        public bool OwnLoginUserCode(StringFieldDefinition<TModel> def)
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


        //LogDataについて
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

        public bool DepartmentNote(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        public bool DepartmentCode(StringFieldDefinition<TModel> def)
        {
            return true;
        }

        //PE0102
        public bool CurrencyCode(ForeignKeyFieldDefinition<TModel, int, Currency> def)
        {
            return ResolveCode(def);
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
            return ResolveCode(def);
        }

        //Customer[Import/Export]
        public bool CollectCategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveCode(def);
        }
        public bool LessThanCollectCategoryId(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveCode(def);
        }
        public bool GreaterThanCollectCategoryId1(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveCode(def);
        }
        public bool GreaterThanCollectCategoryId2(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveCode(def);
        }
        public bool GreaterThanCollectCategoryId3(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            return ResolveCode(def);
        }

        public bool Fee(NullableNumberFieldDefinition<TModel, decimal> def, int foreignflg)
        {
            return true;
        }

        public bool CurrencyCodeForFee(ForeignKeyFieldDefinition<TModel, int, Currency> def, int foreignflg)
        {
            return true;
        }

        public bool CustomerCodeForDiscount(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            return ResolveCode(def);
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
