using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>
    /// 債権総額管理表 の 帳票設定
    /// </summary>
    public class PF0201
    {
        /// <summary>1 : 集計基準日</summary>
        public static int TargetDate => 1;
        /// <summary>2 : 債権総額計算方法</summary>
        public static int ReceiptType => 2;
        /// <summary>3 : 得意先集計方法</summary>
        public static int CustomerGroupType => 3;
        /// <summary>4 : 担当者集計方法</summary>
        public static int StaffType => 4;
        /// <summary>5 : 部門計要否</summary>
        public static int DepartmentTotal => 5;
        /// <summary>6 : 担当者計要否</summary>
        public static int StaffTotal => 6;
        /// <summary>7 : 得意先コード印字要否</summary>
        public static int DisplayCustomerCode => 7;
        /// <summary>8 : 金額単位</summary>
        public static int UnitPrice => 8;
        /// <summary>9 : 与信限度額集計方法 /// </summary>
        public static int CreditLimitType => 9;
    }
}
