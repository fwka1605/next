import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormatterUtil, } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { DateUtil } from 'src/app/common/util/date-util';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MfAggrTransaction } from 'src/app/model/mf-aggr-transaction.model'
import { MfAggrTransactionSearch } from 'src/app/model/mf-aggr-transaction-search.model'
import { MfAggrTransactionService } from 'src/app/service/mf-aggr-transaction.service';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';

@Component({
  selector: 'app-ph2001-mf-aggr-history-search',
  templateUrl: './ph2001-mf-aggr-history-search.component.html',
  styleUrls: ['./ph2001-mf-aggr-history-search.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})

export class Ph2001MfAggrHistorySearchComponent extends BaseComponent implements OnInit, AfterViewInit {

  // 遷移元のコンポーネント
  public paramFrom: ComponentId;

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  // 検索パネルの開閉フラグ
  public panelOpenState: boolean;

  public mfAggrTransactionSearch: MfAggrTransactionSearch;
  public mfAggrTransactions: MfAggrTransaction[];

  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public recordedAtFromCtrl: FormControl;   // 伝票日付
  public recordedAtToCtrl: FormControl;
  public accountNameCtrl: FormControl;       // 金融機関名
  public subAccountNameCtrl: FormControl;    // 支店名
  public accountTypeNameCtrl: FormControl;  // 口座種別
  public bankCodeCtrl: FormControl;         // 銀行
  public branchCodeCtrl: FormControl;
  public accountNumberCtrl: FormControl;
  public currencyCodeCtrl: FormControl;     // 通貨コード
  public currencyId: number;

  public undefineCtrl: FormControl; // 未定用

  public dispSumCount: number;  // 総件数
  public dispSumInAmount: number; // 総入金額
  public dispSumOutAmount: number;  // 総出金額

  public cbxDeleteCtrls: Array<FormControl>  // 削除チェック

  public deleteButtonDisable: boolean = true;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public currencyService: CurrencyMasterService,
    public processResultService: ProcessResultService,
    public localStorageManageService: LocalStorageManageService,
    public mfAggrTransactionService: MfAggrTransactionService,
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
      }
    });

  }

  ngOnInit() {

    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();
    this.getParamFrom();

  }

  ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {

    this.cbxCheckAllCtrl = new FormControl(null);  // 全選択・全解除

    this.recordedAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金日
    this.recordedAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.accountNameCtrl = new FormControl("");     // 金融機関名
    this.subAccountNameCtrl = new FormControl("");  // 支店名
    this.accountTypeNameCtrl = new FormControl(""); // 口座種別

    this.bankCodeCtrl = new FormControl("", [Validators.maxLength(4)]); // 銀行
    this.branchCodeCtrl = new FormControl("", [Validators.maxLength(3)]);
    this.accountNumberCtrl = new FormControl("", [Validators.maxLength(7)]);

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3)]);  // 通貨コード

    this.undefineCtrl = new FormControl(""); // 未定用

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      cbxCheckAllCtrl:this.cbxCheckAllCtrl, // 全選択・全解除

      recordedAtFromCtrl: this.recordedAtFromCtrl,    // 入金日
      recordedAtToCtrl: this.recordedAtToCtrl,

      accountNameCtrl: this.accountNameCtrl,          // 金融機関名
      subAccountNameCtrl: this.subAccountNameCtrl,    // 支店名
      accountTypeNameCtrl: this.accountTypeNameCtrl,  // 口座種別

      bankCodeCtrl: this.bankCodeCtrl, // 銀行
      branchCodeCtrl: this.branchCodeCtrl,
      accountNumberCtrl: this.accountNumberCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl,  // 通貨コード

      undefineCtrl: this.undefineCtrl, // 未定用

    });

  }

  public setFormatter() {

    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl); // 通貨コード

    FormatterUtil.setNumberFormatter(this.bankCodeCtrl); // 銀行
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumberCtrl);

  }


  public setAutoComplete() {

  }

  public openMasterModal(table: TABLE_INDEX) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              const currency = componentRef.instance.SelectedObject;
              this.currencyId = currency.id;
              break;
            }
        }
      }

      componentRef.destroy();
    });
  }

  public clear() {

    this.panelOpenState = true;

    this.MyFormGroup.reset();

    this.mfAggrTransactionSearch = null;
    this.mfAggrTransactions = null;

    this.dispSumCount = 0;
    this.dispSumInAmount = 0;
    this.dispSumOutAmount = 0;

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);;

    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', EVENT_TYPE.NONE);

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

      case BUTTON_ACTION.SEARCH:
        this.search();
        break;
      case BUTTON_ACTION.DELETE:
        this.delete();
        break;
      case BUTTON_ACTION.EXPORT:
        this.export();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }

  }

  public search() {
    this.setSearchOption();
    this.searchRequest();
  }

  public setSearchOption() {

    const option = new MfAggrTransactionSearch();
    this.mfAggrTransactionSearch = new MfAggrTransactionSearch();
    option.companyId = this.userInfoService.Company.id;
    option.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.recordedAtFromCtrl);  // 伝票日付
    option.recordedAtTo = DateUtil.ConvertFromDatepicker(this.recordedAtToCtrl);
    option.accountName = this.accountNameCtrl.value;       //  金融機関名
    option.subAccountName = this.subAccountNameCtrl.value;    //  支店名
    option.accountTypeName = this.accountTypeNameCtrl.value;   //  口座種別
    option.bankCode = this.bankCodeCtrl.value;          //  銀行
    option.branchCode = this.branchCodeCtrl.value;
    option.accountNumber = this.accountNumberCtrl.value;

    if (this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
      option.currencyId = this.currencyId;
    }
    this.mfAggrTransactionSearch = option;
  }

  public searchRequest() {

    let confirmComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let confirmComponentRef = this.viewContainerRef.createComponent(confirmComponentFactory);

    this.mfAggrTransactionService.get(this.mfAggrTransactionSearch)
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);

        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

          if (response.length > 0) {
            this.mfAggrTransactions = new Array<MfAggrTransaction>();
            this.mfAggrTransactions = response;

            this.setDeleteControls();
            //this.setSearchResult();
            this.setDispFooterTotalSum();
            this.panelOpenState = false;
          }
          else {
            // 該当データなし
            this.mfAggrTransactions = new Array<MfAggrTransaction>();
            this.mfAggrTransactions = response;
            this.deleteButtonDisable = true;

            this.panelOpenState = true;
          }

        }
        else if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length === 0) {
          this.mfAggrTransactions = null;

          this.panelOpenState = true;

        }

        confirmComponentRef.destroy();
      });
  }

  public setDeleteControls() {

    this.cbxDeleteCtrls = new Array(this.mfAggrTransactions.length);

    for (let i = 0; i < this.cbxDeleteCtrls.length; i++) {
      this.cbxDeleteCtrls[i] = new FormControl(null);
    }

    for (let i = 0; i < this.cbxDeleteCtrls.length; i++) {
      this.MyFormGroup.removeControl("cbxDeleteCtrl" + i);
      this.MyFormGroup.addControl("cbxDeleteCtrl" + i, this.cbxDeleteCtrls[i]);
    }

  }

  public setSearchResult() {
    // 戻る対応のための検索結果を退避

  }

  public delete() {

    let ids = new Array<number>();
    for (let index = 0; index < this.cbxDeleteCtrls.length; index++) {
      if (this.cbxDeleteCtrls[index].value) {
        const item = this.mfAggrTransactions[index];
        ids.push(item.id);
      }
    }

    if (ids.length == 0) return;

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        // 動作中のコンポーネントを開く
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
        modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
          modalRouterProgressComponentRef.destroy();
        });

        this.mfAggrTransactionService.delete(ids)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              this.search();
              modalRouterProgressComponentRef.destroy();
            }
          });
      }

      componentRef.destroy();

    });

  }

  public export() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    //modalRouterProgressComponentRef.destroy();

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    let result: boolean = false;
    let data: string = "";
    let dataList = this.mfAggrTransactions;

    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    FileUtil.download(resultDatas, "入金データ自動連携データ" + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
    result = true;
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);

    modalRouterProgressComponentRef.destroy();

  }

  ///////////////////////////////////////////////////////////
  public setRecordedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtToCtrl', eventType);
  }

  public setRecordedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'payerNameCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);
            this.currencyId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
          }
          else {
            this.currencyCodeCtrl.setValue("");
            this.currencyId = 0;
            HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      this.currencyId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'recordedAtFromCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setBankCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue("");
    }
    else {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setBranchCode(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.branchCodeCtrl.setValue("");
    }
    else {
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true));
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'accountTypeIdCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setAccountNumber(eventType: string) {
    if (StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.accountNumberCtrl.setValue("");
    }
    else {
      this.accountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.accountNumberCtrl.value, 7, true));
    }
    HtmlUtil.nextFocusByNames(this.elementRef, ['privateBankCodeCtrl', 'customerCodeFromCtrl'], eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setDispFooterTotalSum() {
    this.dispSumCount = this.mfAggrTransactions.length;
    this.dispSumInAmount = 0;
    this.dispSumOutAmount = 0;

    this.mfAggrTransactions.forEach(element => {
      this.dispSumInAmount += element.amount > 0 ? element.amount : 0;
      this.dispSumOutAmount += element.amount < 0 ? (element.amount * -1) : 0;
    });

  }

  ///////////////////////////////////////////////////////////////////////
  public getDisplayAmount(item: MfAggrTransaction, isIncome: boolean = true): string {
    let amount: string;

    if (isIncome) {
      amount = item.amount > 0 ? ("" + item.amount) : "";
    }
    else {
      amount = item.amount < 0 ? ("" + (item.amount * -1)) : "";
    }

    return amount
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxDelete(index: number) {
    let bResult = true;
    for (let indec = 0; index < this.cbxDeleteCtrls.length; index++) {
      if (this.cbxDeleteCtrls[index].value) {
        bResult = false;
        break
      }
    }
    this.deleteButtonDisable = bResult;
  }

  ///////////////////////////////////////////////////////////////////////
  public checkAll(){
    this.selectAllOnOff(this.cbxCheckAllCtrl.value);
  }

  public selectAllOnOff(check: boolean) {
    this.deleteButtonDisable = !check;
    for (let index = 0; index < this.cbxDeleteCtrls.length; index++) {
      this.cbxDeleteCtrls[index].setValue(check);
    }
  }

  //////////////////////////////////////////////////////////////
  public getParamFrom() {
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      this.paramFrom = parseInt(params.get('from'));
    });    
  }  

  //////////////////////////////////////////////////////////////
  public disableBack(): boolean {
    // PD1301 入金自動明細連携 データ抽出
    if (this.paramFrom == ComponentId.PD1301) return false;

    return true;
  }  

  ///////////////////////////////////////////////////////////////////////
  public back() {
    this.router.navigate(['main/PD1301']);
  }

}
