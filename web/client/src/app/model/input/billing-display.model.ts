
export class BillingDisplay {

  public salesAt: string;             // 売上日
  public billingCategory: string;     // 請求区分
  public taxClass: string;            // 税区分
  public debitAccountTitle:number;    // 債権科目
  public note1:string;                // メモ
  public memo: string;                // 備考
  public note2:string;                // メモ
  public note3:string;                // メモ
  public note4:string;                // メモ
  public note5:string;                // メモ
  public note6:string;                // メモ
  public note7:string;                // メモ
  public note8:string;                // メモ
  public billingId: string;      // 請求ID
  public contractNumber: string; // 契約番号
  public quantity: string;       // 数量
  public unitSymbol: string;     // 単位
  public unitPrice: string;      // 単価
  public billingNoTaxAmount: string;  // 請求額(抜)
  public taxAmount: string;      // 消費税
  public billingAmount: string;  // 請求額  

  public billingCategoryId:string // 請求区分ID
  public billingCategoryCode:string;  // 請求区分
  public taxClassId: string;  // 税区分
  public debitAccountTitleId: string;  // 債権科目


}
