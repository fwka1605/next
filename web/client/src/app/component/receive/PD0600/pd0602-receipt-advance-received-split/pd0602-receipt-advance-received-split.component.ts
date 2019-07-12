import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { NavigationEnd, Router, ActivatedRoute, ParamMap } from '@angular/router';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { PageUtil } from 'src/app/common/util/page-util';
import { FormatterUtil, } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { ColumnNameSettigUtil } from 'src/app/common/util/column-name-setting-util';
import { NumberUtil } from 'src/app/common/util/number-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { CategoryType, CODE_TYPE, MATCHING_ASSIGNMENT_FLAG_DICTIONARY, RECEIPT_STATUS_MESSAGE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { MSG_ERR, MSG_WNG, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ReceiptService } from 'src/app/service/receipt.service';
import { Receipt } from 'src/app/model/receipt.model';
import { AdvanceReceiptsResult } from 'src/app/model/advance-receipts-result.model';
import { AdvanceReceivedBackupService } from 'src/app/service/advance-received-backup.service';
import { AdvanceReceivedBackup } from 'src/app/model/advance-received-backup.model';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { forkJoin } from 'rxjs';
import { AdvanceReceivedSplit } from 'src/app/model/advance-received-split.model'
import { AdvanceReceivedSplitSource } from 'src/app/model/advance-received-split-source.model'
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { DatePipe }     from '@angular/common';

const MaxRowCount = 10;

/** 前受金の状態 */
enum ADVANCE_RECEIPT_STATE {
  /** 計上仕訳未出力の場合 */
  STATE23,
  /** 計上仕訳出力前に振替・分割済で、計上仕訳出力済の場合 */
  STATE4,
  /** 計上仕訳出力済で、振替・分割していない場合 */
  STATE5,
  /** 計上仕訳出力済で、振替・分割実行済、振替仕訳未出力 */
  STATE6,
  /** 計上仕訳出力済で、振替・分割実行済、振替仕訳出力済 */
  STATE7,
}

