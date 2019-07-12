using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rac.VOne.Client.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Screen
{
    public static class UtilReminder
    {
        public static ReminderReport CreateReminderReport(IEnumerable<ReminderBilling> source,
            ReminderCommonSetting ReminderCommonSetting,
            List<ReminderSummarySetting> ReminderSummarySetting,
            ReminderTemplateSetting template,
            Company Company,
            List<ColumnNameSetting> ColumnNameSetting,
            DateTime outputAt,
            PdfOutputSetting pdfSetting)
        {
            var reminderReport = new ReminderReport();

            reminderReport.Template = template;
            reminderReport.txtOutputAtHeader.Text = outputAt.ToString("yyyy年MM月dd日");
            reminderReport.txtOutputAtDetail.Text = outputAt.ToString("yyyy年MM月dd日");

            var rd = source.FirstOrDefault();
            var isDestination = rd.DestinationId.HasValue && rd.DestinationId != 0;

            reminderReport.txtCustomerPostalCode.Text = isDestination ? rd.DestinationPostalCode : rd.CustomerPostalCode;
            reminderReport.txtCustomerAddress1.Text = isDestination ? rd.DestinationAddress1 : rd.CustomerAddress1;
            reminderReport.txtCustomerAddress2.Text = isDestination ? rd.DestinationAddress2 : rd.CustomerAddress2;
            reminderReport.txtCustomerName.Text = !string.IsNullOrEmpty(rd.DestinationName) ? rd.DestinationName : rd.CustomerName;

            if (isDestination)
            {
                var isNullDepartment = string.IsNullOrEmpty(rd.DestinationDepartmentName);
                var isNullAddressee = string.IsNullOrEmpty(rd.DestinationAddressee);
                if (isNullDepartment && isNullAddressee)
                {
                    reminderReport.txtCustomerName.Text += rd.DestinationHonorific;
                }
                else if (!isNullDepartment && isNullAddressee)
                {
                    reminderReport.txtDestinationDepartmentName.Text = rd.DestinationDepartmentName + " " + rd.DestinationHonorific;
                    reminderReport.txtDestinationAddressee.Text = "";
                }
                else if (isNullDepartment && !isNullAddressee)
                {
                    reminderReport.txtDestinationDepartmentName.Text = rd.DestinationAddressee + " " + rd.DestinationHonorific;
                    reminderReport.txtDestinationAddressee.Text = "";
                }
                else
                {
                    reminderReport.txtDestinationDepartmentName.Text = rd.DestinationDepartmentName;
                    reminderReport.txtDestinationAddressee.Text = rd.DestinationAddressee + " " + rd.DestinationHonorific;
                }
            }
            else
            {
                var isNullDepartment = string.IsNullOrEmpty(rd.DestinationDepartmentName);
                var isNullAddressee = string.IsNullOrEmpty(rd.DestinationAddressee);
                var destinationHonorific = rd.DestinationHonorific;
                if (isNullDepartment && isNullAddressee)
                {
                    reminderReport.txtCustomerName.Text += destinationHonorific;
                    reminderReport.txtDestinationDepartmentName.Text = "";
                    reminderReport.txtDestinationAddressee.Text = "";
                }
                else if (!isNullDepartment && isNullAddressee)
                {
                    reminderReport.txtDestinationDepartmentName.Text = rd.DestinationDepartmentName + destinationHonorific;
                    reminderReport.txtDestinationAddressee.Text = "";
                }
                else if (isNullDepartment && !isNullAddressee)
                {
                    reminderReport.txtDestinationDepartmentName.Text = rd.DestinationAddressee + destinationHonorific;
                    reminderReport.txtDestinationAddressee.Text = "";
                }
                else
                {
                    reminderReport.txtDestinationDepartmentName.Text = rd.DestinationDepartmentName;
                    reminderReport.txtDestinationAddressee.Text = rd.DestinationAddressee + destinationHonorific;
                }

            }

            reminderReport.DataSource = new BindingSource(source, null);

            reminderReport.txtCompanyPostalCode.Text = "〒" + Company.PostalCode;
            reminderReport.txtCompanyAddress1.Text = Company.Address1;
            reminderReport.txtCompanyAddress2.Text = Company.Address2;
            reminderReport.txtCompanyName.Text = Company.Name;

            reminderReport.txtOwnDepartmentName.Text = ReminderCommonSetting.OwnDepartmentName;
            reminderReport.txtAccountingStaffName.Text = ReminderCommonSetting.AccountingStaffName;

            if (ReminderCommonSetting.OutputDetail == 1)
            {
                reminderReport.lblNote.Text = ColumnNameSetting.First(x => x.ColumnName == ReminderCommonSetting.OutputDetailItem).DisplayColumnName;
                reminderReport.txtNote.DataField = ReminderCommonSetting.OutputDetailItem;
            }

            if (!ReminderSummarySetting.Any(x => x.ColumnName == "Staff" && x.Available == 1))
            {
                reminderReport.txtStaffName.Visible = false;
                reminderReport.lblTel.Visible = false;
                reminderReport.txtStaffTel.Visible = false;
                reminderReport.lblFax.Visible = false;
                reminderReport.txtStaffFax.Visible = false;
            }

            reminderReport.txtAccountName.Text = Company.BankAccountName;
            var rb = source.First();
            if (pdfSetting.IsAllInOne)
            {
                reminderReport.Name = $"督促状{outputAt.ToString("yyyyMMdd")}";
            }
            else
            {
                var name = pdfSetting.FileName.Replace("[CODE]", rb.CustomerCode);
                name = name.Replace("[NAME]", rb.CustomerName);
                name = name.Replace("[NO]", rb.OutputNo.ToString("000000"));
                name = name.Replace("[DATE]", outputAt.ToString("yyyyMMdd"));
                reminderReport.Name = name;
            }
            reminderReport.CustomerReceiveAccount1 = rb.CustomerReceiveAccount1 == 1;
            reminderReport.CustomerReceiveAccount2 = rb.CustomerReceiveAccount2 == 1;
            reminderReport.CustomerReceiveAccount3 = rb.CustomerReceiveAccount3 == 1;

            reminderReport.CompanyBankAccount1 = $"{Company.BankName1} {Company.BranchName1} {Company.AccountType1} {Company.AccountNumber1}";
            reminderReport.CompanyBankAccount2 = $"{Company.BankName2} {Company.BranchName2} {Company.AccountType2} {Company.AccountNumber2}";
            reminderReport.CompanyBankAccount3 = $"{Company.BankName3} {Company.BranchName3} {Company.AccountType3} {Company.AccountNumber3}";

            reminderReport.TotalAmountPrintHandler += (customerId) =>
            {
                reminderReport.txtTotalAmount.Text = source.Where(x => x.CustomerId == customerId)
                                                                    .Sum(x => x.RemainAmount).ToString("#,##0");
            };

            if (ReminderCommonSetting.OutputDetail == 0)
                reminderReport.HideDetail();

            if (!source.Any(x => x.OutputNo > 0))
                reminderReport.HideOutputNo();

            reminderReport.Run(false);

            return reminderReport;
        }
    }
}
