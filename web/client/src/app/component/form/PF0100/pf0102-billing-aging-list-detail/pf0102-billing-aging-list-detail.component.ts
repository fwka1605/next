import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { DateUtil } from 'src/app/common/util/date-util';
import { DatePipe }     from '@angular/common';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { BillingAgingListService } from 'src/app/service/billing-aging-list.service';
import { BillingAgingListDetail } from 'src/app/model/billing-aging-list-detail.model';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ColumnNameSettigUtil } from 'src/app/common/util/column-name-setting-util';

const ymd = 'yyyy/MM/dd';

@Component({
  selector: 'app-pf0102-billing-aging-list-detail',
  templateUrl: './pf0102-billing-aging-list-detail.component.html',
  styleUrls: ['./pf0102-billing-aging-list-detail.component.css']
})
export class Pf0102BillingAgingListDetailComponent extends BaseComponent implements OnInit {

  public billingAgingListDetails: BillingAgingListDetail[];
  public columnNameSettingsResult: ColumnNameSettingsResult;

  public departmentCodeFromCtrl: FormControl;  // 請求部門コード
  public departmentNameFromCtrl: FormControl;
  public departmentCodeToCtrl: FormControl;
  public departmentNameToCtrl: FormControl;
  
  public staffCodeFromCtrl: FormControl;  // 担当者コード
  public staffNameFromCtrl: FormControl;
  public staffCodeToCtrl: FormControl;
  public staffNameToCtrl: FormControl;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public processResultService: ProcessResultService,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public billingAgingListService: BillingAgingListService,
    public columnNameSettingService:ColumnNameSettingMasterService,
    private datePipe: DatePipe,
  ) { 
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title,true);
      }
    });

  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    this.billingAgingListDetails = this.billingAgingListService.BillingAgingListDetails;
    this.setSearchCondition();

    this.columnNameSettingService.Get(CategoryType.Billing)
      .subscribe(response => {
        if (response != this.PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.columnNameSettingsResult = new ColumnNameSettingsResult();
          this.columnNameSettingsResult.columnNames = response;
        }
      });

  }

  public setControlInit() {

    this.departmentCodeFromCtrl = new FormControl("");  // 請求部門コード
    this.departmentNameFromCtrl = new FormControl("");
    this.departmentCodeToCtrl = new FormControl("");
    this.departmentNameToCtrl = new FormControl("");
    
    this.staffCodeFromCtrl = new FormControl("");  // 担当者コード
    this.staffNameFromCtrl = new FormControl("");
    this.staffCodeToCtrl = new FormControl("");
    this.staffNameToCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      departmentCodeFromCtrl: this.departmentCodeFromCtrl,  // 請求部門コード
      departmentNameFromCtrl: this.departmentNameFromCtrl,
      departmentCodeToCtrl: this.departmentCodeToCtrl,
      departmentNameToCtrl: this.departmentNameToCtrl,
      
      staffCodeFromCtrl: this.staffCodeFromCtrl,  // 担当者コード
      staffNameFromCtrl: this.staffNameFromCtrl,
      staffCodeToCtrl: this.staffCodeToCtrl,
      staffNameToCtrl: this.staffNameToCtrl,
    });

  }

  public setFormatter() {

  }

  public clear() {
    this.MyFormGroup.reset();

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

      case BUTTON_ACTION.PRINT:
        //this.print();
        break;

      case BUTTON_ACTION.EXPORT:
        this.export();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }

  }

  //////////////////////////////////////////////////////////////
  public setSearchCondition() {
    const option = this.billingAgingListService.OptionInfo;
    this.departmentCodeFromCtrl.setValue(option.departmentCodeFrom);  //  請求部門コード
    this.departmentNameFromCtrl.setValue(option.departmentNameFrom);
    this.departmentCodeToCtrl.setValue(option.departmentCodeTo);
    this.departmentNameToCtrl.setValue(option.departmentNameTo);

    this.staffCodeFromCtrl.setValue(option.staffCodeFrom);  //  担当者コード
    this.staffNameFromCtrl.setValue(option.staffNameFrom);
    this.staffCodeToCtrl.setValue(option.staffCodeTo);
    this.staffNameToCtrl.setValue(option.staffNameTo);    
    
  }

  //////////////////////////////////////////////////////////////
  public back() {
    this.router.navigate(['main/PF0101', { "process": "back" }]);
  }

  //////////////////////////////////////////////////////////////
  public export() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    let dataList = this.billingAgingListDetails;

    let headers = REPORT_HEADER.BILLING_AGING_LIST_DETAIL_FORM;

    if (this.userInfoService.ApplicationControl.useForeignCurrency == 0) {
      const index = headers.indexOf("通貨コード");
      if (index >= 0) headers.splice(index,1);      
    }
    
    let note1 = this.getColumnAlias("Note1");
    const idx = headers.indexOf("備考");
    if (idx >= 0) {
      headers[idx] = note1;
    }

    let data: string = FileUtil.encloseItemBySymbol(headers).join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(dataList[index].customerCode);
      dataItem.push(dataList[index].customerName);
      if(this.userInfoService.ApplicationControl.useForeignCurrency == 1) {
        dataItem.push(dataList[index].currencyCode);
      }
      dataItem.push(this.datePipe.transform(dataList[index].billedAt, ymd));
      dataItem.push(this.datePipe.transform(dataList[index].dueAt, ymd));
      dataItem.push(this.datePipe.transform(dataList[index].salesAt, ymd));
      dataItem.push(dataList[index].billingAmount);
      dataItem.push(dataList[index].remainAmount);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].staffName);
      dataItem.push(dataList[index].invoiceCode);
      dataItem.push(dataList[index].note);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let customerCode = this.billingAgingListDetails[0].customerCode + "_";
    let result = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);
    try {
      FileUtil.download(resultDatas, this.Title + `${customerCode}` + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      result = true;

    } catch (error) {
      console.error(error);
    }

    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);
    this.openOptions();

    processComponentRef.destroy();


  }

  //////////////////////////////////////////////////////////////
  public getColumnAlias(columnName: string): string {
    if (this.columnNameSettingsResult != null) {
      return ColumnNameSettigUtil.getColumnAlias(this.columnNameSettingsResult.columnNames, columnName);
    }
    return "";
  }  

}
