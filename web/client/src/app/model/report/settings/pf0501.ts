
/// <summary>得意先別消込台帳 の 帳票設定</summary>
export class PF0501
{

  /// <summary>1 : 集計基準日</summary>
  public static TargetDate:number = 1;
  /// <summary>2 : 請求データ集計</summary>
  public static TotalByDay:number = 2;
  /// <summary>3 : 月次改ページ</summary>
  public static MonthlyBreak:number = 3;
  /// <summary>4 : 伝票集計方法</summary>
  public static SlipTotal:number = 4;
  /// <summary>5 : 入金データ集計</summary>
  public static ReceiptGrouping:number = 5;
  /// <summary>6 : 請求残高計算方法</summary>
  public static RemainType:number = 6;
  /// <summary>7 : 請求部門コード表示</summary>
  public static DisplayDepartmentCode:number = 7;
  /// <summary>8 : 入金部門コード表示</summary>
  public static DisplaySectionCode:number = 8;
  /// <summary>9 : 金額単位</summary>
  public static UnitPrice:number = 9;
  /// <summary>10: 消込記号表示</summary>
  public static DisplaySymbol:number = 10;

}
