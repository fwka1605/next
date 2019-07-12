
    /// <summary>帳票設定 回収予定表</summary>
    export class PF0601
    {
        /// <summary>帳票設定 1 : 得意先集計方法</summary>
        public static CustomerType:number = 1;

        /// <summary>帳票設定 2 : 担当者集計方法</summary>
        public static StaffSelection:number = 2;

        /// <summary>帳票設定 3 : 担当者ごと改ページ</summary>
        public static StaffNewPage:number = 3;

        /// <summary>帳票設定 4 : 請求部門ごと改ページ</summary>
        public static DepartmentNewPage:number = 4;

        /// <summary>帳票設定 5 : 請求部門ごと表示</summary>
        public static DisplayDepartment:number = 5;

        /// <summary>帳票設定 6 : 金額単位</summary>
        public static UnitPrice:number = 6;
    }