
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { KanaHistoryCustomersResult } from 'src/app/model/kana-history-customers-result.model';
import { KanaHistoryCustomerMasterService } from 'src/app/service/Master/kana-history-customer-master.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { KanaHistoryPaymentAgencyResult } from 'src/app/model/kana-history-payment-agency-result.model';
import { KanaHistorySearch } from 'src/app/model/kana-history-search.model';
import { KanaHistoryPaymentAgencyMasterService } from 'src/app/service/Master/kana-history-payment-agency-master.service';
import { KanaHistoryCustomer } from 'src/app/model/kana-history-customer.model';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { KanaHistoryPaymentAgency } from 'src/app/model/kana-history-payment-agency.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { SortItem, SortOrder } from 'src/app/model/collation/sort-item';
import { SortUtil } from 'src/app/common/util/sort-util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb1001-kana-history-master',
  templateUrl: './pb1001-kana-history-master.component.html',
  styleUrls: ['./pb1001-kana-history-master.component.css']
})
export class Pb1001KanaHistoryMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public kanaHistoryCustomersResult: KanaHistoryCustomersResult;
  public kanaHistoryPaymentAgencyResult: KanaHistoryPaymentAgencyResult;
  public juridicalPersonalityResult: JuridicalPersonalitysResult;

  public rdoSearchObjectCtrl: FormControl;  // 検索対象

  public payerNameCtrl: FormControl;  // 振込依頼人名

  public customerCodeFromCtrl: FormControl; // 得意先
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;
  public cbxCustomerCtrl: FormControl;

  public rdoOrderCtrl  // 並び順

  public cbxDetailDelFlagCtrls: Array<FormControl>;

  public isCustomerSearchObject: boolean = true;
  public headers: Array<any>;


  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public kanaHistoryCustomerService: KanaHistoryCustomerMasterService,
    public kanaHistoryPaymentService: KanaHistoryPaymentAgencyMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public processResultService: ProcessResultService,
    public customerService: CustomerMasterService,
    public localStorageManageService: LocalStorageManageService

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title, true);
      }
    });
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete()

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }

    this.juridicalPersonalityService.GetItems()
      .subscribe(response => {
        if (response != undefined) {
          this.juridicalPersonalityResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalityResult.juridicalPersonalities = response;
        }
      });
  }

  public setControlInit() {
    this.rdoSearchObjectCtrl = new FormControl(''); // 検索対象

    this.payerNameCtrl = new FormControl(''); // 振込依頼人名

    this.customerCodeFromCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]); // 得意先
    this.customerNameFromCtrl = new FormControl('');
    this.customerCodeToCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl('');
    this.cbxCustomerCtrl = new FormControl('');

    this.rdoOrderCtrl = new FormControl(''); // 並び順
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      rdoSearchObjectCtrl: this.rdoSearchObjectCtrl,  // 検索対象

      payerNameCtrl: this.payerNameCtrl,  // 振込依頼人名

      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,

      rdoOrderCtrl: this.rdoOrderCtrl,  // 並び順
    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }
  }

  public clear() {
    this.MyFormGroup.reset();
    // 初期値を「得意先」「振込依頼人名」に設定
    this.rdoSearchObjectCtrl.setValue('1');
    this.rdoOrderCtrl.setValue('1');

    this.kanaHistoryCustomersResult = null;
    this.kanaHistoryPaymentAgencyResult = null;
    this.isCustomerSearchObject = true;

    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.setRangeCheckbox();

    this.panelOpenState = true;
    this.panel.open();

  }


  public setAutoComplete() {
    // 得意先
    this.initAutocompleteCustomers(this.customerCodeFromCtrl, this.customerService, 0);
    this.initAutocompleteCustomers(this.customerCodeToCtrl, this.customerService, 1);

    // 得意先・決済代行会社に応じて、再設定が必要。
  }

  /**
   * 各テーブルのデータを取得する
   * @param table テーブル名
   * @param keyCode イベント種別
   * @param type データタイプ
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {
    this.customerCodeFromTrigger.closePanel();
    this.customerCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_CUSTOMER:
            {
              if (type === "from") {
                this.customerCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxCustomerCtrl.value == true) {
                  this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
                }
              }
              else if (type === "to") {
                this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }
          case TABLE_INDEX.MASTER_PAYMENT_AGENCY:
            {
              if (type === "from") {
                this.customerCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxCustomerCtrl.value == true) {
                  this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
                }
              }
              else if (type === "to") {
                this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              break;
            }

        }

      }

      componentRef.destroy();
    });
  }

  /**
   * 対象のテーブル切り替え
   */
  public selectSearchObject() {
    if (this.rdoSearchObjectCtrl.value == '1') {
      this.clear();
      this.isCustomerSearchObject = true;
      this.rdoSearchObjectCtrl.setValue('1');

      this.initAutocompleteCustomers(this.customerCodeFromCtrl, this.customerService, 0);
      this.initAutocompleteCustomers(this.customerCodeToCtrl, this.customerService, 1);

    }
    else {
      this.clear();
      this.isCustomerSearchObject = false;
      this.rdoSearchObjectCtrl.setValue('2');

      this.initAutocompleteCustomers(this.customerCodeFromCtrl, this.customerService, 0);
      this.initAutocompleteCustomers(this.customerCodeToCtrl, this.customerService, 1);

    }
  }


  /**
   * 対象のテーブル切り替え
   */
  public selectOrder() {

    let sortItems = new Array<SortItem>();

    if (this.isCustomerSearchObject) {
      if (this.rdoOrderCtrl.value == "1") {
        sortItems.push(new SortItem("payerName", SortOrder.Ascending));
        sortItems.push(new SortItem("customerCode", SortOrder.Ascending));
      }
      else {
        sortItems.push(new SortItem("customerCode", SortOrder.Ascending));
        sortItems.push(new SortItem("payerName", SortOrder.Ascending));
      }
      SortUtil.Sort(sortItems, this.kanaHistoryCustomersResult.kanaHistoryCustomers);
    }
    else {
      if (this.rdoOrderCtrl.value == "1") {
        sortItems.push(new SortItem("payerName", SortOrder.Ascending));
        sortItems.push(new SortItem("paymentAgencyCode", SortOrder.Ascending));
      }
      else {
        sortItems.push(new SortItem("paymentAgencyCode", SortOrder.Ascending));
        sortItems.push(new SortItem("payerName", SortOrder.Ascending));
      }
      SortUtil.Sort(sortItems, this.kanaHistoryPaymentAgencyResult.kanaHistoryPaymentAgency);

    }



  }

  /**
   * データ検索・取得
   */
  public search(messageFlag: boolean = true) {
    let kanaHistorySearch = new KanaHistorySearch();
    this.kanaHistoryCustomersResult = null;
    this.kanaHistoryPaymentAgencyResult = null;

    // 振込依頼人名
    if (!this.StringUtil.IsNullOrEmpty(this.payerNameCtrl.value)) {
      kanaHistorySearch.payerName = this.payerNameCtrl.value;
    }

    // 得意先コード(決済代行会社コード)
    if (!this.StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {
      kanaHistorySearch.codeFrom = this.customerCodeFromCtrl.value;
    }
    if (this.cbxCustomerCtrl.value == true
      && !this.StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
      kanaHistorySearch.codeTo = this.customerCodeToCtrl.value;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();      

    if (this.isCustomerSearchObject) {
      // 得意先検索
      this.kanaHistoryCustomersResult = new KanaHistoryCustomersResult();

      this.kanaHistoryCustomerService.GetItems(kanaHistorySearch)
        .subscribe(response => {
          this.processCustomResult = this.processResultService.processAtGetData(
            this.processCustomResult, response, messageFlag, this.partsResultMessageComponent);
          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
            this.kanaHistoryCustomersResult.kanaHistoryCustomers = response;

            this.selectOrder();

            this.cbxDetailDelFlagCtrls = new Array<FormControl>(
              this.kanaHistoryCustomersResult.kanaHistoryCustomers.length);

            for (let index = 0; index < this.kanaHistoryCustomersResult.kanaHistoryCustomers.length; index++) {
              this.cbxDetailDelFlagCtrls[index] = new FormControl("");
              this.cbxDetailDelFlagCtrls[index].setValue(false);
              this.MyFormGroup.removeControl("cbxDetailDelFlagCtrl" + index);
              this.MyFormGroup.addControl("cbxDetailDelFlagCtrl" + index, this.cbxDetailDelFlagCtrls[index]);
            }
          }
          this.panelOpenState = false;
          this.panel.close();
          processComponentRef.destroy();      
        });
    } else {
      // 決済代行会社検索
      this.kanaHistoryPaymentAgencyResult = new KanaHistoryPaymentAgencyResult();

      this.kanaHistoryPaymentService.GetItems(kanaHistorySearch)
        .subscribe(response => {
          this.processCustomResult = this.processResultService.processAtGetData(
            this.processCustomResult, response, true, this.partsResultMessageComponent);
          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
            this.kanaHistoryPaymentAgencyResult.kanaHistoryPaymentAgency = response;

            this.selectOrder();

            this.cbxDetailDelFlagCtrls = new Array<FormControl>(
              this.kanaHistoryPaymentAgencyResult.kanaHistoryPaymentAgency.length);


            for (let index = 0; index < this.kanaHistoryPaymentAgencyResult.kanaHistoryPaymentAgency.length; index++) {
              this.cbxDetailDelFlagCtrls[index] = new FormControl("");
              this.cbxDetailDelFlagCtrls[index].setValue(false);
              this.MyFormGroup.removeControl("cbxDetailDelFlagCtrl" + index);
              this.MyFormGroup.addControl("cbxDetailDelFlagCtrl" + index, this.cbxDetailDelFlagCtrls[index]);
            }
          }
          this.panelOpenState = false;
          this.panel.close();
          processComponentRef.destroy();      
        });
    }

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
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

      case BUTTON_ACTION.IMPORT:
        this.openImportMethodSelector();
        break;

      case BUTTON_ACTION.EXPORT:
        this.export();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * データの削除
   */
  public delete() {
    let deleteItems: Array<any>;
    let dataResult = new Array<any>();

    if (this.isCustomerSearchObject) {
      dataResult = this.kanaHistoryCustomersResult.kanaHistoryCustomers;
      deleteItems = new Array<KanaHistoryCustomer>();
    } else {
      dataResult = this.kanaHistoryPaymentAgencyResult.kanaHistoryPaymentAgency;
      deleteItems = new Array<KanaHistoryPaymentAgency>();
    }

    for (let i = 0; i < this.cbxDetailDelFlagCtrls.length; i++) {
      if (this.cbxDetailDelFlagCtrls[i].value == true) {
        deleteItems.push(dataResult[i]);
      }
    }

    if (deleteItems.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_DELETE_DATA, this.partsResultMessageComponent);
      return;
    }

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        if (this.isCustomerSearchObject) {
          this.callHistoryCustomerDeleteAPI(deleteItems, 0, deleteItems.length - 1);
        } else {
          this.callHistoryPaymentAgencyDeleteAPI(deleteItems, 0, deleteItems.length - 1);
        }
      }
      componentRef.destroy();
    });
  }

  /**
   * 削除用のAPIを呼び出しデータを削除する(得意先)（再帰処理）
   * @param deleteItems 削除対象のデータ配列
   * @param nowIndex 削除を実行するデータの要素番号
   * @param lastIndex 最大要素番号
   */
  public callHistoryCustomerDeleteAPI(deleteItems: Array<KanaHistoryCustomer>, nowIndex: number, lastIndex: number) {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    this.kanaHistoryCustomerService.Delete(deleteItems[nowIndex])
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtDelete(
          this.processCustomResult, result, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          if (nowIndex == lastIndex) {
            this.search(false);
          } else {
            this.callHistoryCustomerDeleteAPI(deleteItems, nowIndex + 1, lastIndex)
          }
        } else {
          return;
        }
        processComponentRef.destroy();
      });
  }

  /**
   * 削除用のAPIを呼び出しデータを削除する(決済代行会社)（再帰処理）
   * @param deleteItems 削除対象のデータ配列
   * @param nowIndex 削除を実行するデータの要素番号
   * @param lastIndex 最大要素番号
   */
  public callHistoryPaymentAgencyDeleteAPI(deleteItems: Array<KanaHistoryPaymentAgency>, nowIndex: number, lastIndex: number) {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    this.kanaHistoryPaymentService.Delete(deleteItems[nowIndex])
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtDelete(
          this.processCustomResult, result, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          if (nowIndex == lastIndex) {
            this.search(false);
          } else {
            this.callHistoryPaymentAgencyDeleteAPI(deleteItems, nowIndex + 1, lastIndex)
          }
        } else {
          return;
        }
        processComponentRef.destroy();
      });
  }

  /**
   * チェックした項目を判定
   * @param index 行番号
   */
  public onChecked(index: number) {
    this.cbxDetailDelFlagCtrls[index].setValue(!this.cbxDetailDelFlagCtrls[index].value);

    for (let i = 0; i < this.cbxDetailDelFlagCtrls.length; i++) {
      if (this.cbxDetailDelFlagCtrls[i].value == true) {
        this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
        return;
      }
    }
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    if (this.isCustomerSearchObject) {
      componentRef.instance.TableIndex = TABLE_INDEX.MASTER_KANA_HISTORY_CUSTOMER;
    } else {
      componentRef.instance.TableIndex = TABLE_INDEX.MASTER_KANA_HISTORY_PAYMENT_AGENCY;
    }
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.search(false);
    });
    this.openOptions();

  }

  /**
   * エクスポート
   */
  public export() {
    let dataList = new Array<any>();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();      

    if (this.isCustomerSearchObject) {
      this.headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.KANA_HISTORY_CUSTOMER_MASTER);
      if (this.kanaHistoryCustomersResult == null) {
        this.kanaHistoryCustomerService.GetItems(new KanaHistorySearch())
          .subscribe(result => {
            if (result) {
              this.createCsv(result);
            }
            processComponentRef.destroy();
          });
      } else {
        dataList = this.kanaHistoryCustomersResult.kanaHistoryCustomers;
        this.createCsv(dataList);
        processComponentRef.destroy();

      }

    } else {
      this.headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.KANA_HISTORY_PAYMENT_AGENCY_MASTER);
      if (this.kanaHistoryPaymentAgencyResult == null) {
        this.kanaHistoryPaymentService.GetItems(new KanaHistorySearch())
          .subscribe(result => {
            if (result) {
              this.createCsv(result);
            }
            processComponentRef.destroy();
          });
      } else {
        dataList = this.kanaHistoryPaymentAgencyResult.kanaHistoryPaymentAgency;
        this.createCsv(dataList);
        processComponentRef.destroy();
      }
    }
    this.openOptions();
  }

  /**
   * CSV作成
   * @param dataList 出力するデータ
   */
  public createCsv(dataList: Array<any>) {
    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let data: string = this.headers.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].payerName);

      if (this.isCustomerSearchObject) {
        dataItem.push(dataList[index].customerCode);
      } else {
        dataItem.push(dataList[index].paymentAgencyCode);
      }

      dataItem.push(dataList[index].sourceBankName);
      dataItem.push(dataList[index].sourceBranchName);
      dataItem.push(dataList[index].hitCount);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }
    let isTryResult: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    try {
      FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      isTryResult = true;

    } catch (error) {
      console.error(error);
    }
    this.processResultService.processAtOutput(
      this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setPayerName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
  }

  public inputPayerName() {
    let value = this.payerNameCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalityResult.juridicalPersonalities)
    this.payerNameCtrl.setValue(value);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setCustomerCodeFrom(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.customerCodeFromTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)) {

      this.loadStart();
      this.customerService.GetItems(this.customerCodeFromCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeFromCtrl.setValue(response[0].code);
            this.customerNameFromCtrl.setValue(response[0].name);
            if (this.cbxCustomerCtrl.value == true) {
              this.customerCodeToCtrl.setValue(response[0].code);
              this.customerNameToCtrl.setValue(response[0].name);
            }
            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
          }
          else {
            //this.customerCodeFromCtrl.setValue("");
            this.customerNameFromCtrl.setValue("");

            if (this.cbxCustomerCtrl.value == true) {
              this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
              this.customerNameToCtrl.setValue("");
            }


            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
          }
        });
    }
    else {
      this.customerCodeFromCtrl.setValue("");
      this.customerNameFromCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
    }

  }

  public setCustomerCodeTo(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.customerCodeToTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {

      this.loadStart();
      this.customerService.GetItems(this.customerCodeToCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.customerCodeToCtrl.setValue(response[0].code);
            this.customerNameToCtrl.setValue(response[0].name);
          }
          else {
            // this.customerCodeToCtrl.setValue("");
            this.customerNameToCtrl.setValue("");
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'payerNameCtrl', eventType);
        });
    }
    else {
      this.customerCodeToCtrl.setValue("");
      this.customerNameToCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'payerNameCtrl', eventType);
    }

  }

  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PB1001_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }


  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PB1001_CUSTOMER);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }
  }

}
