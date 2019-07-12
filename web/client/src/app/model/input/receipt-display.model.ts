import { Category } from "../category.model";

export class ReceiptDisplay {

  public id:number;
  
  public receiptCategoryId:string;
  public receiptCategoryCode:string;
  public receiptCategory:Category;


  public notInputDueFlag:boolean;

  public receiptAmount:string;

  public dueAt:string;
  public note1:string;
  public note2:string;
  public note3:string;
  public note4:string;
  public billNumber:string;
  public billBankCode:string;
  public billBranchCode:string;
  public billBank:string;
  public billDrawAt:string;
  public billDrawer:string;

  public noteFlag:boolean;
  public memo:string;




}
