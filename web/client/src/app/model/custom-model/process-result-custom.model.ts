import { PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from "../../common/const/status.const";
import { LogData } from "../log-data.model";
import { PartsResultMessageComponent } from "src/app/component/view-parts/parts-result-message/parts-result-message.component";

export class ProcessResultCustom {
    public isModal:boolean;
    public title: string;
    public action: string;
    public status: PROCESS_RESULT_STATUS_TYPE;
    public result: PROCESS_RESULT_RESULT_TYPE;
    public message: string;
    public logData: LogData;

    constructor(isModal:boolean=false){
      this.isModal=isModal;
      this.logData=new LogData();
    }

  public CopyMessage(original: ProcessResultCustom) {
    this.result = original.result;
    this.message = original.message;
  }
}

export class ExistDeleteResult {
    public masterName: string;
    public idName: string;
}