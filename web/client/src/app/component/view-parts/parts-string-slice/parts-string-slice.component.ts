import { Component, OnInit, Input } from '@angular/core';
import { NumberUtil } from 'src/app/common/util/number-util';

import { StringUtil } from 'src/app/common/util/string-util';
import { BaseComponent } from '../../common/base/base-component';

@Component({
  selector: 'app-parts-string-slice',
  templateUrl: './parts-string-slice.component.html',
  styleUrls: ['./parts-string-slice.component.css']
})
export class PartsStringSliceComponent extends BaseComponent implements OnInit {

  constructor() { super();}

  @Input() inputData:string;
  @Input() inputWidth:string;

  

  public outputShortData;
  public outputLongData;

  ngOnInit() {

    let dataCount = NumberUtil.ParseInt(this.inputWidth.replace('px','').replace('em',''));
    dataCount = Math.floor(dataCount/16);
    //dataCount = 6;    

    if(this.inputData.length>dataCount){
      this.outputShortData = this.inputData.substring(0,dataCount)+"...";
      this.outputLongData = this.inputData;
    }
    else{
      this.outputShortData = null;
      this.outputLongData = this.inputData;
    }
  }

}
