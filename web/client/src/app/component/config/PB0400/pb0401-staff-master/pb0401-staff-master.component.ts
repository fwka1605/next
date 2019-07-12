import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { CustomValidators } from 'ng5-validation';
import { StaffsResult } from 'src/app/model/staffs-result.model';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { Staff } from 'src/app/model/staff.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { forkJoin } from 'rxjs';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { BillingService } from 'src/app/service/billing.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb0401-staff-master',
  templateUrl: './pb0401-staff-master.component.html',
  styleUrls: ['./pb0401-staff-master.component.css']
})
export class Pb0401StaffMasterComponent extends BaseComponent implements OnInit {

  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public staffsResult: StaffsResult;

  public staffCodeCtrl: FormControl;  // 担当者コード
  public staffNameCtrl: FormControl;

  public departmentCodeCtrl: FormControl; // 請求部門
  public departmentNameCtrl: FormControl;

  public mailCtrl: FormControl;  // メールアドレス

  public telCtrl: FormControl; // Tel
  public faxCtrl: FormControl; // Fax  

  public undefineCtrl: FormControl;

  public selectIndex: number;
  public departmentId: number;

  @ViewChild('departmentCodeInput', { read: MatAutocompleteTrigger }) departmentCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public staffService: StaffMasterService,
    public loginUserService: LoginUserMasterService,
    public customerService: CustomerMasterService,
    public billingService: BillingService,
    public processResultService: ProcessResultService,
    public departmentMasterService: DepartmentMasterService

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
    this.getStaffData();
  }

  public setControlInit() {

    this.staffCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameCtrl = new FormControl("", [Validators.required]);

    this.departmentCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameCtrl = new FormControl("");

    this.mailCtrl = new FormControl("", [CustomValidators.email]);

    // this.telCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.TEL))]);  // TEL
    // this.faxCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.TEL))]);  // FAX
    this.telCtrl = new FormControl("");  // TEL
    this.faxCtrl = new FormControl("");  // FAX

    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      staffCodeCtrl: this.staffCodeCtrl,
      staffNameCtrl: this.staffNameCtrl,

      departmentCodeCtrl: this.departmentCodeCtrl,
      departmentNameCtrl: this.departmentNameCtrl,

      mailCtrl: this.mailCtrl,

      telCtrl: this.telCtrl,
      faxCtrl: this.faxCtrl,

      undefineCtrl: this.undefineCtrl,

    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.staffCodeCtrl);
    }
    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeCtrl);
    }
    FormatterUtil.setTelFormatter(this.telCtrl);
    FormatterUtil.setTelFormatter(this.faxCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.departmentId = null;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.staffCodeCtrl.enable();

    this.panelOpenState = false;
    this.panel.close();

  }

  public setAutoComplete() {

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeCtrl, this.departmentMasterService, 0);

  }

  /**
   * データ取得
   */
  public getStaffData() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.staffsResult = new StaffsResult();
    this.staffService.GetItems()
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(this.processCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.staffsResult.staffs = response;
        }
        processComponentRef.destroy();
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
   * データ登録・編集
   */
  public registry() {
    let registryData = new Staff();
    let isRegistry: boolean = false;

    if (this.ComponentStatus === COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus === COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.staffsResult.staffs[this.selectIndex];

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    registryData.code = this.staffCodeCtrl.value;
    registryData.name = this.staffNameCtrl.value;
    registryData.departmentId = this.departmentId;
    registryData.mail = this.mailCtrl.value === null ? '' : this.mailCtrl.value;
    registryData.tel = this.telCtrl.value === null ? '' : this.telCtrl.value;
    registryData.fax = this.faxCtrl.value === null ? '' : this.faxCtrl.value;

    this.staffService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getStaffData();
        }
        processComponentRef.destroy();
      });
  }

  /**
   * データ削除
   */
  public delete() {
    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    let existDeleteResultList = new Array<ExistDeleteResult>();
    let staffData = this.staffsResult.staffs[this.selectIndex];

    for (let i = 0; i < 3; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '営業担当者コード';
    }

    let existLoginUserResponse = this.loginUserService.ExitStaff(staffData.id);
    existDeleteResultList[0].masterName = TABLES.MASTER_LOGIN_USER.name;

    let existCustomerResponse = this.customerService.ExistStaff(staffData.id);
    existDeleteResultList[1].masterName = TABLES.MASTER_CUSTOMER.name;

    let existBillingResponse = this.billingService.ExistStaff(staffData.id);
    existDeleteResultList[2].masterName = TABLES.BILLING.name;

    // ３つの処理の待機
    forkJoin(
      existLoginUserResponse,
      existCustomerResponse,
      existBillingResponse
    ).subscribe(
      responseList => {
        processComponentRef.destroy();
        this.processCustomResult = this.processResultService.processAtExist(
          this.processCustomResult, responseList, existDeleteResultList, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.WARNING) {
          return;
        }

        // 削除処理
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        componentRef.instance.ActionName = "削除"
        componentRef.instance.Closing.subscribe(() => {
          componentRef.destroy();

          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

            let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
            let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
            // processComponentRef.destroy();   

            this.staffService.Delete(staffData)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.clear();
                  this.getStaffData();
                }
                processComponentRef.destroy();
              });
          }
        });
      },
      error => {
        console.log(error)
      }
    );
  }

  /**
   * エクスポート処理
   */
  public export() {
    let dataList = this.staffsResult.staffs;
    let result: boolean = false;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.STAFF_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].departmentCode);
      dataItem.push(dataList[index].mail);
      dataItem.push(dataList[index].tel);
      dataItem.push(dataList[index].fax);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    try {
      FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      result = true;

    } catch (error) {
      console.error(error);
    }
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);
    this.openOptions();

    processComponentRef.destroy();
  }

  /**
   * 請求部門コード呼び出し
   * @param table 
   * @param keyCode 
   */
  public openMasterModal(table: TABLE_INDEX) {

    if(this.departmentCodeTrigger!=null){this.departmentCodeTrigger.closePanel();}
      
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              this.departmentId = componentRef.instance.SelectedObject.id;
              this.departmentCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.departmentNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }

        }
      }

      componentRef.destroy();
    });
  }

  /**
 * 印刷用のPDFをダウンロードする
 */
  public print() {
    let result: boolean = false;
    this.staffService.GetReport()
      .subscribe(response => {
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          result = true;

        } catch (error) {
          console.error(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, result, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_STAFF;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getStaffData();
    });
    this.openOptions();
  }

  /**
   * 選択した項目をフォームに表示させる
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    this.panelOpenState = true;
    this.panel.open();

    this.selectIndex = index;

    this.staffCodeCtrl.setValue(this.staffsResult.staffs[index].code);  // 請求部門
    this.staffNameCtrl.setValue(this.staffsResult.staffs[index].name);

    this.departmentCodeCtrl.setValue(this.staffsResult.staffs[index].departmentCode);  // 請求部門
    this.departmentNameCtrl.setValue(this.staffsResult.staffs[index].departmentName);
    this.departmentId = this.staffsResult.staffs[index].departmentId;


    this.mailCtrl.setValue(this.staffsResult.staffs[index].mail);

    this.telCtrl.setValue(this.staffsResult.staffs[index].tel);  // Tel
    this.faxCtrl.setValue(this.staffsResult.staffs[index].fax);  // Fax

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.staffCodeCtrl.disable();
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setStaffCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {
      let codeValue = StringUtil.setUpperCase(this.staffCodeCtrl.value);
      this.staffCodeCtrl.setValue(codeValue);
      let findIndex = this.staffsResult.staffs.findIndex((item) => {
        return (item.code === codeValue);
      });
      if (0 <= findIndex) {
        this.selectLine(findIndex);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'staffNameCtrl', eventType);
  }

  public setStaffName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setDepartmentCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.departmentCodeTrigger!=null){this.departmentCodeTrigger.closePanel();}
    }

    this.departmentId = null;

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeCtrl.value)) {
      this.loadStart();
      this.departmentMasterService.GetItems(this.departmentCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {
            this.departmentId = response[0].id;
            this.departmentCodeCtrl.setValue(response[0].code);
            this.departmentNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'mailCtrl', eventType);
          } else {
            //this.departmentCodeCtrl.setValue("");
            this.departmentNameCtrl.setValue("");
          }
        });
    } else {
      this.departmentCodeCtrl.setValue("");
      this.departmentNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'mailCtrl', eventType);
    }
  }

  public setMail(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'telCtrl', eventType);
  }

  public setTel(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'faxCtrl', eventType);
  }

  public setFax(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
  }

}
