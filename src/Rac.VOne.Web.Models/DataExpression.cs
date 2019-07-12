using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public class DataExpression
    {
        public DataExpression(ApplicationControl appCtrl)
        {
            if (appCtrl != null)
            {
                AccountTitleCodeFormatString = appCtrl.AccountTitleCodeType == 1 ? "A9" : "9";
                AccountTitleCodeLength = appCtrl.AccountTitleCodeLength;

                switch (appCtrl.CustomerCodeType)
                {
                    case 0: CustomerCodeFormatString = "9"; break;
                    case 1: CustomerCodeFormatString = "A9-_"; break;
                    case 2: CustomerCodeFormatString = "KA9-_"; break;
                }
                CustomerCodeLength = appCtrl.CustomerCodeLength;
                MinCustomerFee = appCtrl.UseForeignCurrency == 1 ? 0M : 1M;
                MaxCustomerFee = appCtrl.UseForeignCurrency == 1 ? 9999999.99999M : 9999999M;

                DepartmentCodeFormatString = appCtrl.DepartmentCodeType == 1 ? "A9" : "9";
                DepartmentCodeLength = appCtrl.DepartmentCodeLength;

                IgnoreKanaLength =140;

                LoginUserCodeFormatString = appCtrl.LoginUserCodeType == 1 ? "A9" : "9";
                LoginUserCodeLength = appCtrl.LoginUserCodeLength;

                SectionCodeFormatString = appCtrl.SectionCodeType == 1 ? "A9" : "9";
                SectionCodeLength = appCtrl.SectionCodeLength;

                StaffCodeFormatString = appCtrl.StaffCodeType == 1 ? "A9" : "9";
                StaffCodeLength = appCtrl.StaffCodeLength;

                UseDistribution = appCtrl.UseDistribution;
                UseForeignCurrency = appCtrl.UseForeignCurrency;
                UseReceiptSection = appCtrl.UseReceiptSection;
                UseDiscount = appCtrl.UseDiscount;
                GeneralSettingValue = appCtrl.GeneralSettingValue;
            }
        }

        public string GeneralSettingValue { get; set; }
        public static int CompanyCodeTypeGlobal { get; set; }
        public int CompanyCodeType
        {
            get { return CompanyCodeTypeGlobal; }
            set { CompanyCodeTypeGlobal = value; }
        }
        /// <summary>科目コード文字種類</summary>
        public string AccountTitleCodeFormatString { get; private set; }
        /// <summary>科目コード桁数</summary>
        public int AccountTitleCodeLength { get; private set; }

        /// <summary>得意先コード文字種類</summary>
        public string CustomerCodeFormatString { get; private set; }
        /// <summary>得意先コード桁数</summary>
        public int CustomerCodeLength { get; private set; }
        /// <summary>登録手数料最小値</summary>
        public decimal MinCustomerFee { get; private set; }
        /// <summary>登録手数料最大値</summary>
        public decimal MaxCustomerFee { get; private set; }

        /// <summary>請求部門コード文字種類</summary>
        public string DepartmentCodeFormatString { get; private set; }
        /// <summary>請求部門コード桁数</summary>
        public int DepartmentCodeLength { get; private set; }

        /// <summary>除外カナ桁数</summary>
        public int IgnoreKanaLength { get; private set; }

        /// <summary>担当者コード文字種類</summary>
        public string LoginUserCodeFormatString { get; private set; }
        /// <summary>担当者コード桁数</summary>
        public int LoginUserCodeLength { get; private set; }

        /// <summary>入金部門コード文字種類</summary>
        public string SectionCodeFormatString { get; private set; }
        /// <summary>入金部門コード桁数</summary>
        public int SectionCodeLength { get; private set; }

        /// <summary>営業担当者コード文字種類</summary>
        public string StaffCodeFormatString { get; private set; }
        /// <summary>営業担当者コード桁数</summary>
        public int StaffCodeLength { get; private set; }

        /// <summary>Mail Address</summary>
        public int UseDistribution { get; private set; }

        /// <summary>外貨対応</summary>
        public int UseForeignCurrency { get; private set; }

        /// <summary>入金部門管理</summary>
        public int UseReceiptSection { get; private set; }

        /// <summary> 請求歩引額 </summary>
        public int UseDiscount { get; private set; }

        /// <summary>TEL,FAX番号文字種類</summary>
        public string TelFaxFormatString { get { return "9-"; } }

        //public System.Windows.Forms.ImeMode CustomerCodeImeMode
        //    => CustomerCodeFormatString.Contains("K")
        //        ? System.Windows.Forms.ImeMode.KatakanaHalf
        //        : System.Windows.Forms.ImeMode.Disable;

        #region padding char
        public char? AccountTitleCodePaddingChar
            => AccountTitleCodeFormatString == "9" ? (char?)'0' : null;
        public char? CustomerCodePaddingChar
            => CustomerCodeFormatString == "9" ? (char?)'0' : null;
        public char? DepartmentCodePaddingChar
            => DepartmentCodeFormatString == "9" ? (char?)'0' : null;
        public char? LoginUserCodePaddingChar
            => LoginUserCodeFormatString == "9" ? (char?)'0' : null;
        public char? SectionCodePaddingChar
            => SectionCodeFormatString == "9" ? (char?)'0' : null;
        public char? StaffCodePaddingChar
            => StaffCodeFormatString == "9" ? (char?)'0' : null;
        #endregion
    }
}
