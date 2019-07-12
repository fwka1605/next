import { Component, OnInit, ViewChild, ViewContainerRef, ComponentFactoryResolver, ElementRef, AfterViewInit, ComponentRef } from '@angular/core';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { DateUtil } from 'src/app/common/util/date-util';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { CODE_TYPE, CategoryType } from 'src/app/common/const/kbn.const';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { MatAutocompleteTrigger } from '@angular/material';
import { DatePipe } from '@angular/common';
import { forkJoin, Subject } from 'rxjs';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { Category } from 'src/app/model/category.model'
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { SectionsResult } from 'src/app/model/sections-result.model';
import { MfAggregateWebApiClient } from 'src/app/model/mf-aggr/mf-aggregate-web-api-client';
import { MfAggregationImporter, MF_AGGR_IMPORT_RESULT } from 'src/app/model/mf-aggr/mf-aggregate-importer';
import { ComponentId } from 'src/app/common/const/component-name.const';

const ymd = 'yyyy-MM-dd';

@Component({
  selector: 'app-pd1301-mf-aggr-data-extract',
  templateUrl: './pd1301-mf-aggr-data-extract.component.html',
  styleUrls: ['./pd1301-mf-aggr-data-extract.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]  
})
export class Pd1301MfAggrDataExtractComponent extends BaseComponent implements OnInit,AfterViewInit {

  public panelOpenState: boolean;

  public MF_AGGR_IMPORT_RESULT: typeof MF_AGGR_IMPORT_RESULT = MF_AGGR_IMPORT_RESULT;

  public modalRouterProgressComponentRef: ComponentRef<ModalRouterProgressComponent>;

  public sectionsResult: SectionsResult;
  public receiptCategoryResult: CategoriesResult;

  public receiptCategoryId: number;
  public selectedSectionId: number | null;

  public connectionStatusCtrl:FormControl;    // 連携状態

  public receiptCategoryCodeCtrl:FormControl; // 入金区分
  public receiptCategoryNameCtrl:FormControl;

  public sectionCodeCtrl:FormControl;         // 入金部門
  public sectionNameCtrl:FormControl;

  public mfLastAggregateAtCtrl:FormControl;   // 入金データ自動連携 最終連携日時
  public lastCreatedAtCtrl:FormControl;       // VONE登録データ 最終登録日時

  public extractDateFromCtrl: FormControl;    // 伝票日付
  public extractDateToCtrl: FormControl;

  public UndefineCtrl: FormControl;

  public extractCount: number = 123456;     // 抽出件数
  public extractInCount: number = 123456;   // 抽出入金件数
  public extractInAmount: number = 123456;  // 抽出入金額合計
  public extractOutCount: number = 123456;  // 抽出出金件数
  public extractOutAmount: number = 123456; // 抽出出金額合計

  public registryCount: number = 123456;    // 登録件数
  public registryAmount: number = 123456;   // 登録金額合計

  public registryBtnDisable: boolean = true;

  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public processResultService:ProcessResultService,
    public categoryService:CategoryMasterService,
    public sectionService: SectionMasterService,
    public client: MfAggregateWebApiClient,
    public datePipe: DatePipe,
    public importer: MfAggregationImporter,

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

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let receiptCategoryResponse = this.categoryService.GetItems(CategoryType.Receipt);
    let sectionMasterResponse = this.sectionService.GetItems();
    let importerResponse = this.importer.initializeVOneMaster();

    forkJoin(
      receiptCategoryResponse,
      sectionMasterResponse,
      importerResponse
    )
      .subscribe(responseList => {
        if (responseList.length != 3
        || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE
        || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE
        || responseList[2] == false) {
          this.modalRouterProgressComponentRef.destroy();
        }
        else {
          this.receiptCategoryResult = new CategoriesResult();
          this.receiptCategoryResult.categories = responseList[0];

          this.sectionsResult = new SectionsResult();
          this.sectionsResult.sections = responseList[1];

          this.initializeControlValues();

        }
      });

  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', EVENT_TYPE.NONE);
  }
  
  public setControlInit() {

    this.connectionStatusCtrl = new FormControl("");  // 連携状態

    this.receiptCategoryCodeCtrl = new FormControl("", [Validators.maxLength(2)]);  // 入金区分
    this.receiptCategoryNameCtrl = new FormControl("");  

    this.sectionCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]); // 入金部門コード
    this.sectionNameCtrl = new FormControl("");

    this.mfLastAggregateAtCtrl = new FormControl("");   // 入金データ自動連携 最終連携日時
    this.lastCreatedAtCtrl = new FormControl("");       // VONE登録データ 最終登録日時    

    this.extractDateFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 伝票日付
    this.extractDateToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.UndefineCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      connectionStatusCtrl:this.connectionStatusCtrl,         // 連携状態
      receiptCategoryCodeCtrl: this.receiptCategoryCodeCtrl,  // 入金区分
      receiptCategoryNameCtrl: this.receiptCategoryNameCtrl,
      sectionCodeCtrl: this.sectionCodeCtrl,                  // 入金部門
      sectionNameCtrl: this.sectionNameCtrl,
      mfLastAggregateAtCtrl: this.mfLastAggregateAtCtrl,      // 入金データ自動連携 最終連携日時
      lastCreatedAtCtrl: this.lastCreatedAtCtrl,              // VONE登録データ 最終登録日時
      extractDateFromCtrl: this.extractDateFromCtrl,          // 伝票日付
      extractDateToCtrl: this.extractDateToCtrl,

      UndefineCtrl: this.UndefineCtrl,
    });

  }

  public setFormatter(){

    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeCtrl); // 入金区分

    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeCtrl); // 入金部門コード
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeCtrl);
    }

  }

  //////////////////////////////////////////////////////////////
  public clear() {
    
    this.MyFormGroup.reset();

    this.clearSummaryInfo();

    this.panelOpenState = true;

  }

  //////////////////////////////////////////////////////////////
  public clearSummaryInfo() {

    // 初期化処理
    this.extractCount=0;
    this.extractInCount=0;
    this.extractInAmount=0;
    this.extractOutCount=0;
    this.extractOutAmount=0;
    this.registryCount=0;
    this.registryAmount=0;

  }

  //////////////////////////////////////////////////////////////
  public displayAccessTokenStatus() {

    this.extractDateFromCtrl.reset();
    this.extractDateToCtrl.reset();

    this.validateAccessToken()
      .subscribe(response => {
        if (!response) return;
        if (!this.importer.IsAccessTokenExist) {
          this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.NOT_SETTING_MASTER.replace(MSG_ITEM_NUM.FIRST, '入金データ自動連携 WebAPI 連携設定'),
            this.partsResultMessageComponent);
        }
        else if (!this.importer.IsTokenValid) {
          this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.EXPIRED_TOKEN, this.partsResultMessageComponent);
        }
      });

  }

  //////////////////////////////////////////////////////////////
  public initializeControlValues() {

    this.initializeSubaccountRelatedValues();

    this.validateAccessToken()
      .subscribe(response => {
        if (!response) return;

        this.importer.loadMfMaster()
          .subscribe(response => {
            if (!response) {
              this.modalRouterProgressComponentRef.destroy();
              return;
            }

            let validateControlValues = !StringUtil.IsNullOrEmpty(this.receiptCategoryNameCtrl.value) &&
                                        (!(this.userInfoService.ApplicationControl.useReceiptSection == 1) || !StringUtil.IsNullOrEmpty(this.sectionNameCtrl.value));

            if (validateControlValues) {
              this.importer.saveMfAggrMaster()
                .subscribe(response => {
                  this.modalRouterProgressComponentRef.destroy();
                });
            }

            this.mfLastAggregateAtCtrl.setValue(this.importer.MfLastAggregatedAt);
            this.lastCreatedAtCtrl.setValue(this.importer.LastOneEntityCreatedAt);
            
          });

      });

  }

  //////////////////////////////////////////////////////////////
  public initializeSubaccountRelatedValues() {

    let subAccount = this.importer.FirstSubAccount;
    if (subAccount == undefined) return;

    this.importer.ReceiptCategoryId = subAccount.receiptCategoryId;
    this.importer.SectionId = subAccount.sectionId;
    const category = this.receiptCategoryResult.categories.find(x => x.id == subAccount.receiptCategoryId);
    if (category != undefined) {
      this.receiptCategoryCodeCtrl.setValue(category.code);
      this.receiptCategoryNameCtrl.setValue(category.name);
      this.receiptCategoryId = category.id;      
    }

    if (this.userInfoService.ApplicationControl.useReceiptSection == 1 &&
      subAccount != undefined && subAccount.sectionId != undefined)
    {
      const section = this.sectionsResult.sections.find(x => x.id == subAccount.sectionId);
      this.sectionCodeCtrl.setValue(section.code);
      this.sectionNameCtrl.setValue(section.name);
      this.selectedSectionId = section.id;
    }

  }

  //////////////////////////////////////////////////////////////
  public validateAccessToken(): Subject<boolean> {

    let validateAccessToken: Subject<boolean> = new Subject();

    this.importer.validateToken()
      .subscribe(response => {
        if (!response) {
          this.endSubProcess("validateAccessToken", false, validateAccessToken);
        }
        else {
          const result = this.importer.ImportResult;
          const status = result == MF_AGGR_IMPORT_RESULT.NotAuthorized ? "連携中" 
                        :result == MF_AGGR_IMPORT_RESULT.TokenExpired ? "トークン有効期限切れ" 
                        :result == MF_AGGR_IMPORT_RESULT.Success ? "連携中" : "";
      
          this.connectionStatusCtrl.setValue(status);
          this.registryBtnDisable = !(result == MF_AGGR_IMPORT_RESULT.Success);
          this.endSubProcess("validateAccessToken", true, validateAccessToken);
        }

      });

    validateAccessToken = new Subject();
    return validateAccessToken;
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

      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }  

  //////////////////////////////////////////////////////////////
  public registry() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    this.clearSummaryInfo();

    this.importer.ReceiptCategoryId = this.receiptCategoryId;

    let extractDateFrom: string = "";
    let extractDateTo: string = "";

    if (!StringUtil.IsNullOrEmpty(this.extractDateFromCtrl.value)) {
      extractDateFrom = this.datePipe.transform(DateUtil.ConvertFromDatepicker(this.extractDateFromCtrl), ymd);
    }

    if (!StringUtil.IsNullOrEmpty(this.extractDateToCtrl.value)) {
      extractDateTo = this.datePipe.transform(DateUtil.ConvertFromDatepicker(this.extractDateToCtrl), ymd);
    }

    this.importer.importMfAggrTransactions(extractDateFrom, extractDateTo)
      .subscribe(response => {
        if (response) {

          if (this.importer.ImportResult == MF_AGGR_IMPORT_RESULT.ApiFailure) {
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '入金データ自動連携 マスター情報の取得'),
              this.partsResultMessageComponent);
              this.modalRouterProgressComponentRef.destroy();
              return;
          }
          else if (this.importer.ImportResult == MF_AGGR_IMPORT_RESULT.SaveMasterFailure) {
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '入金データ自動連携 マスター情報の登録'),
              this.partsResultMessageComponent);
              this.modalRouterProgressComponentRef.destroy();
              return;
          }
          else if (this.importer.ImportResult == MF_AGGR_IMPORT_RESULT.DataNotExist) {
              this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);
                this.modalRouterProgressComponentRef.destroy();
                return;  
          }

          this.extractCount     = this.importer.ExtractCount;
          this.extractInCount   = this.importer.ExtractInCount;
          this.extractInAmount  = this.importer.ExtractInAmount;
          this.extractOutCount  = this.importer.ExtractOutCount;
          this.extractOutAmount = this.importer.ExtractOutAmount;

          if (this.importer.ImportResult == MF_AGGR_IMPORT_RESULT.SaveTransactionFailure) {
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
              this.modalRouterProgressComponentRef.destroy();
              return;
          }

          this.registryCount  = this.importer.RegistryCount;
          this.registryAmount = this.importer.RegistryAmount;

          this.mfLastAggregateAtCtrl.setValue(this.importer.MfLastAggregatedAt);
          this.lastCreatedAtCtrl.setValue(this.importer.LastOneEntityCreatedAt);

          this.processCustomResult = this.processResultService.processAtSave(
            this.processCustomResult, response, true, this.partsResultMessageComponent);

          this.modalRouterProgressComponentRef.destroy();
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
          this.modalRouterProgressComponentRef.destroy();
        }

      });

  }

  //////////////////////////////////////////////////////////////
  public openMasterModal(table: TABLE_INDEX  ) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY_EXCLUDE_USE_LIMIT_DATE:
            {
              this.receiptCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.receiptCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              this.receiptCategoryId = componentRef.instance.SelectedId;
              break;
            }
            case TABLE_INDEX.MASTER_SECTION:
            {
              this.sectionCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.sectionNameCtrl.setValue(componentRef.instance.SelectedName);
              this.selectedSectionId = componentRef.instance.SelectedId;
              break;
            }
        }
      }

      componentRef.destroy();
    });

  }  

  ///////////////////////////////////////////////////////////////////////
  public setReceiptCategoryCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.receiptCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeCtrl.value)) {

      this.receiptCategoryCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.receiptCategoryCodeCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.receiptCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            const category = response[0] as Category;
            if (category.useLimitDate == 0) {
              this.receiptCategoryCodeCtrl.setValue(category.code);
              this.receiptCategoryNameCtrl.setValue(category.name);
              this.receiptCategoryId = category.id;
            }
            else {
              this.receiptCategoryNameCtrl.setValue("");
              this.receiptCategoryId = 0;
            }
          }
          else {
            //this.receiptCategoryCodeCtrl.setValue("");
            this.receiptCategoryNameCtrl.setValue("");
            this.receiptCategoryId = 0;
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
        });

    }
    else {
      this.receiptCategoryCodeCtrl.setValue("");
      this.receiptCategoryNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
    }

  }  

  //////////////////////////////////////////////////////////////
  public setSectionCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.sectionCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeCtrl.value)) {

      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            this.sectionCodeCtrl.setValue(response[0].code);
            this.sectionNameCtrl.setValue(response[0].name);
            this.selectedSectionId = response[0].id;
          }
          else {
            this.sectionNameCtrl.setValue("");
            this.selectedSectionId = 0;
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'extractDateFromCtrl', eventType);
        });

    }
    else {
      this.sectionCodeCtrl.setValue("");
      this.sectionNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'extractDateFromCtrl', eventType);
    }

  }

  //////////////////////////////////////////////////////////////
  public setExtractDateFrom(eventType: string){
    HtmlUtil.nextFocusByName(this.elementRef, 'extractDateToCtrl', eventType);
  }

  //////////////////////////////////////////////////////////////
  public setExtractDateTo(eventType: string){
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', eventType);
  }

  //////////////////////////////////////////////////////////////
  public synchronize() {

    const authEndpointUri = this.client.AuthorizationEndpointUri;
    const url = `${authEndpointUri}/bank`
    window.open(url);
  }

  //////////////////////////////////////////////////////////////
  public navigateSetting() {
    this.router.navigate(['main/PH1801', { from: ComponentId.PD1301 }]);
  }

  //////////////////////////////////////////////////////////////
  public navigateDataSearch() {
    this.router.navigate(['main/PH2001', { from: ComponentId.PD1301 }]);
  }  

  //////////////////////////////////////////////////////////////
  public endSubProcess(processName:string,endFlag:boolean,subject:Subject<boolean>){
    //console.log("endProcess:processName=" + processName + ",endFlag=" + endFlag);
    //this.modalRouterProgressComponentRef.destroy();
    subject.next(endFlag);
    subject.complete();
    if (!endFlag) {
      this.modalRouterProgressComponentRef.destroy();
    }
  }

  //////////////////////////////////////////////////////////////
  public endSubProcessAny(processName:string,endObj:any,subject:Subject<any>){
    //console.log("endProcess:processName=" + processName + ",endFlag=" + endObj);
    this.modalRouterProgressComponentRef.destroy();
    subject.next(endObj);
    subject.complete();
    if (endObj == null) {
      this.modalRouterProgressComponentRef.destroy();
    }
  }  

}

