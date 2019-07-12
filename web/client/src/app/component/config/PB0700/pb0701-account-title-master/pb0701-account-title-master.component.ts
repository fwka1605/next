import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { AccountTitlesResult } from 'src/app/model/account-titles-result.model';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { AccountTitle } from 'src/app/model/account-title.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MODAL_STATUS_TYPE, COMPONENT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { forkJoin } from 'rxjs';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CustomerDiscountMasterService } from 'src/app/service/Master/customer-discount-master.service';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { BillingService } from 'src/app/service/billing.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { NavigationEnd, Router, ActivatedRoute } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb0701-account-title-master',
  templateUrl: './pb0701-account-title-master.component.html',
  styleUrls: ['./pb0701-account-title-master.component.css']
})
export class Pb0701AccountTitleMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public accountTitlesResult: AccountTitlesResult;

  public accountCodeCtrl: FormControl; // 科目
  public accountNameCtrl: FormControl;

  public contraAccountCodeCtrl: FormControl; // 相手科目
  public contraAccountNameCtrl: FormControl;

  public contraAccountSubCodeCtrl: FormControl; // 相手科目補助

  public selectIndex: number; // 選択された科目

  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public acountTitlesService: AccountTitleMasterService,
    public categoryService: CategoryMasterService,
    public customerDiscountService: CustomerDiscountMasterService,
    public billingService: BillingService,
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
    
    this.getAcountTitleData();
  }

  public setControlInit() {
    this.accountCodeCtrl = new FormControl('', [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.accountTitleCodeLength)]); // 科目
    this.accountNameCtrl = new FormControl('', [Validators.required]);

    this.contraAccountCodeCtrl = new FormControl(""); // 相手科目
    this.contraAccountNameCtrl = new FormControl("");

    this.contraAccountSubCodeCtrl = new FormControl(""); // 相手科目補助
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      accountCodeCtrl: this.accountCodeCtrl,  // 科目
      accountNameCtrl: this.accountNameCtrl,

      contraAccountCodeCtrl: this.contraAccountCodeCtrl,  // 相手科目
      contraAccountNameCtrl: this.contraAccountNameCtrl,

      contraAccountSubCodeCtrl: this.contraAccountSubCodeCtrl,  // 相手科目補助
    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.accountTitleCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.accountCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.accountCodeCtrl);
    }
    FormatterUtil.setCodeFormatter(this.contraAccountCodeCtrl);
    FormatterUtil.setCodeFormatter(this.contraAccountSubCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    // コード入力欄を有効化する
    this.accountCodeCtrl.enable();

    this.panelOpenState = false;
    this.panel.close();

  }

  /**
   * データ取得
   */
  public getAcountTitleData() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   
    
    this.acountTitlesService.Get()
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.accountTitlesResult = new AccountTitlesResult();
          this.accountTitlesResult.accountTitles = new Array<AccountTitle>();
          this.accountTitlesResult.accountTitles = result;
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
    let registryData = new AccountTitle();
    let isRegistry: boolean = false;

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.accountTitlesResult.accountTitles[this.selectIndex];

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   
        
    registryData.code = this.accountCodeCtrl.value;
    registryData.name = this.accountNameCtrl.value;
    registryData.contraAccountCode = this.contraAccountCodeCtrl.value;
    registryData.contraAccountName = this.contraAccountNameCtrl.value;
    registryData.contraAccountSubCode = this.contraAccountSubCodeCtrl.value;

    this.acountTitlesService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getAcountTitleData();
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

    let accountTitle = this.accountTitlesResult.accountTitles[this.selectIndex];
    let existDeleteResultList = new Array<ExistDeleteResult>();

    for (let i = 0; i < 3; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '科目コード';
    }

    let existCategoryAccountTitleResponse =
      this.categoryService.ExistAccountTitle(accountTitle.id)
    existDeleteResultList[0].masterName = TABLES.MASTER_CATEGORY.name;

    let existCustomerDiscountAccountTitleResponse =
      this.customerDiscountService.ExistAccountTitle(accountTitle.id);
    existDeleteResultList[1].masterName = TABLES.MASTER_CUSTOMER_DISCOUNT.name;

    let existBillingAccountTitleResponse =
      this.billingService.ExistAccountTitle(accountTitle.id);
    existDeleteResultList[2].masterName = TABLES.BILLING.name;

    // ３つの処理の待機
    forkJoin(
      existCategoryAccountTitleResponse,
      existCustomerDiscountAccountTitleResponse,
      existBillingAccountTitleResponse
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


            this.acountTitlesService.Delete(accountTitle)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.clear();
                  this.getAcountTitleData();
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
   * 選択したデータフォームに表示する
   * @param index 行番号
   */
  public selectLine(index: number) {

    this.panelOpenState = true;
    this.panel.open();

    this.selectIndex = index;
    this.accountCodeCtrl.setValue(this.accountTitlesResult.accountTitles[index].code); // 科目
    this.accountNameCtrl.setValue(this.accountTitlesResult.accountTitles[index].name);

    this.contraAccountCodeCtrl.setValue(this.accountTitlesResult.accountTitles[index].contraAccountCode); // 相手科目
    this.contraAccountNameCtrl.setValue(this.accountTitlesResult.accountTitles[index].contraAccountName);

    this.contraAccountSubCodeCtrl.setValue(this.accountTitlesResult.accountTitles[index].contraAccountSubCode); // 相手科目補助

    // ステータス更新
    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // コードを変更不可にする。
    this.accountCodeCtrl.disable();

  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_ACCOUNT_TITLE;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getAcountTitleData();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {
    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.ACCOUNT_TITLE_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;
    let dataList = this.accountTitlesResult.accountTitles;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].contraAccountCode);
      dataItem.push(dataList[index].contraAccountName);
      dataItem.push(dataList[index].contraAccountSubCode);
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
    this.openOptions();
    
    processComponentRef.destroy();   
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setAccountCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.accountCodeCtrl.value)) {
      let codeValue = StringUtil.setUpperCase(this.accountCodeCtrl.value);
      this.accountCodeCtrl.setValue(codeValue);
      let findIndex = this.accountTitlesResult.accountTitles.findIndex((item) => {
        return (item.code === codeValue);
      });
      if (0 <= findIndex) {
        this.selectLine(findIndex);
      }
    }    
    HtmlUtil.nextFocusByName(this.elementRef, 'accountNameCtrl', eventType);
  }

  public setAccountName(eventType: string) {  
    HtmlUtil.nextFocusByName(this.elementRef, 'contraAccountCodeCtrl', eventType);
  }

  public setContraAccountCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.contraAccountCodeCtrl.value)) {
      this.contraAccountCodeCtrl.setValue(StringUtil.setUpperCase(this.contraAccountCodeCtrl.value));
    }      
    HtmlUtil.nextFocusByName(this.elementRef, 'contraAccountNameCtrl', eventType);
  }

  public setContraAccountName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'contraAccountSubCodeCtrl', eventType);
  }

  public setContraAccountSubCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.contraAccountSubCodeCtrl.value)) {
      this.contraAccountSubCodeCtrl.setValue(StringUtil.setUpperCase(this.contraAccountSubCodeCtrl.value));
    }        
    HtmlUtil.nextFocusByName(this.elementRef, 'accountCodeCtrl', eventType);
  }

}
