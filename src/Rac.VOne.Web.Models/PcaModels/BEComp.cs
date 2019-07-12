using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>会社基本情報</summary>
    public class BEComp
    {
        ///<summary>1.エンティティバージョン</summary>
        public int BEVersion { get; set; }
        ///<summary>2.会社ID</summary>
        public int Id { get; set; }
        ///<summary>3.会社コード</summary>
        public string Code { get; set; }
        ///<summary>4.会社ﾌﾘｶﾞﾅ</summary>
        public string Kana { get; set; }
        ///<summary>5.会社名</summary>
        public string Name { get; set; }
        ///<summary>6.郵便番号</summary>
        public string ZipCode { get; set; }
        ///<summary>7.住所１</summary>
        public string Address1 { get; set; }
        ///<summary>8.住所２</summary>
        public string Address2 { get; set; }
        ///<summary>9.住所１ﾌﾘｶﾞﾅ</summary>
        public string Address1Kana { get; set; }
        ///<summary>10.住所２ﾌﾘｶﾞﾅ</summary>
        public string Address2Kana { get; set; }
        ///<summary>11.TEL</summary>
        public string Tel { get; set; }
        ///<summary>12.FAX</summary>
        public string Fax { get; set; }
        ///<summary>13.期首日</summary>
        public IntDate StartDate { get; set; }
        ///<summary>14.期末日</summary>
        public IntDate EndDate { get; set; }
        ///<summary>15.期数</summary>
        public int Number { get; set; }
        ///<summary>16.使用する暦</summary>
        public string KoyomiMode { get; set; }
        ///<summary>17.勘定科目コード桁数</summary>
        public int KmkCodeLength { get; set; }
        ///<summary>18.勘定科目０詰め</summary>
        public string KmkCodeFillZero { get; set; }
        ///<summary>19.補助科目コード桁数</summary>
        public int HojoCodeLength { get; set; }
        ///<summary>20.補助科目０詰め</summary>
        public string HojoCodeFillZero { get; set; }
        ///<summary>21.製造原価勘定</summary>
        public string ProductionCostKmkMode { get; set; }
        ///<summary>22.部門管理
        /// 0: NotManage: 管理しない
        /// 1: AllKmk   : 全科目
        /// 2: PLKmk    : 損益計算書科目
        ///</summary>
        public string BuManageMode { get; set; }
        ///<summary>23.部門コード桁数</summary>
        public int BuCodeLength { get; set; }
        ///<summary>24.部門グループコード桁数</summary>
        public int BuGroupCodeLength { get; set; }
        ///<summary>25.伝票番号管理</summary>
        public string JournalNumberMode { get; set; }
        ///<summary>26.重複伝票番号</summary>
        public string JournalNumberCheckMode { get; set; }
        ///<summary>27.仕訳締切日付</summary>
        public IntDate JournalLockDate { get; set; }
        ///<summary>28.仕訳締切仕訳区分</summary>
        public string JournalLockJournalClass { get; set; }
        ///<summary>29.仕訳データ反映方法</summary>
        public string JournalApprovalMode { get; set; }
        ///<summary>30.残高への仕訳データ反映承認階層</summary>
        public string JournalApprovalRank { get; set; }
        ///<summary>31.承認階層1</summary>
        public string ApprovalRank1Level { get; set; }
        ///<summary>32.承認階層2</summary>
        public string ApprovalRank2Level { get; set; }
        ///<summary>33.承認階層3</summary>
        public string ApprovalRank3Level { get; set; }
        ///<summary>34.承認階層4</summary>
        public string ApprovalRank4Level { get; set; }
        ///<summary>35.自分入力仕訳</summary>
        public string ApprovalUserMode { get; set; }
        ///<summary>36.消費税管理</summary>
        public string TaxMode { get; set; }
        ///<summary>37.消費税自動計算</summary>
        public string TaxCalcMode { get; set; }
        ///<summary>38.消費税端数処理</summary>
        public string TaxRoundMode { get; set; }
        ///<summary>39.前期からの消費税の更新方法</summary>
        public string CarryingOverMode { get; set; }
        ///<summary>40.電子帳簿保存</summary>
        public string EBookSaveMode { get; set; }
        ///<summary>41.訂正削除の履歴を残さない日数</summary>
        public int NotDeleteHistoryDays { get; set; }
        ///<summary>42.合算領域フラグ</summary>
        public string CombinationMode { get; set; }
        ///<summary>43.前年度データ領域名</summary>
        public string PreviousAreaName { get; set; }
        ///<summary>44.既定の科目属性パターン</summary>
        public int DefaultKmkAttributePattern { get; set; }
        ///<summary>45.法人番号</summary>
        public string CorporateMyNumber { get; set; }
        ///<summary>46.データバージョン</summary>
        public string DataVersion { get; set; }
        ///<summary>47.予備1</summary>
        public int Reserve1 { get; set; }
        ///<summary>48.予備2</summary>
        public int Reserve2 { get; set; }
        ///<summary>49.予備3</summary>
        public int Reserve3 { get; set; }

        #region Web API 設定 会社基本情報表示用

        public string GetCompanyInfo() => string.Join("\r\n", new object[] {
            $"会社コード　　　　：{Code}",
            $"会社名　　　　　　：{Name}",
            $"勘定科目コード桁数：{KmkCodeLength}",
            $"勘定科目０詰め　　：０詰め{DoOrNot(KmkCodeFillZero)}",
            $"部門管理　　　　　：{BuManageModeName}",
            $"部門コード桁数　　：{BuCodeLength}",
        });
        private string DoOrNot(string value) => value == "true" ? "する" : "しない";
        private string BuManageModeName =>
            RequireDepartmentAllAccount ? "全科目" :
            RequireDepartmentPLAccount  ? "損益計算書科目" : "管理しない";
        /// <summary>全科目 部門コード必須</summary>
        public bool RequireDepartmentAllAccount => BuManageMode == "AllKmk";
        /// <summary>PL科目のみ 部門コード必須</summary>
        public bool RequireDepartmentPLAccount  => BuManageMode == "PLKmk";

        #endregion
    }

    /// <summary>会社基本情報 Web API 用</summary>
    public class BECompSet
    {
        public BEComp BEComp { get; set; }
    }
}
