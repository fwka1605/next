import { MatchingOrder } from "../matching-order.model";
import { CustomerFee } from "../customer-fee.model";
import { PaymentAgencyFee } from "../payment-agency-fee.model";
import { CollationSetting } from "../collation-setting.model";
import { CollationSearch } from "../collation-search.model";
import { CollationData } from "./collation-data.model";
import { MatchingHeader } from "../matching-header.model";
import {Section} from '../section.model';
import { Department } from "../department.model";
import { eSortType } from "./sort-item";
import { Billing } from "../billing.model";
import { Receipt } from "../receipt.model";
import { ReceiptInput } from "../receipt-input.model";

export class CollationInfo{
  
  // 選択された請求・入金部門のリスト
  public sectionIds:Array<number>;
  public sections:Array<Section> 
  public sectionsWithLoginUser:Array<Section> 
  public departmentIds:Array<number>;
  public departments:Array<Department> 
  public departmentsWithLoginUser:Array<Department> 

  /*
    管理マスタの値
  */
  /// <summary>入金予定日 算出用 現在日付 + N 日の値 管理マスター：回収予定範囲 の値</summary>
  public billingDueAtOffset:number;
  /// <summary>手数料誤差範囲 通貨 JPY 日本円の場合に 管理マスター：手数料誤差 の値を利用</summary>
  public bankFeeTolerance:number;
  /// <summary>消費税誤差 一括消込時の許容誤差。入金額±誤差まで消込可</summary>
  public taxDifferenceTolerance:number;


  /// <summary>照合設定</summary>
  public collationSetting:CollationSetting;
  public clientKey:string;//byte[];
  public collationSearch = new  CollationSearch();

  public collations:Array<CollationData>;
  public matchingHeaders:Array<MatchingHeader>;

  /// <summary>小数点以下桁数</summary>
  public formatNumber:number;
  public matchedIndex = new Array<number>();

  /// <summary>照合後、自動的に消込オプション有効時の対応</summary>
  /// <remarks>
  /// 一括消込画面で、消込完了が一度でも行われたら、trueにする
  /// 照合後、自動消込の機能が有効な場合、消込解除の後、照合処理を実行すると、再度消込れる現象の回避用
  /// </remarks>
  public matchingCancelExecuted:boolean;

  /// <summary>消込完了データを表示</summary>
  public isMatched:boolean;// { get { return cbxShowMatched.Checked; } }
  public searchCondition:Array<object>;
  public legalPersonalities:Array<string>;
  public matchingBillingOrders:Array<MatchingOrder>;
  public matchingReceiptOrders:Array<MatchingOrder>;

  public sortType:eSortType = eSortType.None;

  /// <summary>登録済 得意先手数料 key CustomerId</summary>
  public customerFees:CustomerFee[]; 

  /// <summary>登録済 決済代行会社手数料 key PaymentAgencyId</summary>
  public paymentAgencyFees:PaymentAgencyFee[];

  //public Color MatchingGridBillingBackColor { get; set; }
  //public Color MatchingGridReceiptBackColor { get; set; }
  //public Color ControlDisableBackColor { get; set; }
  //public Color GridLineColor { get; set; }
  public gridDefaultWidth:number;
  public nameWidth:number;// { get { return UseForeignCurrency ? 125 : 145; } }
  public billingPriority:boolean = true;

  public billingReceiptOrder:boolean;// { get { return this.collationSetting.BillingReceiptDisplayOrder == 0; } }


  public collateButtonName:string;
  public matchingButtonName:string;
  public resutlTitle:string;
  public selectionAlias: string;
  public noteAlias: string;

  // 現在操作中のCollationのIndex
  public individualIndexNo:number;


  /////////////////////////////////////
  // 検索を行った請求情報
  ////////////////////////////////////
  public searchBillings:Array<Billing>;

  /////////////////////////////////////
  // 検索を行った入金情報・登録を行った入金情報
  ////////////////////////////////////
  public searchReceipts:Array<Receipt>;
  public registryReceipts:Array<Receipt>;
  public editRecipt:Receipt;
  public deleteReceiptId:number;


  public displayTypeVertical:boolean=true;

}