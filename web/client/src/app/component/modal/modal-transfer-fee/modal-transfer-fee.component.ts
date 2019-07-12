import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BaseComponent } from '../../common/base/base-component';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomerFeeMasterService } from 'src/app/service/Master/customer-fee-master.service';
import { CustomerFeeSearch } from 'src/app/model/customer-fee-search.model';
import { CustomerFeesResult } from 'src/app/model/customer-fees-result.model';
import { CustomerFee } from 'src/app/model/customer-fee.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';

@Component({
  selector: 'app-modal-transfer-fee',
  templateUrl: './modal-transfer-fee.component.html',
  styleUrls: ['./modal-transfer-fee.component.css']
})
export class ModalTransferFeeComponent extends BaseComponent implements OnInit, AfterViewInit {
  /** 登録手数料 */
  public feeNumberCtrl: FormControl;
  /** 登録手数料データ */
  public customerFeesResult: CustomerFeesResult;

  /** 得意先マスターID */
  public customerId: number;
  public get CustomerId() {
    return this.customerId;
  }
  public set CustomerId(value) {
    this.customerId = value;
  }

  /** モーダルを閉じる */
  public closing = new EventEmitter<{}>();
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

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  constructor(
    public elementRef: ElementRef,
    public userInfoService: UserInfoService,
    public customerFeeService: CustomerFeeMasterService,
    public processResultService: ProcessResultService

  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.getCustomerFeeData();
  }

  ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'feeNumberCtrl', EVENT_TYPE.NONE);

  }

  public setControlInit() {
    this.feeNumberCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      feeNumberCtrl: this.feeNumberCtrl
    });
  }



  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processModalCustomResult, action);
    if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }  
  /**
   * データ取得
   */
  public getCustomerFeeData() {
    let search = new CustomerFeeSearch();
    search.customerId = this.customerId;

    this.customerFeeService.Get(search)
      .subscribe(result => {
        this.customerFeesResult = new CustomerFeesResult();
        this.customerFeesResult.customerFees = new Array<CustomerFee>();
        this.customerFeesResult.customerFees = result;

        for (let i = 0; i < this.customerFeesResult.customerFees.length; i++) {
          let convertCreateAt = DateUtil.convertDateString(this.customerFeesResult.customerFees[i].createAt);
          this.customerFeesResult.customerFees[i].createAt = convertCreateAt;
        }
      });
  }

  /**
   * データ登録
   */
  public registry() {
    if (this.feeNumberCtrl.value.length <= 0) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '登録するデータ'),
        this.partsResultMessageComponent);
      return;
    }

    let registryDatas = new Array<CustomerFee>();
    let registryData = new CustomerFee;
    registryData.customerId = this.customerId;
    registryData.currencyId = this.userInfoService.Currency.id;
    registryData.newFee = this.feeNumberCtrl.value;
    registryData.createBy = this.userInfoService.LoginUser.id;
    registryData.updateBy = this.userInfoService.LoginUser.id;
    registryDatas.push(registryData);

    this.customerFeeService.Save(registryDatas)
      .subscribe(result => {
        this.processModalCustomResult = this.processResultService.processAtSave(
          this.processModalCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.getCustomerFeeData();
        }
      });
  }

}
