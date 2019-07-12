using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class MFMatchingJournalizingFileDefinition : RowDefinition<MatchingJournalizing>
    {
        public NumberFieldDefinition<MatchingJournalizing, long> SlipNumberField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, DateTime> RecordedAtField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitAccountTitleNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitSubCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitTaxCategoryField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitDepartmentNameField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, decimal> DebitAmountField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, int> DebitTaxAmountField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditAccountTitleNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditSubCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditTaxCategoryField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditDepartmentNameField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, decimal> CreditAmountField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, int> CreditTaxAmountField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> NoteField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> MatchingMemoField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> MFTagField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> MFJournalizingTypeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> ClosingAdjustmentField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, DateTime> CreateAtField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, DateTime> MFUpdateAtField { get; private set; }

        public MFMatchingJournalizingFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "消込仕訳";
            FileNameToken = DataTypeToken;
            OutputHeader = true;
            Fields.AddRange(InitializeFields());
        }

        private IEnumerable<IFieldDefinition<MatchingJournalizing>> InitializeFields()
        {
            yield return (SlipNumberField = new NumberFieldDefinition<MatchingJournalizing, long>(k => k.SlipNumber,
                "取引No", 1, accept: x => x.StandardNumber(SlipNumberField), formatter: value => value.ToString()));

            yield return (RecordedAtField = new NumberFieldDefinition<MatchingJournalizing, DateTime>(k => k.RecordedAt,
                "取引日", 2, accept: x => x.StandardNumber(RecordedAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));

            yield return (DebitAccountTitleNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitAccountTitleName,
                "借方勘定科目", 3, accept: x => x.StandardString(DebitAccountTitleNameField)));

            yield return (DebitSubCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitSubCode,
                "借方補助科目", 4, accept: x => x.StandardString(DebitSubCodeField)));

            yield return (DebitTaxCategoryField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitTaxCategory,
                "借方税区分", 5, accept: x => x.StandardString(DebitTaxCategoryField)));

            yield return (DebitDepartmentNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitDepartmentName,
                "借方部門", 6, accept: x => x.StandardString(DebitDepartmentNameField)));

            yield return (DebitAmountField = new NumberFieldDefinition<MatchingJournalizing, decimal>(k => k.DebitAmount,
                "借方金額(円)", 7, accept: x => x.StandardNumber(DebitAmountField), formatter: value => value.ToString()));

            yield return (DebitTaxAmountField = new NumberFieldDefinition<MatchingJournalizing, int>(k => k.DebitTaxAmount,
                "借方税額", 8, accept: x => x.StandardNumber(DebitTaxAmountField), formatter: value => value.ToString()));

            yield return (CreditAccountTitleNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditAccountTitleName,
                "貸方勘定科目", 9, accept: x => x.StandardString(CreditAccountTitleNameField)));

            yield return (CreditSubCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditSubCode,
                "貸方補助科目", 10, accept: x => x.StandardString(CreditSubCodeField)));

            yield return (CreditTaxCategoryField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditTaxCategory,
                "貸方税区分", 11, accept: x => x.StandardString(CreditTaxCategoryField)));

            yield return (CreditDepartmentNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditDepartmentName,
                "貸方部門", 12, accept: x => x.StandardString(CreditDepartmentNameField)));

            yield return (CreditAmountField = new NumberFieldDefinition<MatchingJournalizing, decimal>(k => k.CreditAmount,
                "貸方金額(円)", 13, accept: x => x.StandardNumber(CreditAmountField), formatter: value => value.ToString()));

            yield return (CreditTaxAmountField = new NumberFieldDefinition<MatchingJournalizing, int>(k => k.CreditTaxAmount,
                "貸方税額", 14, accept: x => x.StandardNumber(CreditTaxAmountField), formatter: value => value.ToString()));

            yield return (NoteField = new StringFieldDefinition<MatchingJournalizing>(k => k.Note,
                "摘要", 15, accept: x => x.StandardString(NoteField)));

            yield return (MatchingMemoField = new StringFieldDefinition<MatchingJournalizing>(k => k.MatchingMemo,
                "仕訳メモ", 16, accept: x => x.StandardString(MatchingMemoField)));

            yield return (MFTagField = new StringFieldDefinition<MatchingJournalizing>(k => k.MFTag,
                "タグ", 17, accept: x => x.StandardString(MFTagField)));

            yield return (MFJournalizingTypeField = new StringFieldDefinition<MatchingJournalizing>(k => k.MFJournalizingType,
                "MF仕訳タイプ", 18, accept: x => x.StandardString(MFJournalizingTypeField)));

            yield return (ClosingAdjustmentField = new StringFieldDefinition<MatchingJournalizing>(k => k.ClosingAdjustment,
                "決算整理仕訳", 19, accept: x => x.StandardString(ClosingAdjustmentField)));

            yield return (CreateAtField = new NumberFieldDefinition<MatchingJournalizing, DateTime>(k => k.CreateAt,
                "作成日時", 20, accept: x => x.StandardNumber(CreateAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString("yyyy/MM/dd HH:mm")));

            yield return (MFUpdateAtField = new NumberFieldDefinition<MatchingJournalizing, DateTime>(k => k.MFUpdateAt,
                "最終更新日時", 21, accept: x => x.StandardNumber(MFUpdateAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString("yyyy/MM/dd HH:mm")));

        }

    }
}
