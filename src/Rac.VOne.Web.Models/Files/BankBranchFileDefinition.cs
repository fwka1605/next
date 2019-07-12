using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class BankBranchFileDefinition : RowDefinition<BankBranch>
    {
        public StringFieldDefinition<BankBranch> BankCodeField { get; private set; }
        public StringFieldDefinition<BankBranch> BranchCodeField { get; private set; }
        public StringFieldDefinition<BankBranch> BankKanaField { get; private set; }
        public StringFieldDefinition<BankBranch> BankNameField { get; private set; }
        public StringFieldDefinition<BankBranch> BranchKanaField { get; private set; }
        public StringFieldDefinition<BankBranch> BranchNameField { get; private set; }

        public BankBranchFileDefinition(DataExpression applicationControl) : base(applicationControl)
        {
            OutputHeader = false;
            StartLineNumber = 0;
            DataTypeToken = "銀行・支店";
            FileNameToken = DataTypeToken + "マスター";
            DuplicateAdoption = DuplicateAdoption.First; // 重複データは先勝ち
            TreatDuplicateAs = TreatDuplicateAs.Ignore;

            // FieldNumber 1 銀行コード
            BankCodeField = new StringFieldDefinition<BankBranch>(
                cd => cd.BankCode)
            {
                FieldName = "銀行コード",
                FieldNumber = 1,
                Required = true,
                Accept = VisitBankCode,
            };

            // FieldNumber 2 支店コード
            BranchCodeField = new StringFieldDefinition<BankBranch>(
                cd => cd.BranchCode)
            {
                FieldName = "支店コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitBranchCode,
            };

            // FieldNumber 3 銀行名カナ
            BankKanaField = new StringFieldDefinition<BankBranch>(
                cd => cd.BankKana)
            {
                FieldName = "銀行名カナ",
                FieldNumber = 3,
                Required = false,
                Accept = VisitBankKana,
            };

            // FieldNumber 4 銀行名
            BankNameField = new StringFieldDefinition<BankBranch>(
                cd => cd.BankName)
            {
                FieldName = "銀行名",
                FieldNumber = 4,
                Required = true,
                Accept = VisitBankName,
            };

            // FieldNumber 5 支店名カナ
            BranchKanaField = new StringFieldDefinition<BankBranch>(
                cd => cd.BranchKana)
            {
                FieldName = "支店名カナ",
                FieldNumber = 5,
                Required = false,
                Accept = VisitBranchKana,
            };

            // FieldNumber 6 支店名
            BranchNameField = new StringFieldDefinition<BankBranch>(
                cd => cd.BranchName)
            {
                FieldName = "支店名",
                FieldNumber = 6,
                Required = true,
                Accept = VisitBranchName,
            };

            Fields.AddRange(new IFieldDefinition<BankBranch>[] {
                BankCodeField
                ,BranchCodeField
                ,BankKanaField
                ,BankNameField
                ,BranchKanaField
                ,BranchNameField
            });
            KeyFields.AddRange(new IFieldDefinition<BankBranch>[]
            {
                BankCodeField,
                BranchCodeField,
            });
        }

        private bool VisitBankCode(IFieldVisitor<BankBranch> visitor)
        {
            return visitor.BankCode(BankCodeField);
        }

        private bool VisitBranchCode(IFieldVisitor<BankBranch> visitor)
        {
            return visitor.BranchCode(BranchCodeField);
        }

        private bool VisitBankKana(IFieldVisitor<BankBranch> visitor)
        {
            return visitor.BankKanaAndBankBranchKana(BankKanaField);
        }

        private bool VisitBankName(IFieldVisitor<BankBranch> visitor)
        {
            return visitor.NameForBankBranchMaster(BankNameField);
        }

        private bool VisitBranchKana(IFieldVisitor<BankBranch> visitor)
        {
            return visitor.BankKanaAndBankBranchKana(BranchKanaField);
        }

        private bool VisitBranchName(IFieldVisitor<BankBranch> visitor)
        {
            return visitor.NameForBankBranchMaster(BranchNameField);
        }
    }
}
