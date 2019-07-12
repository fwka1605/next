import { Component, OnInit, ViewContainerRef, ElementRef, ViewChild, TemplateRef } from '@angular/core';
import { ComponentFactoryResolver } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ModalCustomerDetailComponent } from 'src/app/component/modal/modal-customer-detail/modal-customer-detail.component';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { COMPONENT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { CustomersResult } from 'src/app/model/customers-result.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Customer } from 'src/app/model/customer.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalImporterSettingCustomerComponent } from 'src/app/component/modal/modal-importer-setting-customer/modal-importer-setting-customer.component';
import { ModalExportMethodSelectorCustomerComponent } from 'src/app/component/modal/modal-export-method-selector-customer/modal-export-method-selector-customer.component';
import { ModalImportMethodSelectorCustomerComponent } from 'src/app/component/modal/modal-import-method-selector-customer/modal-import-method-selector-customer.component';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FunctionType } from 'src/app/common/const/kbn.const';


@Component({
  selector: 'app-pb0501-customer-master',
  templateUrl: './pb0501-customer-master.component.html',
  styleUrls: ['./pb0501-customer-master.component.css']
})
export class Pb0501CustomerMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public searchCondCtrl: FormControl;
  public searchCustomersResult: CustomersResult;

  public readonly autocompleteCustomerPlaceholder = "得意先名称/コード";
  public readonly autocompleteCustomerClass = "input-size--400";
  public readonly autocompleteCustomerControlName = "searchCondCtrl";


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public customerMasterService: CustomerMasterService,
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
    this.initAutocompleteCustomers(this.searchCondCtrl, this.customerMasterService, 0);
  }


  public setControlInit() {

    this.searchCondCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      searchCondCtrl: this.searchCondCtrl,

    })
  }

  public setFormatter() {
  }

  public clear() {
    this.MyFormGroup.reset();
    this.search(false);

    // HtmlUtil.nextFocusByName(this.elementRef, 'searchCondCtrl', EVENT_TYPE.NONE);
  }

  /**
   * データ追加のページを表示する
   */
  public create() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalCustomerDetailComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.search(false);
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
      case BUTTON_ACTION.SEARCH:
        this.search(true);
        break;
      case BUTTON_ACTION.REGISTRY:
        this.create();
        break;

      case BUTTON_ACTION.SETTING:
        this.openCaptureSettingModal()
        break;

      case BUTTON_ACTION.IMPORT:
        this.openImporterSettingModal()
        break;

      case BUTTON_ACTION.EXPORT:
        this.openOutputMethodSelector(2)
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * 印刷
   */
  public openOutputMethodSelector(outputType: number) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalExportMethodSelectorCustomerComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    if (outputType == 1) {
      componentRef.instance.Title = this.Title;
      componentRef.instance.OutputTitle = '印刷';
    } else {
      componentRef.instance.OutputTitle = 'エクスポート';
    }
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.openOptions();
    });
  }

  /**
   * インポート
   */
  public openImporterSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorCustomerComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Title = this.Title;
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.openOptions();
      this.search(false);
    });
  }

  /**
   * 取込設定
   */
  public openCaptureSettingModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImporterSettingCustomerComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Title = this.Title;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
  }

  /**
   * 選択した行のデータを取得する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalCustomerDetailComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Customer = this.searchCustomersResult.customers[index]; // 0～99のランダム値を渡す
    componentRef.instance.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    this.customerMasterService.selectCustmer = this.searchCustomersResult.customers[index];

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.search(false);
    });
  }

  /**
   * データ検索
   */
  public search(isMessage: boolean) {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.searchCustomersResult = new CustomersResult();
    this.searchCustomersResult.processResult = new ProcessResult();
    this.searchCustomersResult.processResult.result = false;
    this.searchCustomersResult.customers = new Array<Customer>();

    this.customerMasterService.GetItems()
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.searchCustomersResult.processResult.result = true;
          this.searchCustomersResult.customers = response;

          if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchCondCtrl.value)) {
            let searchKeyCtrl = this.searchCondCtrl.value
            this.searchCustomersResult.customers = this.searchCustomersResult.customers.filter(
              function (customer: Customer) {

                return customer.code.indexOf(searchKeyCtrl) != -1 || customer.name.indexOf(searchKeyCtrl) != -1;
              }
            )
          }

          this.processCustomResult = this.processResultService.processAtGetData(
            this.processCustomResult, this.searchCustomersResult.customers, isMessage, this.partsResultMessageComponent);

          processComponentRef.destroy();
        }
      });

  }

}
