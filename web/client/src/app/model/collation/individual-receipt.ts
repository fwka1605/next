import { Receipt } from "../receipt.model";
import { CheckBoxStatus } from "src/app/common/const/kbn.const";

export class IndividualReceipt extends Receipt {

  constructor(){
    super();
    this.checkBox=1;
  }

  public checkBox:number=CheckBoxStatus.ON;



  
}

