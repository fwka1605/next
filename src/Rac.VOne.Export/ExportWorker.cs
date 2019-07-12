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
    public class ExportWorker<TModel> : IFieldVisitor<TModel>
            where TModel : class, new()
    {
        // ユーザー情報
        public int LoginCompanyId { get; set; }
        public string LoginCompanyCode { get; set; }
        public int LoginUserId { get; set; }
        public string LoginUserCode { get; set; }
        private DataExpression DataExpression { get; set; }

        public int RecordCount { get; private set; }
        public Dictionary<int, List<string>> Records { get; } = new Dictionary<int, List<string>>();
        public List<string> FieldValues { get; private set; }
        public TModel Model { get; private set; }
        public List<WorkingReport> Reports { get; } = new List<WorkingReport>();

        // <fieldNo, <id, List<lineNo>>>
        public Dictionary<int, Dictionary<int, List<int>>> IdResolutionQueue { get; private set; }

        private void SetField(int index, string value)
        {
            if (FieldValues.Count <= index)
            {
                FieldValues.AddRange(Enumerable.Repeat(
                        string.Empty, index - FieldValues.Count + 1));
            }
            FieldValues[index] = value;
        }

        public ExportWorker(DataExpression exp)
        {
            DataExpression = exp;
        }

        public int NewRecord(TModel model)
        {
            FieldValues = new List<string>();
            Model = model;
            RecordCount++;
            Records.Add(RecordCount, FieldValues);
            Reports.Clear();
            return RecordCount;
        }

        // <fieldNo, <id, lineNo>>
        private Dictionary<int, Dictionary<int, List<int>>> StoreId(
                Dictionary<int, Dictionary<int, List<int>>> queue,
                FieldDefinition<TModel, int> def,
                int value)
        {
            if (queue == null)
            {
                queue = new Dictionary<int, Dictionary<int, List<int>>>();
            }
            // 列番号に対応するキューがあるか
            Dictionary<int, List<int>> fieldQueue = null;
            if (!queue.TryGetValue(def.FieldIndex, out fieldQueue))
            {
                fieldQueue = new Dictionary<int, List<int>>();
                queue.Add(def.FieldIndex, fieldQueue);
            }

            // キューに対象のIdがあるか
            List<int> idQueue = null;
            if (!fieldQueue.TryGetValue(value, out idQueue))
            {
                idQueue = new List<int>();
                fieldQueue.Add(value, idQueue);
            }
            idQueue.Add(RecordCount);

            return queue;
        }

        public bool AccountTitleCode(ForeignKeyFieldDefinition<TModel, int, AccountTitle> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.AccountTitleCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.AccountTitleCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, AccountTitle>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool CategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code?.PadLeft(2, '0'));
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Category>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool PaymentAgencyCode(ForeignKeyFieldDefinition<TModel, int, PaymentAgency> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                //  FieldValues[def.FieldNumber - 1] = $"{code}".PadLeft(2, '0');
                SetField(def.FieldIndex - 1, code?.PadLeft(2, '0'));
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, PaymentAgency>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool CustomerCode(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.CustomerCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.CustomerCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Customer>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool LoginUserCodeField(ForeignKeyFieldDefinition<TModel, int, LoginUser> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.LoginUserCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.LoginUserCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, LoginUser>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool DepartmentCode(ForeignKeyFieldDefinition<TModel, int, Department> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.DepartmentCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.DepartmentCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Department>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool PayerName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool SectionCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool SectionName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool SectionNote(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool SectionMasterPyaerCodeLeft(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool SectionMasterPyaerCodeRight(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool MailAddress(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool OwnCompanyCode(ForeignKeyFieldDefinition<TModel, int, Company> def)
        {
            SetField(def.FieldIndex - 1, LoginCompanyCode);
            return true;
        }

        public bool StaffCode(ForeignKeyFieldDefinition<TModel, int, Staff> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.StaffCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.StaffCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Staff>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        /// <summary>初回パスワード</summary>
        public bool InitialPassword(StringFieldDefinition<TModel> def)
        {
            SetField(def.FieldIndex - 1, ""); // エクスポート時は全て空文字列とする
            return true;
        }

        public bool StandardNumber<TValue>(NumberFieldDefinition<TModel, TValue> def)
            where TValue : struct, IComparable<TValue>
        {
            string result = string.Empty;
            if (def.IsNullable)
            {
                TValue? value = (def as NullableNumberFieldDefinition<TModel, TValue>).GetNullableValue(Model);
                result = (value == null) ? string.Empty : def.Format(value.Value);
            }
            else
            {
                TValue value = def.GetValue(Model);
                result = def.Format(value);
            }

            SetField(def.FieldIndex - 1, result);
            return true;
        }

        public bool StandardString(StringFieldDefinition<TModel> def)
        {
            SetField(def.FieldIndex - 1, def.GetValue(Model));
            return true;
        }

        //For Login User
        public bool OwnLoginUserCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool OwnLoginUserName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool MenuLevelField(NumberFieldDefinition<TModel, int> def)
        {
            int value = def.GetValue(Model);
            SetField(def.FieldIndex - 1, def.Format(value));
            return true;
        }

        public bool FunctionLevelField(NumberFieldDefinition<TModel, int> def)
        {
            int value = def.GetValue(Model);
            SetField(def.FieldIndex - 1, def.Format(value));
            return true;
        }

        public bool UseClientField(NumberFieldDefinition<TModel, int> def)
        {
            int value = def.GetValue(Model);
            SetField(def.FieldIndex - 1, def.Format(value));
            return true;
        }

        public bool UseWebViewerField(NumberFieldDefinition<TModel, int> def)
        {
            int value = def.GetValue(Model);
            SetField(def.FieldIndex - 1, def.Format(value));
            return true;
        }

        public bool StaffCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool StaffName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool AccountTitleCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool AccountTitleName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool ContraAccountCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool ContraAccountName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool ContraAccountSubCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        //For LogDataについて
        public bool SourceBank(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
        public bool SourceBranch(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool DepartmentCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool DepartmentName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool DepartmentNote(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        //PE0102
        public bool CurrencyCode(ForeignKeyFieldDefinition<TModel, int, Currency> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Currency>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool BankCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool BankName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool BranchCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool BranchName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool AccountTypeId(NumberFieldDefinition<TModel, int> def)
        {
            int value = def.GetValue(Model);
            SetField(def.FieldIndex - 1, value.ToString());
            return true;
        }

        public bool AccountNumber(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool UseValueDate(NumberFieldDefinition<TModel, int> def)
        {
            string result = string.Empty;
            if (def.IsNullable)
            {
                int? value = (def as NullableNumberFieldDefinition<TModel, int>).GetNullableValue(Model);
                result = (value == null) ? string.Empty : def.Format(value.Value);
            }
            else
            {
                int value = def.GetValue(Model);
                result = def.Format(value);
            }

            SetField(def.FieldIndex - 1, result);
            return true;
        }

        public bool SectionCode(ForeignKeyFieldDefinition<TModel, int, Section> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.SectionCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.SectionCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Section>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }
        //Customer[Import/Export]
        #region 得意先マスター
        public bool CollectCategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Category>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool LessThanCollectCategoryId(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Category>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }
        public bool GreaterThanCollectCategoryId1(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Category>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool GreaterThanCollectCategoryId2(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Category>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
                else
                {
                    def.GetModelsById = null;
                }
            }
            return true;
        }

        public bool GreaterThanCollectCategoryId3(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Category>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
                else
                {
                    def.GetModelsById = null;
                }
            }
            return true;
        }
        #endregion


        public bool Fee(NullableNumberFieldDefinition<TModel, decimal> def, int foreignflg)
        {
            return StandardNumber(def);
        }

        public bool CurrencyCodeForFee(ForeignKeyFieldDefinition<TModel, int, Currency> def, int foreignflg)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Currency>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool CustomerCodeForDiscount(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            if (def.ModelHasCode)
            {
                string code = def.GetCode(Model);
                if (DataExpression.CustomerCodeFormatString == "9")
                {
                    code = code?.PadLeft(DataExpression.CustomerCodeLength, '0');
                }
                SetField(def.FieldIndex - 1, code);
            }
            else
            {
                var nullable = def as NullableForeignKeyFieldDefinition<TModel, int, Customer>;
                if (!def.IsNullable
                        || nullable.GetNullableValue(Model) != null)
                {
                    int id = def.GetValue(Model);
                    IdResolutionQueue = StoreId(IdResolutionQueue, def, id);
                }
            }
            return true;
        }

        public bool MinValue(NumberFieldDefinition<TModel, decimal> def)
        {
            return StandardNumber(def);
        }

        public bool Rate(NumberFieldDefinition<TModel, decimal> def)
        {
            return StandardNumber(def);
        }

        public bool RoundingMode(NumberFieldDefinition<TModel, int> def)
        {
            return StandardNumber(def);
        }

        public bool SubCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool JuridicalPersonalityKana(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool HolidayCalendar(NumberFieldDefinition<TModel, DateTime> def)
        {
            return StandardNumber(def);
        }

        public bool BankKanaAndBankBranchKana(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool NameForBankBranchMaster(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool CurrencyCode(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool CurrencyName(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool CurrencySymbol(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool CurrencyPrecision(NumberFieldDefinition<TModel, int> def)
        {
            return StandardNumber(def);
        }

        public bool CurrencyDisplayOrder(NumberFieldDefinition<TModel, int> def)
        {
            return StandardNumber(def);
        }

        public bool CurrencyNote(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool CurrencyTolerance(NumberFieldDefinition<TModel, decimal> def)
        {
            return StandardNumber(def);
        }

        public bool Tel(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }

        public bool Fax(StringFieldDefinition<TModel> def)
        {
            return StandardString(def);
        }
    }
}
