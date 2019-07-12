using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>帳票設定 回収予定表</summary>
    public class PF0601
    {
        /// <summary>帳票設定 1 : 得意先集計方法</summary>
        public static int CustomerType => 1;

        /// <summary>帳票設定 2 : 担当者集計方法</summary>
        public static int StaffSelection => 2;

        /// <summary>帳票設定 3 : 担当者ごと改ページ</summary>
        public static int StaffNewPage => 3;

        /// <summary>帳票設定 4 : 請求部門ごと改ページ</summary>
        public static int DepartmentNewPage => 4;

        /// <summary>帳票設定 5 : 請求部門ごと表示</summary>
        public static int DisplayDepartment => 5;

        /// <summary>帳票設定 6 : 金額単位</summary>
        public static int UnitPrice => 6;
    }
}
