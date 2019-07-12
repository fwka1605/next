import { Billing } from "../billing.model";

export class IndividualBilling extends Billing {

  
  constructor(){
    super();
    this.checkBox=1;
  }

  public checkBox:number=1;
}

