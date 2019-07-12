using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;


namespace Rac.VOne.Web.Models.Files
{
    public class BillingInvoiceDetailFileDifinition : RowDefinition<BillingInvoiceDetailForExport>
    {
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyCodeField { get; private set; }  //1
        public NumberFieldDefinition<BillingInvoiceDetailForExport, long> BillingInputIdField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, long> BillingIdField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime> BilledAtField { get; private set; }
        public  NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime> ClosingAtField { get; private set; }
        public  NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime> SalesAtField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime> DueAtField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, decimal> BillingAmountField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, decimal> TaxAmountField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, decimal> PriceField { get; private set; } //10
        public NumberFieldDefinition<BillingInvoiceDetailForExport, decimal> RemainAmountField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note1Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note3Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note4Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note5Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note6Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note7Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> Note8Field { get; private set; } //20
        public StringFieldDefinition<BillingInvoiceDetailForExport> MemoField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> ContractNumberField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, decimal> QuantityField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> UnitSymbolField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, decimal> UnitPriceField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime> PublishAtField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime> PublishAt1stField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, int> AssignmentFlagField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> DepartmentNameField { get; private set; }//30
        public StringFieldDefinition<BillingInvoiceDetailForExport> BillingCategoryCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> BillingCategoryNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> BillingCategoryExternalCodeField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, int> TaxClassIdField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> TaxClassNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CollectCategoryCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CollectCategoryNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CollectCategoryExternalCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> StaffCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> StaffNameField { get; private set; }//40
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerNameField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, int> ShareTransferFeeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerPostalCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerAddress1Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerAddress2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerDepartmentNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerAddresseeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerHonorificField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CustomerNoteField { get; private set; }//50
        public StringFieldDefinition<BillingInvoiceDetailForExport> ExclusiveBankCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> ExclusiveBankNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> ExclusiveBranchCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> VirtualBranchCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> ExclusiveBranchNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> VirtualAccountNumberField { get; private set; }
        public NumberFieldDefinition<BillingInvoiceDetailForExport, int> ExclusiveAccountTypeIdField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBankName1Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBranchName1Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAccountType1Field { get; private set; }//60
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAccountNumber1Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBankName2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBranchName2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAccountType2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAccountNumber2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBankName3Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBranchName3Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAccountType3Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAccountNumber3Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyBankAccountNameField { get; private set; } //70
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyNameField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyPostalCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAddress1Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyAddress2Field { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyTelField { get; private set; }
        public StringFieldDefinition<BillingInvoiceDetailForExport> CompanyFaxField { get; private set; } //76

        public BillingInvoiceDetailFileDifinition(DataExpression expression, List<ExportFieldSetting> settings) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求書明細データ";
            FileNameToken = DataTypeToken;
            OutputHeader = settings.FirstOrDefault(x => x.ColumnName == "RequireHeader")?.AllowExport == 1;
            Fields.AddRange(InitializeFields(settings));
        }

