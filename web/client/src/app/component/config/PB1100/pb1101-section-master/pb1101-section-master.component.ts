
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { forkJoin } from 'rxjs';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { SectionsResult } from 'src/app/model/sections-result.model';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { Section } from 'src/app/model/section.model';
import { BankAccountMasterService } from 'src/app/service/Master/bank-account-master.service';
import { ReceiptService } from 'src/app/service/receipt.service';
import { SectionWithDepartmentMasterService } from 'src/app/service/Master/section-with-department-master.service';
import { SectionWithLoginUserMasterService } from 'src/app/service/Master/section-with-login-user-master.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb1101-section-master',
  templateUrl: './pb1101-section-master.component.html',
  styleUrls: ['./pb1101-section-master.component.css']
})
export class Pb1101SectionMasterComponent extends BaseComponent implements OnInit  {

  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public sectionsResult: SectionsResult;

  public sectionCodeCtrl: FormControl;  // 入金部門コード
  public sectionNameCtrl: FormControl;

  public noteCtrl: FormControl;  // 備考

  public payerCodeLeftCtrl: FormControl; // Tel
  public payerCodeRightCtrl: FormControl; // Fax  

  public undefineCtrl: FormControl;

  public selectIndex: number;

  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public sectionService: SectionMasterService,
    public processResultService: ProcessResultService,
    public bankAccountService: BankAccountMasterService,
    public receiptService: ReceiptService,
    public sectionWithDepartmentService: SectionWithDepartmentMasterService,
    public sectionWithLoginUserService: SectionWithLoginUserMasterService

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
        
    this.getSectionData();
  }

  public setControlInit() {
    this.sectionCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    this.sectionNameCtrl = new FormControl("", [Validators.required]);

    this.noteCtrl = new FormControl("");

    this.payerCodeLeftCtrl = new FormControl("");
    this.payerCodeRightCtrl = new FormControl("");

    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      sectionCodeCtrl: this.sectionCodeCtrl,
      sectionNameCtrl: this.sectionNameCtrl,

      noteCtrl: this.noteCtrl,

      payerCodeLeftCtrl: this.payerCodeLeftCtrl,
      payerCodeRightCtrl: this.payerCodeRightCtrl,

      undefineCtrl: this.undefineCtrl,

    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeCtrl);
    }

    FormatterUtil.setNumberFormatter(this.payerCodeLeftCtrl);
    FormatterUtil.setNumberFormatter(this.payerCodeRightCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();

    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.sectionCodeCtrl.enable();

    this.panelOpenState = false;
    this.panel.close();
  }

  /**
   * データ取得
   */
  public getSectionData() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    this.sectionsResult = new SectionsResult();
    this.sectionService.GetItems()
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.sectionsResult.sections = response;
          this.sectionsResult.sections.forEach(item => {
            item.payerCodeLeft = item.payerCode.substr(0, 3);
            item.payerCodeRight = item.payerCode.substr(3);
          });
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
    let registryData = new Section();
    let isRegistry: boolean = false;

    if (this.ComponentStatus === COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus === COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.sectionsResult.sections[this.selectIndex];

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    registryData.code = this.sectionCodeCtrl.value;
    registryData.name = this.sectionNameCtrl.value;
    registryData.note = this.noteCtrl.value;
    registryData.payerCodeLeft = this.payerCodeLeftCtrl.value;
    registryData.payerCodeRight = this.payerCodeRightCtrl.value;
    registryData.payerCode = this.payerCodeLeftCtrl.value + this.payerCodeRightCtrl.value;
    registryData = FileUtil.replaceNull(registryData);

    this.sectionService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getSectionData();
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
    let section = this.sectionsResult.sections[this.selectIndex];

    for (let i = 0; i < 4; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '入金部門コード';
    }

    let existBankAccountResponse = this.bankAccountService.ExistSection(section.id);
    existDeleteResultList[0].masterName = TABLES.MASTER_BANK_ACCOUNT.name;

    let existReceiptResponse = this.receiptService.ExistSection(section.id);
    existDeleteResultList[1].masterName = TABLES.RECEIPT.name;
    
    let existSectionWithDepartmentResponse = this.sectionWithDepartmentService.ExistSection(section.id);
    existDeleteResultList[2].masterName = TABLES.SECTION_WITH_DEPARTMENT.name;

    let existSectionWithLoginUserResponse = this.sectionWithLoginUserService.ExistSection(section.id);
    existDeleteResultList[3].masterName = TABLES.SECTION_WITH_LOGINUSER.name;

    // ３つの処理の待機
    forkJoin(
      existBankAccountResponse,
      existReceiptResponse,
      existSectionWithDepartmentResponse,
      existSectionWithLoginUserResponse
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

          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

            let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
            let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
            // processComponentRef.destroy();  

            this.sectionService.Delete(section)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.clear();
                  this.getSectionData();
                }

                processComponentRef.destroy();  

              });
          }
          componentRef.destroy();
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
    let dataList = this.sectionsResult.sections;
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

    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.SECTION_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];

      // CSV出力項目の設定
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].note);
      dataItem.push(dataList[index].payerCodeLeft);
      dataItem.push(dataList[index].payerCodeRight);
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
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_SECTION;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getSectionData();
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
    let section = this.sectionsResult.sections[index];

    this.sectionCodeCtrl.setValue(this.sectionsResult.sections[index].code);
    this.sectionNameCtrl.setValue(this.sectionsResult.sections[index].name);

    this.noteCtrl.setValue(this.sectionsResult.sections[index].note); 
    
    this.payerCodeLeftCtrl.setValue(this.sectionsResult.sections[index].payerCodeLeft);  
    this.payerCodeRightCtrl.setValue(this.sectionsResult.sections[index].payerCodeRight); 

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.sectionCodeCtrl.disable();
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setSectionCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.sectionCodeCtrl.value)) {
      let codeValue = StringUtil.setUpperCase(this.sectionCodeCtrl.value);
      this.sectionCodeCtrl.setValue(codeValue);
      let findIndex = this.sectionsResult.sections.findIndex((item) => {
        return (item.code === codeValue);
      });
      if (0 <= findIndex) {
        this.selectLine(findIndex);
      }
    }    
    HtmlUtil.nextFocusByName(this.elementRef, 'sectionNameCtrl', eventType);
  }

  public setSectionName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'noteCtrl', eventType);
  }

  public setNote(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'payerCodeLeftCtrl', eventType);
  }

  public setPayerCodeLeft(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'payerCodeRightCtrl', eventType);
  }

  public setPayerCodeRight(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
  }

}
