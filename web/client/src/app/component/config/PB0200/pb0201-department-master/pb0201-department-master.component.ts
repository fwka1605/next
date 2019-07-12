import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLES } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { DepartmentsResult } from 'src/app/model/departments-result.model';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Department } from 'src/app/model/department.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { forkJoin } from 'rxjs';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { SectionWithDepartmentMasterService } from 'src/app/service/Master/section-with-department-master.service';
import { BillingService } from 'src/app/service/billing.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb0201-department-master',
  templateUrl: './pb0201-department-master.component.html',
  styleUrls: ['./pb0201-department-master.component.css']
})

export class Pb0201DepartmentMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public departmentsResult: DepartmentsResult;

  public departmentCodeCtrl: FormControl; // 請求部門
  public departmentNameCtrl: FormControl;

  public staffCodeCtrl: FormControl;  // 担当者コード
  public staffNameCtrl: FormControl;

  public noteCtrl: FormControl; // 備考

  public undefineCtrl: FormControl;

  public staffId: number;
  public selectIndex: number; // 選択された科目

  @ViewChild('staffCodeInput', { read: MatAutocompleteTrigger }) staffCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public departmentService: DepartmentMasterService,
    public staffService: StaffMasterService,
    public billingService: BillingService,
    public sectionWithDepartmentService: SectionWithDepartmentMasterService,
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
    this.getDepartmentData();
  }

  public setControlInit() {
    this.departmentCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameCtrl = new FormControl("", [Validators.required]);

    this.staffCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameCtrl = new FormControl("");

    this.noteCtrl = new FormControl("");

    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      departmentCodeCtrl: this.departmentCodeCtrl,
      departmentNameCtrl: this.departmentNameCtrl,

      staffCodeCtrl: this.staffCodeCtrl,
      staffNameCtrl: this.staffNameCtrl,

      noteCtrl: this.noteCtrl,

      undefineCtrl: this.undefineCtrl,
    })
  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeCtrl);
    }
    else if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.ALPHA) {
      FormatterUtil.setCodeFormatter(this.departmentCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeCtrl);
    }
    else if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.ALPHA) {
      FormatterUtil.setCodeFormatter(this.staffCodeCtrl);
    }
  }

  public clear() {
    this.MyFormGroup.reset();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.departmentCodeCtrl.enable();
    this.staffId = null;

    this.panelOpenState = false;
    this.panel.close();

  }

  public setAutoComplete() {
    // 担当者
    this.initAutocompleteStaffs(this.staffCodeCtrl, this.staffService, 0);
  }

  /**
   * データ取得
   */
  public getDepartmentData() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.departmentsResult = new DepartmentsResult();

    this.departmentService.GetItems()
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.departmentsResult.departments = new Array<Department>();
          this.departmentsResult.departments = result;
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

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    let registryDatas = new Array<Department>();
    let registryData = new Department();
    let isRegistry = false;

    if (this.ComponentStatus === COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.companyId = this.userInfoService.Company.id;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus === COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.departmentsResult.departments[this.selectIndex];

    } else {
      return;
    }

    registryData.code = this.departmentCodeCtrl.value;
    registryData.name = this.departmentNameCtrl.value;
    registryData.staffId = this.staffId;
    registryData.note = this.noteCtrl.value;
    registryData.updateBy = this.userInfoService.LoginUser.id;
    registryData = FileUtil.replaceNull(registryData);
    registryDatas.push(registryData);

    this.departmentService.Save(registryDatas)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getDepartmentData();
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
    // processComponentRef.destroy();

    let registryData = this.departmentsResult.departments[this.selectIndex];
    let existDeleteResultList = new Array<ExistDeleteResult>();

    for (let i = 0; i < 3; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '請求部門コード';
    }

    let existStaffMasterResponse =
      this.staffService.ExistDepartment(registryData.id);
    existDeleteResultList[0].masterName = TABLES.MASTER_STAFF.name;

    let existBillingResponse =
      this.billingService.ExistDepartment(registryData.id);
    existDeleteResultList[1].masterName = TABLES.BILLING.name;

    let existSectionWithDepartmentResponse =
      this.sectionWithDepartmentService.ExistDepartment(registryData.id);
    existDeleteResultList[2].masterName = TABLES.SECTION_WITH_DEPARTMENT.name;

    // ３つの処理の待機
    forkJoin(
      existStaffMasterResponse,
      existBillingResponse,
      existSectionWithDepartmentResponse
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

            this.departmentService.Delete(registryData)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.clear();
                  this.getDepartmentData();
                  processComponentRef.destroy();
                }
              });
          }
        });
      },
      error => {
        processComponentRef.destroy();
        console.log(error)
      }
    );
  }

  /**
   * エクスポート
   */
  public export() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    let dataList = this.departmentsResult.departments;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.DEPARTMENT_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].note);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let result = false;
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
   * 印刷用のPDFをダウンロードする
   */
  public print() {
    let result = false;
    this.departmentService.GetReport()
      .subscribe(response => {
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          result = true;
        } catch (error) {
          console.log(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, result, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }

  /**
   * 各テーブルのデータを取得する
   * @param table テーブル種別
   * @param keyCode イベント種別
   */
  public openMasterModal(table: number) {
    if(this.staffCodeTrigger!=null){this.staffCodeTrigger.closePanel();}

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLES.MASTER_STAFF.id:
            {
              this.staffId = componentRef.instance.SelectedObject.id;
              this.staffCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.staffNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
        }
      }

      componentRef.destroy();
    });
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLES.MASTER_DEPARTMENT.id;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getDepartmentData();
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
    this.departmentsResult.departments[index];

    this.departmentCodeCtrl.setValue(this.departmentsResult.departments[index].code);  // 請求部門
    this.departmentNameCtrl.setValue(this.departmentsResult.departments[index].name);

    this.staffCodeCtrl.setValue(this.departmentsResult.departments[index].staffCode);  // 担当者コード
    this.staffNameCtrl.setValue(this.departmentsResult.departments[index].staffName);
    this.staffId = this.departmentsResult.departments[index].staffId;
    this.setStaffCode(null);

    this.noteCtrl.setValue(this.departmentsResult.departments[index].note);  // 備考

    // ステータス更新
    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // コードは変更不可にする。
    this.departmentCodeCtrl.disable();


  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setDepartmentCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.departmentCodeCtrl.value)) {
      let codeValue = StringUtil.setUpperCase(this.departmentCodeCtrl.value);
      this.departmentCodeCtrl.setValue(codeValue);
      let findIndex = this.departmentsResult.departments.findIndex((item) => {
        return (item.code === codeValue);
      });
      if (0 <= findIndex) {
        this.selectLine(findIndex);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentNameCtrl', eventType);
  }

  public setDepartmentName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      if(this.staffCodeTrigger!=null){this.staffCodeTrigger.closePanel();}
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {
      this.staffService.GetItems(this.staffCodeCtrl.value)
        .subscribe(response => {
          if (response != undefined && response != null && 0 < response.length) {
            this.staffCodeCtrl.setValue(response[0].code);
            this.staffNameCtrl.setValue(response[0].name);
            this.staffId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'noteCtrl', eventType);
          } else {
            //this.staffCodeCtrl.setValue("");
            this.staffNameCtrl.setValue("");
            this.staffId = null;

          }
        });
    } else {
      this.staffNameCtrl.setValue("");
      this.staffId = null;
      HtmlUtil.nextFocusByName(this.elementRef, 'noteCtrl', eventType);
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setNote(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeCtrl', eventType);
  }

}
