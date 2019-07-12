import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { Customer } from 'src/app/model/customer.model';
import { CustomersResult } from 'src/app/model/customers-result.model';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomerGroupMasterService } from 'src/app/service/Master/customer-group-master.service';
import { CustomerGroupsResult } from 'src/app/model/customer-groups-result.model';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { CustomerGroup } from 'src/app/model/customer-group.model';
import { CustomerSearch } from 'src/app/model/customer-search.model';
import { MasterImportData } from 'src/app/model/master-import-data.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';


@Component({
  selector: 'app-pb0601-customer-group-master',
  templateUrl: './pb0601-customer-group-master.component.html',
  styleUrls: ['./pb0601-customer-group-master.component.css']
})
export class Pb0601CustomerGroupMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public beforeCustomersResult: CustomerGroupsResult;
  public afterCustomersResult: CustomersResult;

  public parentCustomerCodeCtrl: FormControl;  // 債権代表者
  public parentCustomerNameCtrl: FormControl;

  public customerCodeFromCtrl: FormControl;  // 得意先
  public customerNameFromCtrl: FormControl;
  public customerCodeToCtrl: FormControl;
  public customerNameToCtrl: FormControl;

  public customers: Array<Customer>;
  public customerCodeFromId: number;
  public customerCodeToId: number;
  public parentCustomerId: number;

  public isAddButton: boolean = true;

  @ViewChild('parentCustomerCodeInput', { read: MatAutocompleteTrigger }) parentCustomerCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeFromInput', { read: MatAutocompleteTrigger }) customerCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('customerCodeToInput', { read: MatAutocompleteTrigger }) customerCodeToTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public customerService: CustomerMasterService,
    public customerGroupService: CustomerGroupMasterService,
    public processResultService: ProcessResultService

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

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }

    this.setAutoComplete();
  }

  public setControlInit() {
    this.parentCustomerCodeCtrl = new FormControl('', [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);   // 債権代表者
    this.parentCustomerNameCtrl = new FormControl('');

    this.customerCodeFromCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);     // 得意先
    this.customerNameFromCtrl = new FormControl('');
    this.customerCodeToCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameToCtrl = new FormControl('');
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      parentCustomerCodeCtrl: this.parentCustomerCodeCtrl,   // 債権代表者
      parentCustomerNameCtrl: this.parentCustomerNameCtrl,

      customerCodeFromCtrl: this.customerCodeFromCtrl,  // 得意先
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.parentCustomerCodeCtrl);
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.parentCustomerCodeCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.parentCustomerCodeCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }
  }

  public clear() {
    this.MyFormGroup.reset();
    this.beforeCustomersResult = null;
    this.afterCustomersResult = null;
    this.customerCodeFromId = null;
    this.customerCodeToId = null;
    this.parentCustomerId = null;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.customerCodeFromCtrl.disable();
    this.customerCodeToCtrl.disable();
    this.isAddButton = true;
    this.getCustomers();
  }


  public setAutoComplete() {
    // 債権代表者コード
    this.initAutocompleteParentCustomers(this.parentCustomerCodeCtrl, this.customerService, 0);

    // 得意先（フリー）    
    this.initAutocompleteFreeCustomers(this.customerCodeFromCtrl, this.customerService, 0);
    this.initAutocompleteFreeCustomers(this.customerCodeToCtrl, this.customerService, 1);
  }

  public customerCodeClear() {
    this.customerCodeFromCtrl.setValue('');
    this.customerNameFromCtrl.setValue('');
    this.customerCodeToCtrl.setValue('');
    this.customerNameToCtrl.setValue('');

    this.customerCodeFromCtrl.enable();
    this.customerCodeToCtrl.enable();
  }

  /**
   * 各テーブルからデータを取得
   * @param table 対象テーブル
   * @param keyCode イベント
   * @param type フォーム種別
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {
    this.isAddButton = true;

    this.parentCustomerCodeTrigger.closePanel();
    this.customerCodeFromTrigger.closePanel();
    this.parentCustomerCodeTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_CUSTOMER_PARENT:
            {
              if (type === "from") {
                this.customerCodeFromId = componentRef.instance.SelectedObject.id
                this.customerCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameFromCtrl.setValue(componentRef.instance.SelectedName);
              }
              else if (type === "to") {
                this.customerCodeToId = componentRef.instance.SelectedObject.id
                this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              this.setAddButton();
              break;
            }
          case TABLE_INDEX.MASTER_PARENT_CUSTOMER:
            {
              this.parentCustomerId = componentRef.instance.SelectedObject.id;
              this.parentCustomerCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.parentCustomerNameCtrl.setValue(componentRef.instance.SelectedName);

              this.customerGroupService.GetItems(this.parentCustomerId)
                .subscribe(response => {

                  this.processResultService.processAtGetData(
                    this.processCustomResult, response, true, this.partsResultMessageComponent);

                  this.beforeCustomersResult = new CustomerGroupsResult();
                  this.beforeCustomersResult.customerGroups = response;
                  this.afterCustomersResult = new CustomersResult();
                  this.afterCustomersResult.customers = new Array<Customer>();

                  for (let i = 0; i < response.length; i++) {
                    let customer = new Customer;
                    customer.id = response[i].childCustomerId;
                    customer.code = response[i].childCustomerCode;
                    customer.name = response[i].childCustomerName;

                    this.afterCustomersResult.customers.push(customer);
                  }

                  this.customerCodeClear();
                  this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

                });
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

      case BUTTON_ACTION.PRINT:
        this.print();
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
   * データ登録・削除
   */
  public registry() {
    if (this.StringUtil.IsNullOrEmpty(this.parentCustomerNameCtrl.value)) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "債権代表者コード");
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    let importData = new MasterImportData<CustomerGroup>();
    let insertItems = new Array<CustomerGroup>();
    let deleteItems = new Array<CustomerGroup>();
    const beforeCustomers = this.beforeCustomersResult.customerGroups;
    const afterCustomers = this.afterCustomersResult.customers;

    // 追加と削除の区別
    for (let i = 0; i < afterCustomers.length; i++) {
      let customerGroup = new CustomerGroup();
      customerGroup.parentCustomerId = this.parentCustomerId;
      customerGroup.updateBy = this.userInfoService.LoginUser.id;
      customerGroup.createBy = this.userInfoService.LoginUser.id;

      // 新しいIDが　beforeにある：何もしない、beforeにない：追加)
      let findIndex = beforeCustomers.findIndex((item) => {
        return (item.childCustomerId === afterCustomers[i].id);
      });
      if (findIndex < 0) {
        customerGroup.childCustomerId = afterCustomers[i].id;
        insertItems.push(customerGroup);
      }
    }

    for (let i = 0; i < beforeCustomers.length; i++) {
      let customerGroup = new CustomerGroup();
      customerGroup.parentCustomerId = this.parentCustomerId;
      customerGroup.updateBy = this.userInfoService.LoginUser.id;
      customerGroup.updateAt = beforeCustomers[i].updateAt;

      // 既存IDが　afterにある：何もしない、afterにない：削除)
      let findIndex = afterCustomers.findIndex((item) => {
        return (item.id === beforeCustomers[i].childCustomerId);
      });
      if (findIndex < 0) {
        customerGroup.childCustomerId = beforeCustomers[i].childCustomerId;
        deleteItems.push(customerGroup);
      }
    }

    if (insertItems.length == 0 && deleteItems.length == 0) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.NO_DATA.replace(MSG_ITEM_NUM.FIRST, "変更する");
      processComponentRef.destroy();
      return;
    }

    // サーバに送信
    importData.insertItems = insertItems;
    importData.deleteItems = deleteItems;
    importData.updateItems = new Array<CustomerGroup>();

    this.customerGroupService.Save(importData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
        }
        processComponentRef.destroy();
      });
  }

  /**
   * 変更後の得意先に追加
   */
  public add() {
    let addCustomerCodeIds = new Array<number>();
    let customerCodeFrom = this.StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value);
    let customerCodeTo = this.StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value);

    if (customerCodeFrom && customerCodeTo) {
      // From/To ともに未入力
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "追加する得意先コード");
      return;
    }

    let findIndex;
    if (!customerCodeFrom) {
      // 重複チェック
      findIndex = this.afterCustomersResult.customers.findIndex((item) => {
        return (item.id === this.customerCodeFromId);
      });

      if (findIndex < 0) {
        addCustomerCodeIds.push(
          this.customers.findIndex((item) => {
            return (item.id === this.customerCodeFromId);
          })
        );
      }
    }

    if (!customerCodeTo) {
      // 重複チェック
      findIndex = this.afterCustomersResult.customers.findIndex((item) => {
        return (item.id === this.customerCodeToId);
      });

      if (findIndex < 0) {
        addCustomerCodeIds.push(
          this.customers.findIndex((item) => {
            return (item.id === this.customerCodeToId);
          })
        );
      }
    }

    if (addCustomerCodeIds.length == 1) {
      this.afterCustomersResult.customers.push(this.customers[addCustomerCodeIds[0]]);

    } else if (addCustomerCodeIds.length == 2) {
      // 複数のコード選択の場合
      let sliceCustomers = this.customers.slice(addCustomerCodeIds[0], addCustomerCodeIds[1] + 1);
      let sliceCustomerIds = new Array<Number>();
      let afterCustomerIds = new Array<Number>();
      let addCustomers = new Array<Customer>();

      for (let i = 0; i < sliceCustomers.length; i++) {
        sliceCustomerIds.push(sliceCustomers[i].id);
      }

      for (let i = 0; i < this.afterCustomersResult.customers.length; i++) {
        afterCustomerIds.push(this.afterCustomersResult.customers[i].id);
      }

      for (let i = 0; i < sliceCustomerIds.length; i++) {
        if (afterCustomerIds.indexOf(sliceCustomerIds[i]) < 0) {
          addCustomers.push(sliceCustomers[i]);
        }
      }
      this.afterCustomersResult.customers = this.afterCustomersResult.customers.concat(addCustomers);
    }

    this.afterCustomersResult.customers.sort(
      function (a, b) {
        if (a.id < b.id) return -1;
        if (a.id > b.id) return 1;
        return 0;
      });
  }

  /**
   * 変更対象の得意先から削除
   * @param index 行番号
   */
  public selectDeleteLine(index: number) {
    this.afterCustomersResult.customers.splice(index, 1);
  }

  /**
   * 変更対象の得意先を全削除
   */
  public allDelete() {

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.afterCustomersResult.customers.length = 0;
  }

  /**
   * エクスポート
   */
  public export() {
    let dataList = new Array<CustomerGroup>();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.customerGroupService.GetItems()
      .subscribe(result => {
        if (result) {
          dataList = result;

          // 件数チェック
          if (dataList.length <= 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
            processComponentRef.destroy();
            return;
          }

          let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.CUSTOMERGROUP_MASTER);
          let data: string = headers.join(",") + LINE_FEED_CODE;

          for (let index = 0; index < dataList.length; index++) {
            let dataItem: Array<any> = [];
            dataItem.push(this.userInfoService.Company.code);
            dataItem.push(dataList[index].parentCustomerCode);
            dataItem.push(dataList[index].childCustomerCode);
            dataItem = FileUtil.encloseItemBySymbol(dataItem);

            data = data + dataItem.join(",") + LINE_FEED_CODE;
          }
          let resultDatas: Array<any> = [];
          resultDatas.push(data);
          let isTryResult: boolean = false;

          try {
            FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
            isTryResult = true;

          } catch (error) {
            console.error(error);
          }

          this.processResultService.processAtOutput(
            this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);

        }
        processComponentRef.destroy();
      });
    this.openOptions();
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLES.MASTER_CUSTOMER_GROUP.id;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.clear();
    });
    this.openOptions();
  }

  /**
   * 印刷用のPDFをダウンロードする
   */
  public print() {
    let isTryResult: boolean = false;
    this.customerGroupService.GetReport()
      .subscribe(response => {
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          isTryResult = true;

        } catch (error) {
          console.error(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, isTryResult, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }

  /**
   * 得意先コードを全件取得する
   */
  public getCustomers() {
    let customerSearch = new CustomerSearch();
    customerSearch.isParent = 0;
    customerSearch.xorParentCustomerId = 0;
    this.customerService.GetItemsByCustomerSearch(customerSearch)
      .subscribe(result => {
        if (result) {
          this.customers = result;
        }
      });
  }

  public setAddButton() {
    this.isAddButton = true;
    if (!this.StringUtil.IsNullOrEmpty(this.customerCodeFromCtrl.value)
      || !this.StringUtil.IsNullOrEmpty(this.customerCodeToCtrl.value)) {
      this.isAddButton = false;
    }
  }

  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setParentCustomerCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.parentCustomerCodeTrigger.closePanel();
    }

    this.parentCustomerId = null;
    if (StringUtil.IsNullOrEmpty(this.parentCustomerCodeCtrl.value)) {
      this.parentCustomerNameCtrl.setValue("");

    } else {

      this.loadStart();
      this.customerService.GetItems(this.parentCustomerCodeCtrl.value, 1)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.parentCustomerId = response[0].id;
            this.parentCustomerCodeCtrl.setValue(response[0].code);
            this.parentCustomerNameCtrl.setValue(response[0].name);

            this.customerGroupService.GetItems(this.parentCustomerId)
              .subscribe(response => {

                this.processResultService.processAtGetData(
                  this.processCustomResult, response, true, this.partsResultMessageComponent);

                this.beforeCustomersResult = new CustomerGroupsResult();
                this.beforeCustomersResult.customerGroups = response;
                this.afterCustomersResult = new CustomersResult();
                this.afterCustomersResult.customers = new Array<Customer>();

                for (let i = 0; i < response.length; i++) {
                  let customer = new Customer;
                  customer.id = response[i].childCustomerId;
                  customer.code = response[i].childCustomerCode;
                  customer.name = response[i].childCustomerName;

                  this.afterCustomersResult.customers.push(customer);
                }

                this.customerCodeClear();
                this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

              });

            HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
          }
          else {
            //this.parentCustomerCodeCtrl.setValue("");
            this.parentCustomerNameCtrl.setValue("");
          }
        });
    }
  }


  public setCustomerCode(eventType: string, type: string = 'from') {

    if (eventType != EVENT_TYPE.BLUR) {
      if (type == 'from') {
        this.customerCodeFromTrigger.closePanel();
      } else {
        this.customerCodeToTrigger.closePanel();
      }
    }

    let formControlCode: FormControl;
    let formControlName: FormControl;
    let nextFormName: string;

    if (type == 'from') {
      this.customerCodeFromId = null;
      formControlCode = this.customerCodeFromCtrl
      formControlName = this.customerNameFromCtrl
      nextFormName = 'customerCodeToCtrl';

    } else {
      this.customerCodeToId = null;
      formControlCode = this.customerCodeToCtrl
      formControlName = this.customerNameToCtrl
      nextFormName = 'customerCodeFromCtrl';
    }

    if (StringUtil.IsNullOrEmpty(formControlCode.value)) {

      formControlCode.setValue("");
      formControlName.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);

    } else {
      let customerSearch = new CustomerSearch();
      customerSearch.isParent = 0;
      customerSearch.xorParentCustomerId = 0;
      customerSearch.codes = new Array<string>();
      customerSearch.codes.push(formControlCode.value);

      this.loadStart();
      this.customerService.GetItemsByCustomerSearch(customerSearch)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            if (type == 'from') {
              this.customerCodeFromId = response[0].id;
            } else {
              this.customerCodeToId = response[0].id;
            }
            formControlCode.setValue(response[0].code);
            formControlName.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);

          } else {
            // formControlCode.setValue("");
            formControlName.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);
          }
          this.setAddButton();
        });
    }
    this.setAddButton();
  }

}
