import { Component, OnInit, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-modal-router-progress',
  templateUrl: './modal-router-progress.component.html',
  styleUrls: ['./modal-router-progress.component.css']
})
export class ModalRouterProgressComponent extends BaseComponent implements OnInit {

  constructor(
    private processResultService:ProcessResultService
  ) {
    super();
   }

  private closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }

  public set Closing(value) {
    this.closing = value;
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  ngOnInit() {
  }

}
