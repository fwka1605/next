
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
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
import { Department } from 'src/app/model/department.model';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { SectionWithDepartmentMasterService } from 'src/app/service/Master/section-with-department-master.service';
import { SectionWithDepartmentsResult } from 'src/app/model/section-with-departments-result.model';
import { SectionWithDepartment } from 'src/app/model/section-with-department.model';
import { DepartmentSearch } from 'src/app/model/department-search.model';
import { SectionWithDepartmentSearch } from 'src/app/model/section-with-department-search.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';

@Component({
  selector: 'app-pb1201-section-with-department-master',
  templateUrl: './pb1201-section-with-department-master.component.html',
  styleUrls: ['./pb1201-section-with-department-master.component.css']
})
export class Pb1201SectionWithDepartmentMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public beforeDepartmentsResult: SectionWithDepartmentsResult;
  public afterDepartmentsResult: SectionWithDepartmentsResult;

  public sectionCodeCtrl: FormControl;  // 入金部門
  public sectionNameCtrl: FormControl;

  public cbxDepartmentCtrl: FormControl;  // 得意先
  public departmentCodeFromCtrl: FormControl;  // 得意先
  public departmentNameFromCtrl: FormControl;
  public departmentCodeToCtrl: FormControl;
  public departmentNameToCtrl: FormControl;

  public departments: Array<Department>;
  public departmentCodeFromId: number;
  public departmentCodeToId: number;
  public sectionId: number;

  public isAddButton: boolean = true;

  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeFromInput', { read: MatAutocompleteTrigger }) departmentCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('departmentCodeToInput', { read: MatAutocompleteTrigger }) departmentCodeToTrigger: MatAutocompleteTrigger;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public departmentService: DepartmentMasterService,
    public sectionService: SectionMasterService,
    public sectionWithDepartmentService: SectionWithDepartmentMasterService,
    public processResultService: ProcessResultService,
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

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }

    this.setAutoComplete();
  }

  public setControlInit() {
    this.sectionCodeCtrl = new FormControl('', [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);   // 債権代表者
    this.sectionNameCtrl = new FormControl('');

    this.cbxDepartmentCtrl = new FormControl(false);
    this.departmentCodeFromCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);     // 得意先
    this.departmentNameFromCtrl = new FormControl('');
    this.departmentCodeToCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameToCtrl = new FormControl('');
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      sectionCodeCtrl: this.sectionCodeCtrl,   // 債権代表者
      sectionNameCtrl: this.sectionNameCtrl,

      cbxDepartmentCtrl: this.cbxDepartmentCtrl,
      departmentCodeFromCtrl: this.departmentCodeFromCtrl,  // 得意先
      departmentNameFromCtrl: this.departmentNameFromCtrl,
      departmentCodeToCtrl: this.departmentCodeToCtrl,
      departmentNameToCtrl: this.departmentNameToCtrl,
    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeFromCtrl);
      FormatterUtil.setNumberFormatter(this.departmentCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.departmentCodeToCtrl);
    }
  }

  public clear() {
    this.MyFormGroup.reset();
    this.beforeDepartmentsResult = null;
    this.afterDepartmentsResult = null;
    this.departmentCodeFromId = null;
    this.departmentCodeToId = null;
    this.sectionId = null;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.departmentCodeFromCtrl.disable();
    this.departmentCodeToCtrl.disable();
    this.isAddButton = true;
  }

  public setAutoComplete() {

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeCtrl, this.sectionService, 0);

    // 請求部門は入金部門を指定された時に設定にする。
  }

  public departmentCodeClear() {
    this.departmentCodeFromCtrl.setValue('');
    this.departmentNameFromCtrl.setValue('');
    this.departmentCodeToCtrl.setValue('');
    this.departmentNameToCtrl.setValue('');

    this.departmentCodeFromCtrl.enable();
    this.departmentCodeToCtrl.enable();

    // 請求部門
    let departmentSearch = new DepartmentSearch();
    departmentSearch.withSectionId = this.sectionId;
    this.initAutocompleteDepartments(this.departmentCodeFromCtrl, this.departmentService, 0, departmentSearch);
    this.initAutocompleteDepartments(this.departmentCodeToCtrl, this.departmentService, 1, departmentSearch);

  }

  /**
   * 各テーブルからデータを取得
   * @param table 対象テーブル
   * @param keyCode イベント
   * @param type フォーム種別
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {
    this.isAddButton = true;

    this.sectionCodeTrigger.closePanel();
    this.departmentCodeFromTrigger.closePanel();
    this.departmentCodeToTrigger.closePanel();


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;
    if (table == TABLE_INDEX.MASTER_SECTION_WITH_DEPARTMENT) {
      componentRef.instance.SelectedId = this.sectionId;
    }

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_SECTION_WITH_DEPARTMENT:
            {
              if (type === "from") {
                this.departmentCodeFromId = componentRef.instance.SelectedObject.id
                this.departmentCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.departmentNameFromCtrl.setValue(componentRef.instance.SelectedName);
              }
              else if (type === "to") {
                this.departmentCodeToId = componentRef.instance.SelectedObject.id
                this.departmentCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.departmentNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              this.setAddButton();
              break;
            }
          case TABLE_INDEX.MASTER_SECTION:
            {
              this.sectionId = componentRef.instance.SelectedObject.id;
              this.sectionCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.sectionNameCtrl.setValue(componentRef.instance.SelectedName);
              this.getSectionWithDepartment();
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
    if (this.StringUtil.IsNullOrEmpty(this.sectionNameCtrl.value)) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "入金部門コード");
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    let importData = new MasterImportData<SectionWithDepartment>();
    let insertItems = new Array<SectionWithDepartment>();
    let deleteItems = new Array<SectionWithDepartment>();
    const beforeDepartments = this.beforeDepartmentsResult.sectionDepartments;
    const afterDepartments = this.afterDepartmentsResult.sectionDepartments;

    // 追加と削除の区別
    for (let i = 0; i < afterDepartments.length; i++) {
      let sectionWithDepartment = new SectionWithDepartment();
      sectionWithDepartment.sectionId = this.sectionId;
      sectionWithDepartment.updateBy = this.userInfoService.LoginUser.id;
      sectionWithDepartment.createBy = this.userInfoService.LoginUser.id;

      // 新しいIDが　beforeにある：何もしない、beforeにない：追加)
      let findIndex = beforeDepartments.findIndex((item) => {
        return (item.departmentId === afterDepartments[i].departmentId);
      });
      if (findIndex < 0) {
        sectionWithDepartment.departmentId = afterDepartments[i].departmentId;
        insertItems.push(sectionWithDepartment);
      }
    }

    for (let i = 0; i < beforeDepartments.length; i++) {
      let sectionWithDepartment = new SectionWithDepartment();
      sectionWithDepartment.sectionId = this.sectionId;
      sectionWithDepartment.updateBy = this.userInfoService.LoginUser.id;
      sectionWithDepartment.updateAt = beforeDepartments[i].updateAt;

      // 既存IDが　afterにある：何もしない、afterにない：削除)
      let findIndex = afterDepartments.findIndex((item) => {
        return (item.departmentId === beforeDepartments[i].departmentId);
      });
      if (findIndex < 0) {
        sectionWithDepartment.departmentId = beforeDepartments[i].departmentId;
        deleteItems.push(sectionWithDepartment);
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
    importData.updateItems = new Array<SectionWithDepartment>();

    this.sectionWithDepartmentService.Save(importData)
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
    let addDepartmentCodeIndexs = new Array<number>();
    let departmentCodeFrom = this.StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value);
    let departmentCodeTo = this.StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value);

    if (departmentCodeFrom && departmentCodeTo) {
      // From/To ともに未入力
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "追加する請求部門コード");
      return;
    }

    let findIndex: number = -1;
    if (!departmentCodeFrom) {
      findIndex = this.getDepertmentIdIndex(this.departmentCodeFromId);
      if (0 <= findIndex) {
        addDepartmentCodeIndexs.push(findIndex);
      }
    }

    if (!departmentCodeTo) {
      findIndex = this.getDepertmentIdIndex(this.departmentCodeToId);
      if (0 <= findIndex) {
        addDepartmentCodeIndexs.push(findIndex);
      }
    }

    if (addDepartmentCodeIndexs.length == 1) {
      let sectionDepartment = new SectionWithDepartment();
      sectionDepartment.departmentId = this.departments[addDepartmentCodeIndexs[0]].id;
      sectionDepartment.departmentCode = this.departments[addDepartmentCodeIndexs[0]].code;
      sectionDepartment.departmentName = this.departments[addDepartmentCodeIndexs[0]].name;
      this.afterDepartmentsResult.sectionDepartments.push(sectionDepartment);

    } else if (addDepartmentCodeIndexs.length == 2) {
      // 複数のコード選択の場合
      let sliceDepartments = this.departments.slice(addDepartmentCodeIndexs[0], addDepartmentCodeIndexs[1] + 1);
      let addDepartments = new Array<SectionWithDepartment>();

      for (let i = 0; i < sliceDepartments.length; i++) {
        findIndex = this.getDepertmentIdIndex(sliceDepartments[i].id);
        if (0 <= findIndex) {
          let addDepartment = new SectionWithDepartment();
          addDepartment.departmentId = sliceDepartments[i].id;
          addDepartment.departmentCode = sliceDepartments[i].code;
          addDepartment.departmentName = sliceDepartments[i].name;
          addDepartments.push(addDepartment);
        }
      }
      this.afterDepartmentsResult.sectionDepartments = this.afterDepartmentsResult.sectionDepartments.concat(addDepartments);
    }

    this.afterDepartmentsResult.sectionDepartments.sort(
      function (a, b) {
        if (a.departmentCode < b.departmentCode) return -1;
        if (a.departmentCode > b.departmentCode) return 1;
        return 0;
      });
  }

  /**
   * 登録可能の請求部門コードであるかチェック
   * @param targetId チェック対象のID
   */
  public getDepertmentIdIndex(targetId: number) {
    let isAdd: boolean = false;
    let findIndex: number = -1;

    // 登録済みと重複チェック
    findIndex = this.afterDepartmentsResult.sectionDepartments.findIndex((item) => {
      return (item.departmentId === targetId);
    });

    isAdd = findIndex < 0 ? true : false;

    if (isAdd) {
      // 請求部門コード群から該当のIdを検索する
      findIndex = this.departments.findIndex((item) => {
        return (item.id === targetId);
      });
      return findIndex;
    }
    return -1;
  }

  /**
   * 変更対象の得意先から削除
   * @param index 行番号
   */
  public selectDeleteLine(index: number) {
    this.afterDepartmentsResult.sectionDepartments.splice(index, 1);
  }

  /**
   * 変更対象の得意先を全削除
   */
  public allDelete() {
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.afterDepartmentsResult.sectionDepartments.length = 0;
  }

  /**
   * エクスポート
   */
  public export() {
    let dataList = new Array<SectionWithDepartment>();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    this.sectionWithDepartmentService.GetItems()
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

          let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.SECTION_WITH_DEPARTMENT_MASTER);
          let data: string = headers.join(",") + LINE_FEED_CODE;

          for (let index = 0; index < dataList.length; index++) {
            let dataItem: Array<any> = [];
            dataItem.push(this.userInfoService.Company.code);
            dataItem.push(dataList[index].sectionCode);
            dataItem.push(dataList[index].departmentCode);
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

    componentRef.instance.TableIndex = TABLES.MASTER_SECTION_WITH_DEPARTMENT.id;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.clear();
    });
    this.openOptions();
  }

  /**
   * 追加ボタンの制御
   */
  public setAddButton() {
    this.isAddButton = true;
    if (!this.StringUtil.IsNullOrEmpty(this.departmentCodeFromCtrl.value)
      || !this.StringUtil.IsNullOrEmpty(this.departmentCodeToCtrl.value)) {
      this.isAddButton = false;
    }
  }

  /**
   * 登録済みの請求部門コードを取得
   */
  public getSectionWithDepartment() {
    let sectionWithDepartmentSearch = new SectionWithDepartmentSearch();
    sectionWithDepartmentSearch.sectionId = this.sectionId;
    this.sectionWithDepartmentService.GetItems(sectionWithDepartmentSearch)
      .subscribe(response => {
        this.processResultService.processAtGetData(
          this.processCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.beforeDepartmentsResult = new SectionWithDepartmentsResult();
          this.beforeDepartmentsResult.sectionDepartments = new Array<SectionWithDepartment>();
          this.beforeDepartmentsResult.sectionDepartments = response;
          this.afterDepartmentsResult = new SectionWithDepartmentsResult();
          this.afterDepartmentsResult.sectionDepartments = new Array<SectionWithDepartment>();
          this.beforeDepartmentsResult.sectionDepartments.forEach(item => {
            this.afterDepartmentsResult.sectionDepartments.push(item);
          })

          this.departmentCodeClear();
          this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

          // すでに登録されている請求部門以外を取得する
          let departmentSearch = new DepartmentSearch();
          departmentSearch.withSectionId = this.sectionId;
          this.departmentService.GetItemsByDepartmentSearch(departmentSearch)
            .subscribe(response => {
              this.processResultService.processAtGetData(
                this.processCustomResult, response, false, this.partsResultMessageComponent);
              if (this.processCustomResult.result != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.departments = response;
              }
            });
        }
      });
  }

  public setCbxDepartment(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PB1201_DEPARTMENT;
    localstorageItem.value = this.cbxDepartmentCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "departmentNameFromCtrl", eventType);
  } 
  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////

  public setSectionCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.sectionCodeTrigger.closePanel();
    }

    this.sectionId = null;
    if (StringUtil.IsNullOrEmpty(this.sectionCodeCtrl.value)) {
      this.sectionNameCtrl.setValue("");

    } else {
      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.sectionId = response[0].id;
            this.sectionCodeCtrl.setValue(response[0].code);
            this.sectionNameCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeFromCtrl', eventType);

            // 登録済みの請求部門コード
            this.getSectionWithDepartment();
          }
          else {
            //this.sectionCodeCtrl.setValue("");
            this.sectionNameCtrl.setValue("");
          }
        });
    }
  }


  public setDepartmentCode(eventType: string, type: string = 'from') {
    let formControlCode: FormControl;
    let formControlName: FormControl;
    let nextFormName: string;

    if (type == 'from') {
      if (eventType != EVENT_TYPE.BLUR) {
        this.departmentCodeFromTrigger.closePanel();
      }
      this.departmentCodeFromId = null;
      formControlCode = this.departmentCodeFromCtrl
      formControlName = this.departmentNameFromCtrl
      nextFormName = 'departmentCodeToCtrl';
    } else {
      if (eventType != EVENT_TYPE.BLUR) {
        this.departmentCodeToTrigger.closePanel();
      }
      this.departmentCodeToId = null;
      formControlCode = this.departmentCodeToCtrl
      formControlName = this.departmentNameToCtrl
      nextFormName = 'departmentCodeFromCtrl';
    }

    if (StringUtil.IsNullOrEmpty(formControlCode.value)) {
      formControlCode.setValue("");
      formControlName.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);

    } else {
      let departmentSearch = new DepartmentSearch();
      departmentSearch.codes = [formControlCode.value];
      departmentSearch.withSectionId = this.sectionId;
      this.loadStart();
      this.departmentService.GetItemsByDepartmentSearch(departmentSearch)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            if (type == 'from') {
              this.departmentCodeFromId = response[0].id;

              if (this.cbxDepartmentCtrl.value) {
                this.departmentCodeToId = response[0].id;
                this.departmentCodeToCtrl.setValue(response[0].code);
                this.departmentNameToCtrl.setValue(response[0].name);
              }
            } else {
              this.departmentCodeToId = response[0].id;
            }
            formControlCode.setValue(response[0].code);
            formControlName.setValue(response[0].name);
          } else {
            // formControlCode.setValue("");
            formControlName.setValue("");
          }
          HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);
          this.setAddButton();
        });
    }
    this.setAddButton();
  }

}