        private IEnumerable<IFieldDefinition<BillingInvoiceDetailForExport>> InitializeFields(List<ExportFieldSetting> GridSettingInfo)
        {
            var index = 0;
            foreach (var setting in GridSettingInfo.Where(x => x.IsStandardField && x.AllowExport == 1))
            {
                index++;
                //1
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyCode))
                    yield return (CompanyCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyCode,
                    setting.Caption, index, accept: x => x.StandardString(CompanyCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BillingInputId))
                    yield return (BillingInputIdField = new NumberFieldDefinition<BillingInvoiceDetailForExport, long>(k => k.BillingInputId,
                        setting.Caption, index, accept: x => x.StandardNumber(BillingInputIdField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BillingId))
                    yield return (BillingIdField = new NumberFieldDefinition<BillingInvoiceDetailForExport, long>(k => k.BillingId,
                        setting.Caption, index, accept: x => x.StandardNumber(BillingIdField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BilledAt))
                    yield return (BilledAtField = new NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime>(k => k.BilledAt,
                    setting.Caption, index, accept: x => x.StandardNumber(BilledAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString(setting.DateFormat)));
                //5
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ClosingAt))
                    yield return (ClosingAtField = new NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime>(k => k.ClosingAt,
                    setting.Caption, index, accept: x => x.StandardNumber(ClosingAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString(setting.DateFormat)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.SalesAt))
                    yield return (SalesAtField = new NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime>(k => k.SalesAt,
                    setting.Caption, index, accept: x => x.StandardNumber(SalesAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString(setting.DateFormat)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.DueAt))
                    yield return (DueAtField = new NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime>(k => k.DueAt,
                    setting.Caption, index, accept: x => x.StandardNumber(DueAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString(setting.DateFormat)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BillingAmount))
                    yield return (BillingAmountField = new NumberFieldDefinition<BillingInvoiceDetailForExport, decimal>(k => k.BillingAmount,
                    setting.Caption, index, accept: x => x.StandardNumber(BillingAmountField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.TaxAmount))
                    yield return (TaxAmountField = new NumberFieldDefinition<BillingInvoiceDetailForExport, decimal>(k => k.TaxAmount,
                    setting.Caption, index, accept: x => x.StandardNumber(TaxAmountField), formatter: value => value.ToString()));
                //10
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Price))
                    yield return (PriceField = new NumberFieldDefinition<BillingInvoiceDetailForExport, decimal>(k => k.Price,
                    setting.Caption, index, accept: x => x.StandardNumber(PriceField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.RemainAmount))
                    yield return (RemainAmountField = new NumberFieldDefinition<BillingInvoiceDetailForExport, decimal>(k => k.RemainAmount,
                    setting.Caption, index, accept: x => x.StandardNumber(RemainAmountField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.InvoiceCode))
                    yield return (InvoiceCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.InvoiceCode,
                    setting.Caption, index, accept: x => x.StandardString(InvoiceCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note1))
                    yield return (Note1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note1,
                    setting.Caption, index, accept: x => x.StandardString(Note1Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note2))
                    yield return (Note2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note2,
                    setting.Caption, index, accept: x => x.StandardString(Note2Field)));
                //15
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note3))
                    yield return (Note3Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note3,
                    setting.Caption, index, accept: x => x.StandardString(Note3Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note4))
                    yield return (Note4Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note4,
                    setting.Caption, index, accept: x => x.StandardString(Note4Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note5))
                    yield return (Note5Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note5,
                    setting.Caption, index, accept: x => x.StandardString(Note5Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note6))
                    yield return (Note6Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note6,
                    setting.Caption, index, accept: x => x.StandardString(Note6Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note7))
                    yield return (Note7Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note7,
                    setting.Caption, index, accept: x => x.StandardString(Note7Field)));
                //20
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Note8))
                    yield return (Note8Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Note8,
                    setting.Caption, index, accept: x => x.StandardString(Note8Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.UnitSymbol))
                    yield return (UnitSymbolField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.UnitSymbol,
                    setting.Caption, index, accept: x => x.StandardString(UnitSymbolField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Memo))
                    yield return (MemoField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.Memo,
                    setting.Caption, index, accept: x => x.StandardString(MemoField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ContractNumber))
                    yield return (ContractNumberField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.ContractNumber,
                    setting.Caption, index, accept: x => x.StandardString(ContractNumberField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.Quantity))
                    yield return (QuantityField = new NumberFieldDefinition<BillingInvoiceDetailForExport, decimal>(k => k.Quantity,
                    setting.Caption, index, accept: x => x.StandardNumber(QuantityField), formatter: value => value.ToString()));
                //25
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.UnitPrice))
                    yield return (UnitPriceField = new NumberFieldDefinition<BillingInvoiceDetailForExport, decimal>(k => k.UnitPrice,
                    setting.Caption, index, accept: x => x.StandardNumber(UnitPriceField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.PublishAt))
                    yield return (PublishAtField = new NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime>(k => k.PublishAt,
                    setting.Caption, index, accept: x => x.StandardNumber(PublishAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString(setting.DateFormat)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.PublishAt1st))
                    yield return (PublishAt1stField = new NumberFieldDefinition<BillingInvoiceDetailForExport, DateTime>(k => k.PublishAt1st,
                    setting.Caption, index, accept: x => x.StandardNumber(PublishAt1stField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString(setting.DateFormat)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.AssignmentFlag))
                    yield return (AssignmentFlagField = new NumberFieldDefinition<BillingInvoiceDetailForExport, int>(k => k.AssignmentFlag,
                        setting.Caption, index, accept: x => x.StandardNumber(AssignmentFlagField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.DepartmentCode))
                    yield return (DepartmentCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.DepartmentCode,
                    setting.Caption, index, accept: x => x.StandardString(DepartmentCodeField)));
                //30
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.DepartmentName))
                    yield return (DepartmentNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.DepartmentName,
                    setting.Caption, index, accept: x => x.StandardString(DepartmentNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BillingCategoryCode))
                    yield return (BillingCategoryCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.BillingCategoryCode,
                    setting.Caption, index, accept: x => x.StandardString(BillingCategoryCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BillingCategoryName))
                    yield return (BillingCategoryNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.BillingCategoryName,
                    setting.Caption, index, accept: x => x.StandardString(BillingCategoryNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.BillingCategoryExternalCode))
                    yield return (BillingCategoryExternalCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.BillingCategoryExternalCode,
                    setting.Caption, index, accept: x => x.StandardString(BillingCategoryExternalCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.TaxClassId))
                    yield return (TaxClassIdField = new NumberFieldDefinition<BillingInvoiceDetailForExport, int>(k => k.TaxClassId,
                        setting.Caption, index, accept: x => x.StandardNumber(TaxClassIdField), formatter: value => value.ToString()));
                //35
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.TaxClassName))
                    yield return (TaxClassNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.TaxClassName,
                    setting.Caption, index, accept: x => x.StandardString(TaxClassNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CollectCategoryCode))
                    yield return (CollectCategoryCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CollectCategoryCode,
                    setting.Caption, index, accept: x => x.StandardString(CollectCategoryCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CollectCategoryName))
                    yield return (CollectCategoryNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CollectCategoryName,
                    setting.Caption, index, accept: x => x.StandardString(CollectCategoryNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CollectCategoryExternalCode))
                    yield return (CollectCategoryExternalCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CollectCategoryExternalCode,
                    setting.Caption, index, accept: x => x.StandardString(CollectCategoryExternalCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.StaffCode))
                    yield return (StaffCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.StaffCode,
                    setting.Caption, index, accept: x => x.StandardString(StaffCodeField)));
                //40
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.StaffName))
                    yield return (StaffNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.StaffName,
                    setting.Caption, index, accept: x => x.StandardString(StaffNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerCode))
                    yield return (CustomerCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerCode,
                    setting.Caption, index, accept: x => x.StandardString(CustomerCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerName))
                    yield return (CustomerNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerName,
                    setting.Caption, index, accept: x => x.StandardString(CustomerNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ShareTransferFee))
                    yield return (ShareTransferFeeField = new NumberFieldDefinition<BillingInvoiceDetailForExport, int>(k => k.ShareTransferFee,
                        setting.Caption, index, accept: x => x.StandardNumber(ShareTransferFeeField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerPostalCode))
                    yield return (CustomerPostalCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerPostalCode,
                    setting.Caption, index, accept: x => x.StandardString(CustomerPostalCodeField)));
                //45
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerAddress1))
                    yield return (CustomerAddress1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerAddress1,
                    setting.Caption, index, accept: x => x.StandardString(CustomerAddress1Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerAddress2))
                    yield return (CustomerAddress2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerAddress2,
                    setting.Caption, index, accept: x => x.StandardString(CustomerAddress2Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerDepartmentName))
                    yield return (CustomerDepartmentNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerDepartmentName,
                    setting.Caption, index, accept: x => x.StandardString(CustomerDepartmentNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerAddressee))
                    yield return (CustomerAddresseeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerAddressee,
                    setting.Caption, index, accept: x => x.StandardString(CustomerAddresseeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerHonorific))
                    yield return (CustomerHonorificField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerHonorific,
                    setting.Caption, index, accept: x => x.StandardString(CustomerHonorificField)));
                //50
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CustomerNote))
                    yield return (CustomerNoteField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CustomerNote,
                    setting.Caption, index, accept: x => x.StandardString(CustomerNoteField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ExclusiveBankCode))
                    yield return (ExclusiveBankCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.ExclusiveBankCode,
                    setting.Caption, index, accept: x => x.StandardString(ExclusiveBankCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ExclusiveBankName))
                    yield return (ExclusiveBankNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.ExclusiveBankName,
                    setting.Caption, index, accept: x => x.StandardString(ExclusiveBankNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ExclusiveBranchCode))
                    yield return (ExclusiveBranchCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.ExclusiveBranchCode,
                    setting.Caption, index, accept: x => x.StandardString(ExclusiveBranchCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.VirtualBranchCode))
                    yield return (VirtualBranchCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.VirtualBranchCode,
                    setting.Caption, index, accept: x => x.StandardString(VirtualBranchCodeField)));
                //55
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ExclusiveBranchName))
                    yield return (ExclusiveBranchNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.ExclusiveBranchName,
                    setting.Caption, index, accept: x => x.StandardString(ExclusiveBranchNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.VirtualAccountNumber))
                    yield return (VirtualAccountNumberField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.VirtualAccountNumber,
                    setting.Caption, index, accept: x => x.StandardString(VirtualAccountNumberField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.ExclusiveAccountTypeId))
                    yield return (ExclusiveAccountTypeIdField = new NumberFieldDefinition<BillingInvoiceDetailForExport, int>(k => k.ExclusiveAccountTypeId,
                        setting.Caption, index, accept: x => x.StandardNumber(ExclusiveAccountTypeIdField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBankName1))
                    yield return (CompanyBankName1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBankName1,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBankName1Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBranchName1))
                    yield return (CompanyBranchName1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBranchName1,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBranchName1Field)));
                //60
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAccountType1))
                    yield return (CompanyAccountType1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAccountType1,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAccountType1Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAccountNumber1))
                    yield return (CompanyAccountNumber1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAccountNumber1,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAccountNumber1Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBankName2))
                    yield return (CompanyBankName2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBankName2,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBankName2Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBranchName2))
                    yield return (CompanyBranchName2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBranchName2,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBranchName2Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAccountType2))
                    yield return (CompanyAccountType2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAccountType2,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAccountType2Field)));
                //65
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAccountNumber2))
                    yield return (CompanyAccountNumber2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAccountNumber2,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAccountNumber2Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBankName3))
                    yield return (CompanyBankName3Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBankName3,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBankName3Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBranchName3))
                    yield return (CompanyBranchName3Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBranchName3,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBranchName3Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAccountType3))
                    yield return (CompanyAccountType3Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAccountType3,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAccountType3Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAccountNumber3))
                    yield return (CompanyAccountNumber3Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAccountNumber3,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAccountNumber3Field)));
                //70
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyBankAccountName))
                    yield return (CompanyBankAccountNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyBankAccountName,
                    setting.Caption, index, accept: x => x.StandardString(CompanyBankAccountNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyName))
                    yield return (CompanyNameField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyName,
                    setting.Caption, index, accept: x => x.StandardString(CompanyNameField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyPostalCode))
                    yield return (CompanyPostalCodeField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyPostalCode,
                    setting.Caption, index, accept: x => x.StandardString(CompanyPostalCodeField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAddress1))
                    yield return (CompanyAddress1Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAddress1,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAddress1Field)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyAddress2))
                    yield return (CompanyAddress2Field = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyAddress2,
                    setting.Caption, index, accept: x => x.StandardString(CompanyAddress2Field)));
                //75
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyTel))
                    yield return (CompanyTelField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyTel,
                    setting.Caption, index, accept: x => x.StandardString(CompanyTelField)));
                if (setting.ColumnName == nameof(BillingInvoiceDetailForExport.CompanyFax))
                    yield return (CompanyFaxField = new StringFieldDefinition<BillingInvoiceDetailForExport>(k => k.CompanyFax,
                    setting.Caption, index, accept: x => x.StandardString(CompanyFaxField)));
            }
    }
        public IFieldDefinition<BillingInvoiceDetailForExport> ConvertSettingToField(string column)
        {
            IFieldDefinition<BillingInvoiceDetailForExport> field = null;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyCode)) field = CompanyCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.BillingInputId)) field = BillingInputIdField;
            if (column == nameof(BillingInvoiceDetailForExport.BillingId)) field = BillingIdField;
            if (column == nameof(BillingInvoiceDetailForExport.BilledAt)) field = BilledAtField;
            if (column == nameof(BillingInvoiceDetailForExport.ClosingAt)) field = ClosingAtField;
            if (column == nameof(BillingInvoiceDetailForExport.SalesAt)) field = SalesAtField;
            if (column == nameof(BillingInvoiceDetailForExport.DueAt)) field = DueAtField;
            if (column == nameof(BillingInvoiceDetailForExport.BillingAmount)) field = BillingAmountField;
            if (column == nameof(BillingInvoiceDetailForExport.TaxAmount)) field = TaxAmountField;
            if (column == nameof(BillingInvoiceDetailForExport.Price)) field = PriceField;
            if (column == nameof(BillingInvoiceDetailForExport.RemainAmount)) field = RemainAmountField;
            if (column == nameof(BillingInvoiceDetailForExport.InvoiceCode)) field = InvoiceCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.Note1)) field = Note1Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note2)) field = Note2Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note3)) field = Note3Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note4)) field = Note4Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note5)) field = Note5Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note6)) field = Note6Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note7)) field = Note7Field;
            if (column == nameof(BillingInvoiceDetailForExport.Note8 )) field = Note8Field;
            if (column == nameof(BillingInvoiceDetailForExport.Memo)) field = MemoField;
            if (column == nameof(BillingInvoiceDetailForExport.ContractNumber)) field = ContractNumberField;
            if (column == nameof(BillingInvoiceDetailForExport.Quantity)) field = QuantityField;
            if (column == nameof(BillingInvoiceDetailForExport.UnitSymbol)) field = UnitSymbolField;
            if (column == nameof(BillingInvoiceDetailForExport.UnitPrice)) field = UnitPriceField;
            if (column == nameof(BillingInvoiceDetailForExport.PublishAt)) field = PublishAtField;
            if (column == nameof(BillingInvoiceDetailForExport.PublishAt1st)) field = PublishAt1stField;
            if (column == nameof(BillingInvoiceDetailForExport.AssignmentFlag)) field = AssignmentFlagField;
            if (column == nameof(BillingInvoiceDetailForExport.DepartmentCode)) field = DepartmentCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.DepartmentName)) field = DepartmentNameField;
            if (column == nameof(BillingInvoiceDetailForExport.BillingCategoryCode)) field = BillingCategoryCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.BillingCategoryName)) field = BillingCategoryNameField;
            if (column == nameof(BillingInvoiceDetailForExport.BillingCategoryExternalCode)) field = BillingCategoryExternalCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.TaxClassId)) field = TaxClassIdField;
            if (column == nameof(BillingInvoiceDetailForExport.TaxClassName)) field = TaxClassNameField;
            if (column == nameof(BillingInvoiceDetailForExport.CollectCategoryCode)) field = CollectCategoryCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.CollectCategoryName)) field = CollectCategoryNameField;
            if (column == nameof(BillingInvoiceDetailForExport.CollectCategoryExternalCode)) field = CollectCategoryExternalCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.StaffName)) field = StaffNameField;
            if (column == nameof(BillingInvoiceDetailForExport.StaffCode)) field = StaffCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerCode)) field = CustomerCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerName)) field = CustomerNameField;
            if (column == nameof(BillingInvoiceDetailForExport.ShareTransferFee)) field = ShareTransferFeeField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerPostalCode)) field = CustomerPostalCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerAddress1)) field = CustomerAddress1Field;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerAddress2)) field = CustomerAddress2Field;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerDepartmentName)) field = CustomerDepartmentNameField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerAddressee)) field = CustomerAddresseeField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerHonorific)) field = CustomerHonorificField;
            if (column == nameof(BillingInvoiceDetailForExport.CustomerNote)) field = CustomerNoteField;
            if (column == nameof(BillingInvoiceDetailForExport.ExclusiveBankCode)) field = ExclusiveBankCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.ExclusiveBankName)) field = ExclusiveBankNameField;
            if (column == nameof(BillingInvoiceDetailForExport.ExclusiveBranchCode)) field = ExclusiveBranchCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.VirtualBranchCode)) field = VirtualBranchCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.ExclusiveBranchName)) field = ExclusiveBranchNameField;
            if (column == nameof(BillingInvoiceDetailForExport.VirtualAccountNumber)) field = VirtualAccountNumberField;
            if (column == nameof(BillingInvoiceDetailForExport.ExclusiveAccountTypeId)) field = ExclusiveAccountTypeIdField;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBankName1)) field = CompanyBankName1Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBranchName1)) field = CompanyBranchName1Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAccountType1)) field = CompanyAccountType1Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAccountNumber1)) field = CompanyAccountNumber1Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBankName2)) field = CompanyBankName2Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBranchName2)) field = CompanyBranchName2Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAccountType2)) field = CompanyAccountType2Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAccountNumber2)) field = CompanyAccountNumber2Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBankName3)) field = CompanyBankName3Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBranchName3)) field = CompanyBranchName3Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAccountType3)) field = CompanyAccountType3Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAccountNumber3)) field = CompanyAccountNumber3Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyBankAccountName)) field = CompanyBankAccountNameField;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyName)) field = CompanyNameField;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyPostalCode)) field = CompanyPostalCodeField;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAddress1)) field = CompanyAddress1Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyAddress2)) field = CompanyAddress2Field;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyTel)) field = CompanyTelField;
            if (column == nameof(BillingInvoiceDetailForExport.CompanyFax)) field = CompanyFaxField;
            return field;
        }
    }
}
