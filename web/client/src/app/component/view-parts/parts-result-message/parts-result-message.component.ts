import { Component, OnInit, Input, ElementRef, OnDestroy } from '@angular/core';
import { ProcessResultCustom } from 'src/app/model/custom-model/process-result-custom.model';
import { BaseComponent } from '../../common/base/base-component';
import { StringUtil } from 'src/app/common/util/string-util';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';

@Component({
  selector: 'app-parts-result-message',
  templateUrl: './parts-result-message.component.html',
  styleUrls: ['./parts-result-message.component.css']
})
export class PartsResultMessageComponent implements OnInit,OnDestroy {


  @Input() processCustomResult:ProcessResultCustom;
  public canOpen:boolean;

  private messageInterval:any;

  constructor(
    public elementRef: ElementRef,
  ) { }

  ngOnInit() {
    
  }

  ngOnDestroy(){
    if(this.messageInterval!=null){
      clearInterval(this.messageInterval);
    }
  }

  public isSuccessDisplay():boolean{
    let rtn:boolean;
    
    if(this.processCustomResult==null) return false;
    if(StringUtil.IsNullOrEmpty(this.processCustomResult.message)) return false;

    if(this.processCustomResult.result!=PROCESS_RESULT_RESULT_TYPE.SUCCESS) return false;

    return true;
  }

  
  public isWarningDisplay():boolean{
    let rtn:boolean;
    
    if(this.processCustomResult==null) return false;
    if(StringUtil.IsNullOrEmpty(this.processCustomResult.message)) return false;

    if(this.processCustomResult.result!=PROCESS_RESULT_RESULT_TYPE.WARNING) return false;

    return true;
  }


  public isFailureDisplay():boolean{
    let rtn:boolean;
    
    if(this.processCustomResult==null) return false;
    if(StringUtil.IsNullOrEmpty(this.processCustomResult.message)) return false;

    if(this.processCustomResult.result!=PROCESS_RESULT_RESULT_TYPE.FAILURE) return false;

    return true;
  }

  public isVisibility():string{
    if(this.canOpen){
      return "visibility:visible";
    }
    else{
      return "visibility:hidden";
    }
  }

  public closeMessage(){
    this.canOpen=false;
  }

  public openMessage(){

    this.canOpen=true;

    /*
    // 5秒後にFalse
    let message = this.elementRef.nativeElement.ownerDocument.getElementById("successMessage");

    if(message!=null && message.style !=undefined){
      message.style.opacity = "1.0";
    }


    this.messageInterval = setInterval(() => {
      let message = this.elementRef.nativeElement.ownerDocument.getElementById("successMessage");
      if(this.isSuccessDisplay()&&(message!=null && message.style !=undefined)){

        if(StringUtil.IsNullOrEmpty(message.style.opacity)){
          message.style.opacity = "0.95";
        }
        else{
          let opacity:number= parseFloat(message.style.opacity)-0.05;
          message.style.opacity = ""+opacity;

          if(opacity==0){
            this.canOpen=false;
            clearInterval(this.messageInterval);
          }
        }

        //this.canOpen=false;
      }
    }, 500);
    */

  }

}