@Component({
  selector: 'app-pd0602-receipt-advance-received-split',
  templateUrl: './pd0602-receipt-advance-received-split.component.html',
  styleUrls: ['./pd0602-receipt-advance-received-split.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pd0602ReceiptAdvanceReceivedSplitComponent extends BaseComponent implements OnInit, AfterViewInit {

  public MATCHING_ASSIGNMENT_FLAG_DICTIONARY: typeof MATCHING_ASSIGNMENT_FLAG_DICTIONARY = MATCHING_ASSIGNMENT_FLAG_DICTIONARY;

  public advanceReceiptState: ADVANCE_RECEIPT_STATE;

  public columnNameSettingsResult: ColumnNameSettingsResult;

  public originalReceipt: Receipt;
  public advanceReceipts: Array<Receipt>;
  public advanceReceiptsResult: AdvanceReceiptsResult;
  public advanceReceivedBackup: AdvanceReceivedBackup;

  // 行の追加
  public addRowsGroup: FormGroup;
  public processingAtCtrl: FormControl;   //  処理予定日
  public customerCodeCtrl: FormControl;   //  得意先コード
  public receiptAmountCtrl: FormControl;  //  金額
  public numberOfLineCtrl: FormControl;   //  行数
  public customerName: string;            //  得意先名格納用
  public customerId: number               //  得意先Id格納用

  // 前受金情報
  public outputAtCtrl: FormControl;             //  計上仕訳日
  public transferDateCtrl: FormControl;         //  振替処理年月日
  public cbxTransferOutputAtCtrl: FormControl;  //  振替仕訳有
  public transferOutputAtCtrl: FormControl;     //  振替仕訳日

  //  明細
  public cbxCancelAdvanceReceivedCtrls: Array<FormControl>  // チェック
  public detailAssignmentFlagCtrls: Array<FormControl>      // 消込区分
  public detailProcessingAtCtrls: Array<FormControl>        // 処理予定日
  public detailCustomerCodeCtrls: Array<FormControl>        // 得意先コード
  public detailCustomerNameCtrls: Array<FormControl>        // 得意先名
  public detailCustomerIdCtrls: Array<FormControl>          // 得意先Id
  public detailReceiptAmountCtrls: Array<FormControl>       // 入金額
  public detailMemoCtrls: Array<FormControl>    // メモ
  public detailNote1Ctrls: Array<FormControl>   // 備考
  public detailNote2Ctrls: Array<FormControl>   // 備考2
  public detailNote3Ctrls: Array<FormControl>   // 備考3
  public detailNote4Ctrls: Array<FormControl>   // 備考4

  //  フッター
  public transferAmountTotalCtrl: FormControl;  //  振替額合計
  public splitRemainingCtrl: FormControl;       //  残額

  public registryDisableFlag: boolean = false;    //  登録
  public rowDeleteDisableFlag: boolean = true;    //  削除
  public cancelSplitDisableFlag: boolean = true;  //  振替取消
  public reDisplayDisableFlag: boolean = false;   //  再表示
  public selectAllDisableFlag: boolean = false;   //  全選択・全解除

  public addLineBtnDisableFlag: boolean = false;

  public errorMsg: string;

  public transferAmount: number;
  public remainingAmount: number;

  @ViewChild('customerCodeInput', { read: MatAutocompleteTrigger }) customerCodeTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public processResultService: ProcessResultService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public receiptService: ReceiptService,
    public advanceReceivedBackupService: AdvanceReceivedBackupService,
    public customerService: CustomerMasterService,
    private datePipe: DatePipe,
    private formBuilder: FormBuilder,
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title,true);
      }
    });

   }

  ngOnInit() {

    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.setAutoComplete();
    this.clear();
    this.loadData();

  }

  ngAfterViewInit(){
  }

  public loadData() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let originalReceiptId: number;

    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      originalReceiptId = parseInt(params.get("id"));
    });

    let columnNameSettingResponse = this.columnNameSettingService.Get(CategoryType.Receipt);
    let advanceReceiptsResponse = this.receiptService.GetAdvanceReceipts(originalReceiptId);
    let advanceReceivedBackupResponse = this.advanceReceivedBackupService.GetByOriginalReceiptId(originalReceiptId);

    forkJoin(
      columnNameSettingResponse,
      advanceReceiptsResponse,
      advanceReceivedBackupResponse,
    )
      .subscribe(responseList => {

        if (responseList != undefined && responseList.length == 3) {

          this.columnNameSettingsResult = new ColumnNameSettingsResult();
          this.columnNameSettingsResult.columnNames = responseList[0];

          this.advanceReceiptsResult = new AdvanceReceiptsResult();
          this.advanceReceiptsResult = responseList[1];
          this.originalReceipt = this.advanceReceiptsResult.originalReceipt;
          this.advanceReceipts = this.advanceReceiptsResult.advanceReceipts;

          this.advanceReceivedBackup = new AdvanceReceivedBackup();
          this.advanceReceivedBackup = responseList[2];

          if (this.advanceReceipts.length > 0) {
            this.initializeDetailsControl();
            this.setAdvanceReceiptState();
            this.setScreenMode();
            this.setSum();

            modalRouterProgressComponentRef.destroy();
          }

        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '初期化'),
            this.partsResultMessageComponent);

          modalRouterProgressComponentRef.destroy();
        }

      });

  }

  public setControlInit() {

    this.processingAtCtrl = new FormControl("", [Validators.maxLength(10)]);  //  処理予定日
    this.customerCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);  //  得意先コード
    this.receiptAmountCtrl = new FormControl("");  //  金額
    this.numberOfLineCtrl = new FormControl("", [Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)]);  //  行数

    this.outputAtCtrl = new FormControl("");  //  計上仕訳日
    this.transferDateCtrl = new FormControl("", [Validators.required]);  //  振替処理年月日
    this.cbxTransferOutputAtCtrl = new FormControl("");  //  振替仕訳有
    this.transferOutputAtCtrl = new FormControl("");  //  振替仕訳日

    this.transferAmountTotalCtrl = new FormControl("");
    this.splitRemainingCtrl = new FormControl("");

  }

  public setValidator() {
    this.addRowsGroup = this.formBuilder.group({
      processingAtCtrl: this.processingAtCtrl,
      customerCodeCtrl: this.customerCodeCtrl,
      receiptAmountCtrl: this.receiptAmountCtrl,
      numberOfLineCtrl: this.numberOfLineCtrl,
    });
    this.MyFormGroup = new FormGroup({
      addRowsGroup: this.addRowsGroup,

      outputAtCtrl: this.outputAtCtrl,
      transferDateCtrl: this.transferDateCtrl,
      cbxTransferOutputAtCtrl: this.cbxTransferOutputAtCtrl,
      transferOutputAtCtrl: this.transferOutputAtCtrl,

      transferAmountTotalCtrl: this.transferAmountTotalCtrl,
      splitRemainingCtrl: this.splitRemainingCtrl,

    });

  }

  public setFormatter() {

    //FormatterUtil.setAlphabetFormatter(this.orgCurrencyCodeCtrl); // 通貨コード

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeCtrl); // 得意先コード
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeCtrl);
    }

    FormatterUtil.setCurrencyFormatter(this.receiptAmountCtrl, false); //  金額
    FormatterUtil.setNumberFormatter(this.numberOfLineCtrl);    // 行数

  }

  public setAutoComplete() {
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeCtrl,this.customerService,0);
  }

  public initializeDetailsControl() {

    const pipe = new RacCurrencyPipe();
    const detailCount = this.advanceReceipts.length;

    this.cbxCancelAdvanceReceivedCtrls = new Array(detailCount);
    this.detailAssignmentFlagCtrls = new Array(detailCount);
    this.detailProcessingAtCtrls = new Array(detailCount);
    this.detailCustomerCodeCtrls = new Array(detailCount);
    this.detailCustomerNameCtrls = new Array(detailCount);
    this.detailCustomerIdCtrls = new Array(detailCount);
    this.detailReceiptAmountCtrls = new Array(detailCount);
    this.detailMemoCtrls = new Array(detailCount);
    this.detailNote1Ctrls = new Array(detailCount);
    this.detailNote2Ctrls = new Array(detailCount);
    this.detailNote3Ctrls = new Array(detailCount);
    this.detailNote4Ctrls = new Array(detailCount);

    for (let i = 0; i < detailCount; i++) {
      this.cbxCancelAdvanceReceivedCtrls[i] = new FormControl();
      this.detailAssignmentFlagCtrls[i] = new FormControl();
      if (this.advanceReceipts[i].processingAt != undefined) {
        let date = new Date(DateUtil.getYYYYMMDD(1, this.advanceReceipts[i].processingAt));
        this.detailProcessingAtCtrls[i] = new FormControl(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));
      }
      else {
        this.detailProcessingAtCtrls[i] = new FormControl();
      }
      this.detailCustomerCodeCtrls[i] = new FormControl(this.advanceReceipts[i].customerCode, [Validators.required]);
      this.detailCustomerNameCtrls[i] = new FormControl(this.advanceReceipts[i].customerName);
      this.detailCustomerIdCtrls[i] = new FormControl(this.advanceReceipts[i].customerId);
      this.detailReceiptAmountCtrls[i] = new FormControl(pipe.transform(this.advanceReceipts[i].receiptAmount, false), Validators.min(0));
      this.detailMemoCtrls[i] = new FormControl(this.advanceReceipts[i].receiptMemo);
      this.detailNote1Ctrls[i] = new FormControl(this.advanceReceipts[i].note1);
      this.detailNote2Ctrls[i] = new FormControl(this.advanceReceipts[i].note2);
      this.detailNote3Ctrls[i] = new FormControl(this.advanceReceipts[i].note3);
      this.detailNote4Ctrls[i] = new FormControl(this.advanceReceipts[i].note4);

      this.MyFormGroup.removeControl("cbxCancelAdvanceReceivedCtrl" + i);
      this.MyFormGroup.removeControl("detailAssignmentFlagCtrl" + i);
      this.MyFormGroup.removeControl("detailProcessingAtCtrl" + i);
      this.MyFormGroup.removeControl("detailCustomerCodeCtrl" + i);
      this.MyFormGroup.removeControl("detailCustomerNameCtrl" + i);
      this.MyFormGroup.removeControl("detailCustomerIdCtrl" + i);
      this.MyFormGroup.removeControl("detailReceiptAmountCtrl" + i);
      this.MyFormGroup.removeControl("detailMemoCtrl" + i);
      this.MyFormGroup.removeControl("detailNote1Ctrl" + i);
      this.MyFormGroup.removeControl("detailNote2Ctrl" + i);
      this.MyFormGroup.removeControl("detailNote3Ctrl" + i);
      this.MyFormGroup.removeControl("detailNote4Ctrl" + i);

      this.MyFormGroup.addControl("cbxCancelAdvanceReceivedCtrl" + i, this.cbxCancelAdvanceReceivedCtrls[i]);
      this.MyFormGroup.addControl("detailAssignmentFlagCtrl" + i, this.detailAssignmentFlagCtrls[i]);
      this.MyFormGroup.addControl("detailProcessingAtCtrl" + i, this.detailProcessingAtCtrls[i]);
      this.MyFormGroup.addControl("detailCustomerCodeCtrl" + i, this.detailCustomerCodeCtrls[i]);
      this.MyFormGroup.addControl("detailCustomerNameCtrl" + i, this.detailCustomerNameCtrls[i]);
      this.MyFormGroup.addControl("detailCustomerIdCtrl" + i, this.detailCustomerIdCtrls[i]);
      this.MyFormGroup.addControl("detailReceiptAmountCtrl" + i, this.detailReceiptAmountCtrls[i]);
      this.MyFormGroup.addControl("detailMemoCtrl" + i, this.detailMemoCtrls[i]);
      this.MyFormGroup.addControl("detailNote1Ctrl" + i, this.detailNote1Ctrls[i]);
      this.MyFormGroup.addControl("detailNote2Ctrl" + i, this.detailNote2Ctrls[i]);
      this.MyFormGroup.addControl("detailNote3Ctrl" + i, this.detailNote3Ctrls[i]);
      this.MyFormGroup.addControl("detailNote4Ctrl" + i, this.detailNote4Ctrls[i]);

    }

    for (let i = 0; i < detailCount; i++) {
      FormatterUtil.setCurrencyFormatter(this.detailReceiptAmountCtrls[i], false);
    }

  }

  public setControlValuesToModels() {
    for (let index = 0; index < this.cbxCancelAdvanceReceivedCtrls.length; index++) {
      if (this.advanceReceipts.length <= index) return;

      if (this.detailProcessingAtCtrls[index].value)  this.advanceReceipts[index].processingAt = DateUtil.ConvertFromDatepicker(this.detailProcessingAtCtrls[index]);
      if (this.detailCustomerCodeCtrls[index].value) this.advanceReceipts[index].customerCode = this.detailCustomerCodeCtrls[index].value;
      if (this.detailCustomerNameCtrls[index].value) this.advanceReceipts[index].customerName = this.detailCustomerNameCtrls[index].value;
      if (this.detailCustomerIdCtrls  [index].value) this.advanceReceipts[index].customerId   = this.detailCustomerIdCtrls  [index].value;
      if (this.detailReceiptAmountCtrls[index].value) this.advanceReceipts[index].receiptAmount = NumberUtil.ParseInt(this.detailReceiptAmountCtrls[index].value);
      if (this.detailMemoCtrls [index].value) this.advanceReceipts[index].receiptMemo = this.detailMemoCtrls[index].value;
      if (this.detailNote1Ctrls[index].value) this.advanceReceipts[index].note1 = this.detailNote1Ctrls[index].value;
      if (this.detailNote2Ctrls[index].value) this.advanceReceipts[index].note2 = this.detailNote2Ctrls[index].value;
      if (this.detailNote3Ctrls[index].value) this.advanceReceipts[index].note3 = this.detailNote3Ctrls[index].value;
      if (this.detailNote4Ctrls[index].value) this.advanceReceipts[index].note4 = this.detailNote4Ctrls[index].value;
    }
  }

  public setAdvanceReceiptState() {

    if (this.advanceReceipts[0].outputAt == undefined && this.advanceReceivedBackup == undefined) {
      this.advanceReceiptState = ADVANCE_RECEIPT_STATE.STATE23;
      return;
    }

    if (this.advanceReceivedBackup == undefined) {
      if (this.advanceReceipts.length > 1) {
        this.advanceReceiptState = ADVANCE_RECEIPT_STATE.STATE4;
      }
      else {
        this.advanceReceiptState = ADVANCE_RECEIPT_STATE.STATE5;
      }
    }
    else {
      if (this.advanceReceivedBackup.transferOutputAt == undefined) {
        this.advanceReceiptState = ADVANCE_RECEIPT_STATE.STATE6;
      }
      else {
        this.advanceReceiptState = ADVANCE_RECEIPT_STATE.STATE7;
      }
    }

  }

  public setScreenMode() {

    switch (this.advanceReceiptState) {

      case ADVANCE_RECEIPT_STATE.STATE23:
      {
        this.transferDateCtrl.disable();
        HtmlUtil.nextFocusByName(this.elementRef, 'processingAtCtrl', EVENT_TYPE.NONE);
        break;
      }
      case ADVANCE_RECEIPT_STATE.STATE4:
      {
        this.setAddLineGroupControlsDisable();
        this.setDetailsControlsDisable();
        this.outputAtCtrl.setValue(this.advanceReceipts[0].outputAt);

        this.transferDateCtrl.disable();
        this.registryDisableFlag = true;
        this.reDisplayDisableFlag = true;
        this.selectAllDisableFlag = true;

        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.JOURNALIZED_CANNOT_UPDATE_AND_DELETE.replace(MSG_ITEM_NUM.FIRST, '編集'),
          this.partsResultMessageComponent);

        break;
      }
      case ADVANCE_RECEIPT_STATE.STATE5:
      {
        this.outputAtCtrl.setValue(this.advanceReceipts[0].outputAt);
        this.transferDateCtrl.enable();
        HtmlUtil.nextFocusByName(this.elementRef, 'processingAtCtrl', EVENT_TYPE.NONE);
        break;
      }
      case ADVANCE_RECEIPT_STATE.STATE6:
      {
        this.outputAtCtrl.setValue(this.advanceReceivedBackup.outputAt);
        let date = new Date(this.datePipe.transform(this.advanceReceipts[0].recordedAt , 'yyyy/MM/dd'));
        this.transferDateCtrl.setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));

        if (this.allDetailsCanEdit()) {
          this.transferDateCtrl.enable();
          this.cancelSplitDisableFlag = false;
        }
        else {
          this.transferDateCtrl.disable();
          this.cancelSplitDisableFlag = true;
        }

        this.cbxTransferOutputAtCtrl.setValue(true);

        HtmlUtil.nextFocusByName(this.elementRef, 'processingAtCtrl', EVENT_TYPE.NONE);
        break;
      }
      case ADVANCE_RECEIPT_STATE.STATE7:
      {
        this.setAddLineGroupControlsDisable();
        this.setDetailsControlsDisable();

        this.outputAtCtrl.setValue(this.advanceReceivedBackup.outputAt);
        this.transferOutputAtCtrl.setValue(this.advanceReceivedBackup.transferOutputAt);

        let date = new Date(this.datePipe.transform(this.advanceReceipts[0].recordedAt , 'yyyy/MM/dd'));
        this.transferDateCtrl.setValue(new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate()));
        this.transferDateCtrl.disable();
        this.cbxTransferOutputAtCtrl.setValue(true);

        this.registryDisableFlag = true;
        this.reDisplayDisableFlag = true;
        this.selectAllDisableFlag = true;

        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.JOURNALIZED_CANNOT_UPDATE_AND_DELETE.replace(MSG_ITEM_NUM.FIRST, '編集'),
          this.partsResultMessageComponent);

        break;
      }

    }

  }

  public setAddLineGroupControlsDisable() {

    this.processingAtCtrl.disable();
    this.customerCodeCtrl.disable();
    this.receiptAmountCtrl.disable();
    this.numberOfLineCtrl.disable();
    this.addLineBtnDisableFlag = true;
  }

  public setDetailsControlsDisable() {

    for (let i = 0; i < this.cbxCancelAdvanceReceivedCtrls.length; i++) {
      this.cbxCancelAdvanceReceivedCtrls[i].disable();
      this.detailProcessingAtCtrls[i].disable();
      this.detailCustomerCodeCtrls[i].disable();
      this.detailReceiptAmountCtrls[i].disable();
      this.detailMemoCtrls[i].disable();
      this.detailNote1Ctrls[i].disable();
      this.detailNote2Ctrls[i].disable();
      this.detailNote3Ctrls[i].disable();
      this.detailNote4Ctrls[i].disable();
    }

  }

  public allDetailsCanEdit(): boolean {
    return this.advanceReceipts.every(x => x.assignmentFlag == 0);
  }

  public clear() {

    this.MyFormGroup.reset();
    this.numberOfLineCtrl.setValue(1);
    this.outputAtCtrl.disable();
    this.transferOutputAtCtrl.disable();
    this.cbxTransferOutputAtCtrl.disable();

  }

  public openMasterModal(table: TABLE_INDEX, index: number = -1) {
    if (index >= 0 &&
       (this.advanceReceipts.length <= index ||
        this.advanceReceipts[index].assignmentFlag > 0)) return;

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
  
        componentRef.instance.TableIndex = table;
  
        componentRef.instance.Closing.subscribe(() => {
  
          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
            switch (table) {
              case TABLE_INDEX.MASTER_CUSTOMER:
                {
                  if (index < 0) {
                    this.customerCodeCtrl.setValue(componentRef.instance.SelectedCode);
                    this.customerName = componentRef.instance.SelectedName;
                    this.customerId   = componentRef.instance.selectedId;
                  }
                  else {
                    this.detailCustomerCodeCtrls[index].setValue(componentRef.instance.SelectedCode);
                    this.detailCustomerIdCtrls[index].setValue(componentRef.instance.SelectedId);
                    this.detailCustomerNameCtrls[index].setValue(componentRef.instance.SelectedName);
                  }
                  break;
                }
            }
          }
  
          componentRef.destroy();
        });
  
  }

  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processCustomResult, action);
    if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {

      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;
      case BUTTON_ACTION.DELETE:
        this.delete();
        break;
      case BUTTON_ACTION.REDISPLAY:
        this.reDisplay();
        break;
      case BUTTON_ACTION.CANCEL:
        this.cancelSplit();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }

  }


  ///////////////////////////////////////////////////////////
  public registry() {

    if (!this.validateForRegistry()) return;

    let source = new AdvanceReceivedSplitSource();
    source.companyId = this.userInfoService.ApplicationControl.companyId;
    source.loginUserId = this.userInfoService.LoginUser.id;
    source.currencyId = this.originalReceipt.currencyId;
    source.originalReceiptId = this.originalReceipt.id;
    source.items = this.getSplitReceipts();

    let advanceReceivedSplitResponse = this.receiptService.AdvanceReceivedDataSplit(source);

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    forkJoin(advanceReceivedSplitResponse)
      .subscribe(response=>{
        if (response[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.SAVE_SUCCESS, this.partsResultMessageComponent);
          this.processResultService.createdLog(this.processCustomResult.logData);
        }
        else{
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();
      });

  }

  public validateForRegistry(): boolean {

    if (this.cbxCancelAdvanceReceivedCtrls.length != 0 && NumberUtil.ParseInt(this.splitRemainingCtrl.value) != 0 ) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.ADVANCE_RECEIVED_AMOUNT_UNMATCH_BEFORE_AND_AFTER,
        this.partsResultMessageComponent);
      return false;
    }

    if (this.advanceReceivedBackup == undefined) {
      if (this.cbxCancelAdvanceReceivedCtrls.length == 1 && this.detailCustomerCodeCtrls[0].value == this.advanceReceipts[0].customerCode) {
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.NO_DATA.replace(MSG_ITEM_NUM.FIRST, '登録できる'),
            this.partsResultMessageComponent);
        return false;
      }
    }

    if (this.cbxCancelAdvanceReceivedCtrls.length == 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_GRID_ONE_REQUIRED, this.partsResultMessageComponent);
      return false;
    }

    if (this.advanceReceiptState == ADVANCE_RECEIPT_STATE.STATE5 || this.advanceReceiptState == ADVANCE_RECEIPT_STATE.STATE6) {

      if (this.transferDateCtrl.value == undefined) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '振替処理年月日'),
          this.partsResultMessageComponent);

        HtmlUtil.nextFocusByName(this.elementRef, 'transferDateCtrl', EVENT_TYPE.NONE);
        return false;
      }

    }

    //  明細各行チェック
    for (let i = 0; i < this.cbxCancelAdvanceReceivedCtrls.length; i++) {

      if (StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[i].value)) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '得意先コード'),
          this.partsResultMessageComponent);

        HtmlUtil.nextFocusByName(this.elementRef, 'detailCustomerCodeCtrl' + i, EVENT_TYPE.NONE);
        return false;
      }

      if (this.detailReceiptAmountCtrls[i].value == undefined || NumberUtil.ParseInt(this.detailReceiptAmountCtrls[i].value) == 0) {
        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '入金額'),
          this.partsResultMessageComponent);

        HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptAmountCtrl' + i, EVENT_TYPE.NONE);
        return false;
      }

      if (this.processingAtCtrl.value != undefined) {
        if (!DateUtil.isValidRange(this.processingAtCtrl, this.transferDateCtrl)) {
          let msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '処理予定日')
                                                        .replace(MSG_ITEM_NUM.SECOND, '振替処理年月日');
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, msg, this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'processingAtCtrl', EVENT_TYPE.NONE);
          return false;
        }

        let tmpProcessingAt = new NgbDate(this.processingAtCtrl.value.year, this.processingAtCtrl.value.month, this.processingAtCtrl.value.day);
        let dRecordedAt = new Date(DateUtil.getYYYYMMDD(1, this.originalReceipt.recordedAt));
        let tmpRecordedAt = new NgbDate(dRecordedAt.getFullYear(), dRecordedAt.getMonth() + 1,dRecordedAt.getDay());

        if (tmpProcessingAt.before(tmpRecordedAt)) {
          let msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '処理予定日')
                                                        .replace(MSG_ITEM_NUM.SECOND, '入金日');
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, msg, this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'processingAtCtrl', EVENT_TYPE.NONE);
          return false;
        }
      }

    }

    return true;
  }

  public getSplitReceipts(): AdvanceReceivedSplit[] {

    let advanceReceivedSplits = new Array<AdvanceReceivedSplit>();

    for (let i = 0; i < this.cbxCancelAdvanceReceivedCtrls.length; i++) {
      let item = new AdvanceReceivedSplit();
      item.processingAt = this.detailProcessingAtCtrls[i].value;
      item.customerId = this.detailCustomerIdCtrls[i].value;
      item.staffId = null;
      item.receiptAmount = this.detailReceiptAmountCtrls[i].value;
      item.memo = this.detailMemoCtrls[i].value;
      item.note1 = this.detailNote1Ctrls[i].value;
      item.note2 = this.detailNote2Ctrls[i].value;
      item.note3 = this.detailNote3Ctrls[i].value;
      item.note4 = this.detailNote4Ctrls[i].value;

      advanceReceivedSplits.push(item);

    }

    return advanceReceivedSplits;

  }


  ///////////////////////////////////////////////////////////
  public reDisplay() {
    this.loadData();
    this.rowDeleteDisableFlag = true;
  }

  ///////////////////////////////////////////////////////////
  public cancelSplit() {

    let source = new AdvanceReceivedSplitSource();
    source.companyId = this.userInfoService.ApplicationControl.companyId;
    source.loginUserId = this.userInfoService.LoginUser.id;
    source.currencyId = this.originalReceipt.currencyId;
    source.originalReceiptId = this.originalReceipt.id;

    let cancelAdvanceSplitResponse = this.receiptService.CancelAdvanceReceivedDataSplit(source);

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    forkJoin(cancelAdvanceSplitResponse)
      .subscribe(response=>{
        if (response[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.processResultService.processAtSuccess(
            this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
          this.processResultService.createdLog(this.processCustomResult.logData);
        }
        else {
          this.processResultService.processAtFailure(
            this.processCustomResult,MSG_ERR.SOMETHING_ERROR.replace("{0}","振替取消処理"),
            this.partsResultMessageComponent);
        }
        modalRouterProgressComponentRef.destroy();
      });

  }

  ///////////////////////////////////////////////////////////
  public selectAll() {

    for (let index = 0; index < this.cbxCancelAdvanceReceivedCtrls.length; index++) {
      if (this.advanceReceipts[index].assignmentFlag == 0) {
        this.cbxCancelAdvanceReceivedCtrls[index].setValue(true);
      }
    }
    if (this.cbxCancelAdvanceReceivedCtrls.filter(x => x.value).length > 0) {
      this.rowDeleteDisableFlag = false;
    }
  }

  ///////////////////////////////////////////////////////////
  public clearAll() {
    for (let index = 0; index < this.cbxCancelAdvanceReceivedCtrls.length; index++) {
      this.cbxCancelAdvanceReceivedCtrls[index].setValue(false);
    }
    this.rowDeleteDisableFlag = true;
  }

  public back() {
    this.router.navigate(['main/PD0601', { "process": "back" }]);
  }

  ///////////////////////////////////////////////////////////
  public setProcessingAt(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCustomerCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value)) {
      this.customerId   = undefined;
      this.customerName = undefined;
    }
    else {
      let code = this.customerCodeCtrl.value as string;
      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        code = StringUtil.setPaddingFrontZero(code, this.userInfoService.ApplicationControl.customerCodeLength);
        this.customerCodeCtrl.setValue(code);
      }
      this.loadStart();
      this.customerService.GetItems(code)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.customerCodeCtrl.setValue(response[0].code);
            this.customerName = response[0].name;
            this.customerId   = response[0].id;
          }
          else {
            let msg = MSG_WNG.MASTER_NOT_EXIST.split(MSG_ITEM_NUM.FIRST).join("得意先");
            this.errorMsg = msg.replace(MSG_ITEM_NUM.SECOND, code);
            this.customerCodeCtrl.setValue('');
            this.customerName = undefined;
            this.customerId   = undefined;
          }
        });

    }

    HtmlUtil.nextFocusByName(this.elementRef, 'receiptAmountCtrl', eventType);

  }

  ///////////////////////////////////////////////////////////
  public setReceiptAmount(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'numberOfLineCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public ignoreTransform(control: FormControl) {
    return StringUtil.IsNullOrEmpty(control.value) || control.value === '0';
  }

  public onFocusCurrencyControl(control: FormControl) {
    if (this.ignoreTransform(control)) {
      return;
    }
    const pipe = new RacCurrencyPipe();
    control.setValue(pipe.reverceTransform(control.value, false));
  }

  public onLeaveCurrencyControl(control: FormControl) {
    if (this.ignoreTransform(control)) {
      return;
    }
    const pipe = new RacCurrencyPipe();
    let value = NumberUtil.ParseInt(control.value);
    control.setValue(pipe.transform(value, false));
  }

  public onFocusReceiptAmount() {
    this.onFocusCurrencyControl(this.receiptAmountCtrl);
  }

  public onLeaveReceiptAmount() {
    this.onLeaveCurrencyControl(this.receiptAmountCtrl);
  }

  ///////////////////////////////////////////////////////////
  public addLine() {

    if (this.numberOfLineCtrl.value == undefined) return;

    const addLines = NumberUtil.ParseInt(this.numberOfLineCtrl.value);
    if (addLines < 1) return;

    if (this.advanceReceipts.length + addLines > MaxRowCount) {
      this.processCustomResult = this.processResultService.processAtWarning(this.processCustomResult,
        MSG_WNG.ADVANCED_RECEIVE_SPLIT_ROW_COUNT_ERROR.replace(MSG_ITEM_NUM.FIRST, MaxRowCount.toString()),
        this.partsResultMessageComponent);
      return;
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeCtrl.value) && !StringUtil.IsNullOrEmpty(this.errorMsg)) {

      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, this.errorMsg, this.partsResultMessageComponent);
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeCtrl', EVENT_TYPE.NONE);
      return;
    }

    for(let i = 0; i < addLines; i++) {
      let receipt = new Receipt();
      receipt.assignmentFlag = 0;
      receipt.processingAt = this.processingAtCtrl.value != undefined ? this.processingAtCtrl.value : null;

      if (this.processingAtCtrl.value != undefined) {
        receipt.processingAt = DateUtil.ConvertFromDatepicker(this.processingAtCtrl)
      }

      receipt.customerCode  = this.customerCodeCtrl.value != undefined ? this.customerCodeCtrl.value : null;
      receipt.customerName  = StringUtil.IsNullOrEmpty(this.customerName) ? "" : this.customerName ;
      receipt.customerId    = this.customerId != 0 ? this.customerId : null;
      receipt.receiptAmount = this.receiptAmountCtrl.value != undefined ? this.receiptAmountCtrl.value : null;
      receipt.receiptMemo   = "";
      receipt.note1         = "";
      receipt.note2         = "";
      receipt.note3         = "";
      receipt.note4         = "";

      this.advanceReceipts.push(receipt);
    }

    this.setControlValuesToModels();

    this.initializeDetailsControl();
    this.setSum();
  }
  public delete() {
    const maxCount = this.cbxCancelAdvanceReceivedCtrls.length;
    const maxIndex = maxCount - 1;
    let count = 0;
    for (let i = maxIndex; i >= 0; i--) {
      if (this.cbxCancelAdvanceReceivedCtrls[i].value) {
        this.advanceReceipts.splice(i, 1);
        count++;
      }
    }

    if (this.advanceReceipts.length == 0) {
      this.rowDeleteDisableFlag = true;
    }
    else {
      this.rowDeleteDisableFlag = true;
    }

    this.setControlValuesToModels();
    
    // this.removeGridControls(maxCount, maxIndex);
    this.initializeDetailsControl();
    this.setSum();

  }

  public removeGridControls(count: number, maxIndex: number) {
    let index = maxIndex;
    for (let i = 0; i < count; i++) {
      this.MyFormGroup.removeControl("cbxCancelAdvanceReceivedCtrl" + index);
      this.MyFormGroup.removeControl("detailAssignmentFlagCtrl" + index);
      this.MyFormGroup.removeControl("detailProcessingAtCtrl" + index);
      this.MyFormGroup.removeControl("detailCustomerCodeCtrl" + index);
      this.MyFormGroup.removeControl("detailCustomerNameCtrl" + index);
      this.MyFormGroup.removeControl("detailCustomerIdCtrl" + index);
      this.MyFormGroup.removeControl("detailReceiptAmountCtrl" + index);
      this.MyFormGroup.removeControl("detailMemoCtrl" + index);
      this.MyFormGroup.removeControl("detailNote1Ctrl" + index);
      this.MyFormGroup.removeControl("detailNote2Ctrl" + index);
      this.MyFormGroup.removeControl("detailNote3Ctrl" + index);
      this.MyFormGroup.removeControl("detailNote4Ctrl" + index);
      index--;
    }
  }

  ///////////////////////////////////////////////////////////
  public setOutputAt(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'transferDateCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setTransferDate(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'transferOutputAtCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setTransferOutputAt(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'transferOutputAtCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public getColumnAlias(columnName: string): string {
    if (this.columnNameSettingsResult != null) {
      let tmp = ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, columnName);
      return tmp;
    }
    return "";
  }

  ///////////////////////////////////////////////////////////
  public setCbxCancelAdvanceReceived(index:number) {

    this.rowDeleteDisableFlag = true;
    for (let index = 0; index < this.cbxCancelAdvanceReceivedCtrls.length; index++) {
      if (this.cbxCancelAdvanceReceivedCtrls[index].value) {
        this.rowDeleteDisableFlag = false;
        break;
      }
    }
  }

  ///////////////////////////////////////////////////////////
  public getOriginalCategoryCodeName(): string {

    if (this.originalReceipt == undefined) return "";
    return this.originalReceipt.categoryCode + ":" + this.originalReceipt.categoryName;
  }

  ///////////////////////////////////////////////////////////
  public getOriginalRemainAmount(): number {

    if (this.originalReceipt != undefined) {
      return this.originalReceipt.receiptAmount - this.originalReceipt.assignmentAmount;
    }
  }

  ///////////////////////////////////////////////////////////
  public setDetailProcessingAt(eventType: any, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailCustomerCodeCtrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public setDetailCustomerCode(eventType: string, index: number) {
    if (!StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.detailCustomerCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailCustomerCodeCtrls[index].value, this.userInfoService.ApplicationControl.customerCodeLength));
      }

      const code = this.detailCustomerCodeCtrls[index].value;
      this.customerService.GetItems(code)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.detailCustomerCodeCtrls[index].setValue(response[0].code);
            this.detailCustomerIdCtrls[index].setValue(response[0].id);
            this.detailCustomerNameCtrls[index].setValue(response[0].name);
          }
          else {
            let msg = MSG_WNG.MASTER_NOT_EXIST.split(MSG_ITEM_NUM.FIRST).join("得意先");
            msg = msg.replace(MSG_ITEM_NUM.SECOND, code);
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, msg, this.partsResultMessageComponent);
            this.detailCustomerCodeCtrls[index].setValue("");
            this.detailCustomerIdCtrls[index].setValue("");
            this.detailCustomerNameCtrls[index].setValue("");
          }
        });
        HtmlUtil.nextFocusByName(this.elementRef, 'detailReceiptAmountCtrl' + index, eventType);
    }
    else {
      this.detailCustomerCodeCtrls[index].setValue("");
      this.detailCustomerIdCtrls[index].setValue("");
      this.detailCustomerNameCtrls[index].setValue("");
    }

  }

  ///////////////////////////////////////////////////////////
  public setDetailMemo(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote1Ctrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public setDetailNote1(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote2Ctrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public setDetailNote2(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote3Ctrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public setDetailNote3(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote4Ctrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public setDetailNote4(eventType: string, index: number) {
    HtmlUtil.nextFocusByName(this.elementRef, 'detailNote4Ctrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public onFocusDetailReceiptAmount(index: number) {
    this.onFocusCurrencyControl(this.detailReceiptAmountCtrls[index]);
  }

  public onLeaveDetailReceiptAmount(index: number) {
    this.onLeaveCurrencyControl(this.detailReceiptAmountCtrls[index]);
    this.setSum();
  }

  public setDetailReceiptAmount(eventType: string, index: number) {
    this.setSum();
    HtmlUtil.nextFocusByName(this.elementRef, 'detailMemoCtrl' + index, eventType);
  }

  ///////////////////////////////////////////////////////////
  public setSum() {

    let total: number = 0;
    for (let i = 0; i < this.cbxCancelAdvanceReceivedCtrls.length; i++) {
      total += NumberUtil.ParseInt(this.detailReceiptAmountCtrls[i].value);
    }

    this.transferAmount = total;
    this.remainingAmount = (this.originalReceipt.receiptAmount - this.originalReceipt.assignmentAmount) - total;

  }

  public getAssignmentValue(receipt: Receipt): string {
    const state
     = receipt.assignmentFlag == 0 ? 'no' :
       receipt.assignmentFlag == 1 ? 'part' :
       receipt.assignmentFlag == 2 ? 'full' : '';
    return `<span class="tag--${state}Assignment">${MATCHING_ASSIGNMENT_FLAG_DICTIONARY[receipt.assignmentFlag].val}</span>`;
  }

}
