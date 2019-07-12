export class Amount {    
  /// <summary>金額 換算前の金額</summary>
  public value: number; 
  /// <summary>通貨 通貨コード JPY, USD, CNY など</summary>
  public currency: string;
  /// <summary>換算するターゲット通貨</summary>
  public target_currency: string;
  /// <summary>通貨レート</summary>
  public rate: number;
  /// <summary>通貨換算後の金額</summary>
  public converted_value: number;    
}