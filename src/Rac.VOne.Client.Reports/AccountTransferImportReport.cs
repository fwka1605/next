using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    public partial class AccountTransferImportReport : GrapeCity.ActiveReports.SectionReport
    {
        public AccountTransferImportReport()
        {
            InitializeComponent();
        }

        // 精度設定できるようにしておくが、当画面／帳票は日本円前提なのでPrecisionは基本的に0固定
        private uint _AmountPrecision = 0;
        public uint AmountPrecision
        {
            get
            {
                return _AmountPrecision;
            }
            private set
            {
                _AmountPrecision = value;

                AmountDisplayFormat = "#,##0";
                if (value != 0)
                {
                    AmountDisplayFormat += "." + new string('0', (int)value);
                }
            }
        }

        public string AmountDisplayFormat { get; private set; } = "#,##0";

        public void SetBasicPageSetting(string companyCode, string companyName, uint precision = 0)
        {
            lblCompanyCodeName.Text = companyCode + " " + companyName;

            AmountPrecision = precision;
            txtBillingAmount.OutputFormat = AmountDisplayFormat;
            txtTransferAmount.OutputFormat = AmountDisplayFormat;
            txtTotalBillingAmount.OutputFormat = AmountDisplayFormat;
            txtTotalTransferAmount.OutputFormat = AmountDisplayFormat;
        }

        public void SetData(IEnumerable<ReportRow> reportRowList)
        {
            DataSource = new BindingSource(reportRowList, null);
        }

        /// <summary>
        /// 帳票の行データ
        /// </summary>
        public class ReportRow
        {
            // 請求書情報 ※ 登録後の更新値ではなく照合時の値を出力
            public Billing Billing { get; set; } // 照合できた請求データの中でIdが一番若いもの

            // 口座振替情報(ファイル記載情報)
            public int TransferResultCode { get; set; }
            public decimal TransferAmount { get; set; }
            public string TransferBankName { get; set; }
            public string TransferBranchName { get; set; }
            public string TransferCustomerCode { get; set; }
            public string TransferAccountName { get; set; }

            // 参照／加工生成
            public string BillingCustomerCode { get { return Billing?.CustomerCode; } }
            public string BillingCustomerName { get { return Billing?.CustomerName; } }
            public string BillingDepartmentCode { get { return Billing?.DepartmentCode; } }
            public string BillingDepartmentName { get { return Billing?.DepartmentName; } }
            public string BillingInvoiceCode { get { return Billing?.InvoiceCode; } }
            public string BillingNote1 { get { return Billing?.Note1; } }
            public DateTime? BillingBilledAt { get { return Billing?.BilledAt; } }
            public DateTime? BillingSalesAt { get { return Billing?.SalesAt; } }
            public DateTime? BillingClosingAt { get { return Billing?.ClosingAt; } }
            public DateTime? BillingDueAt { get { return Billing?.DueAt; } }

            /// <summary>
            /// 口座振替依頼データ作成/取込 で BillingAmount の取り扱いを変える
            /// 作成時は、合算処理した場合に、請求データ情報は出力しない
            /// </summary>
            public bool IsImport { get; set; } = true;

            // 合算設定時(Company.TransferAggregate = 1)、請求データ部には複数ある請求データのうちIdが
            // 一番若いデータだけを表示するが、請求金額欄には合算値を表示する必要がある。
            // 当画面／帳票に於いては請求金額＝引落金額となるので、合算処理は行わず引落金額から取得する。
            public decimal? BillingAmount
            {
                get
                {
                    return (!IsImport || Billing != null)
                        ? TransferAmount
                        : /* 照合不能→空欄 */ (decimal?)null;
                }
            }

            public string TransferResult { get { return TransferResultCode == 0 ? "振替済" : "振替不能"; } }
            /// <summary>
            /// 口座振替 抽出時検証結果
            /// </summary>
            public string CreateResult { get; set; }

        }
    }
} 