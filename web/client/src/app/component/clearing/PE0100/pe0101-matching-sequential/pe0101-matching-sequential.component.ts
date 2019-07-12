import { ComponentRef, Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit, ViewChild } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, COMPONENT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { StringUtil } from 'src/app/common/util/string-util';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { ModalMultiMasterComponent } from 'src/app/component/modal/modal-multi-master/modal-multi-master.component';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter, NgbCalendar, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { DBService } from 'src/app/service/db.service';
import { Section } from 'src/app/model/section.model';
import { MatchingOrder } from 'src/app/model/matching-order.model';
import { CollationSearch } from 'src/app/model/collation-search.model';
import { GeneralSettingUtil } from 'src/app/common/util/general-setting-util';
import { forkJoin } from 'rxjs';
import { MatchingUtil } from 'src/app/common/util/matching-util';
import { CategoryType, SHARE_TRANSFER_FEE_DICTIONARY, CheckBoxStatus, FunctionType } from 'src/app/common/const/kbn.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { CollationData } from 'src/app/model/collation/collation-data.model';
import { CustomerFeeMasterService } from 'src/app/service/Master/customer-fee-master.service';
import { PaymentAgencyFeeMasterService } from 'src/app/service/Master/payment-agency-fee-master.service';
import { MatchingService } from 'src/app/service/matching.service';
import { CustomerFeeSearch } from 'src/app/model/customer-fee-search.model';
import { PaymentAgencyFeeSearch } from 'src/app/model/payment-agency-fee-search.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { ModalCustomerDetailComponent } from 'src/app/component/modal/modal-customer-detail/modal-customer-detail.component';
import { MatchingHeader } from 'src/app/model/matching-header.model';
import { ModalMatchingRecordedAtComponent } from 'src/app/component/modal/modal-matching-recorded-at/modal-matching-recorded-at.component';
import { MatchingSequentialSource } from 'src/app/model/matching-sequential-source.model';
import { MatchingResult } from 'src/app/model/matching-result.model';
import { MatchingErrorType } from 'src/app/common/const/matching.const';
import { MatchingCancelSource } from 'src/app/model/matching-cancel-source.model';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ModalCollatePrintComponent } from 'src/app/component/modal/modal-collate-print/modal-collate-print.component';
import { CollationInfo } from 'src/app/model/collation/collation-info';
import { SortItem, eSortType, SortOrder } from 'src/app/model/collation/sort-item';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { Department } from 'src/app/model/department.model';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { SortUtil } from 'src/app/common/util/sort-util';
import { MatchingSequentialReportSource } from 'src/app/model/matching-sequential-report-source.model';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_INF, MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { PageUtil } from 'src/app/common/util/page-util';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { GridSizeKey } from 'src/app/common/const/local-storage-key-const';
import { MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pe0101-matching-sequential',
  templateUrl: './pe0101-matching-sequential.component.html',
  styleUrls: ['./pe0101-matching-sequential.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pe0101MatchingSequentialComponent extends BaseComponent implements OnInit,AfterViewInit {

  public readonly SHARE_TRANSFER_FEE_DICTIONARY: typeof SHARE_TRANSFER_FEE_DICTIONARY = SHARE_TRANSFER_FEE_DICTIONARY;
  public readonly eSortType: typeof eSortType = eSortType;
  public readonly CheckBoxStatus: typeof CheckBoxStatus = CheckBoxStatus;
  public readonly SortOrder: typeof SortOrder = SortOrder;

  public readonly CategoryType: typeof CategoryType = CategoryType;

  public panelOpenState: boolean;
  public loadingData = true;

  public collationInfo: CollationInfo = new CollationInfo();

  public departments: Department[];
  public departmentsWithLoginUser: Department[];
  public sections: Section[];
  public sectionsWithLoginUser: Section[];


  
  /*
    画面入力項目フォーム
  */

  public cbxCheckAllCtrl:FormControl; // 全選択・全解除

  public receiptRecordedAtFromCtrl: FormControl;  // 入金日
  public receiptRecordedAtToCtrl: FormControl;

  public billingDueAtFromCtrl: FormControl;  // 入金予定日
  public billingDueAtToCtrl: FormControl;

  public currencyCodeCtrl: FormControl; // 通貨コード

  public departmentNameCtrl: FormControl; // 請求部門

  public sectionNameCtrl: FormControl; // 入金部門

  public cmbBillingDataCtrl: FormControl; // 請求データ

  public rdoDisplayTargetAmountCtrl: FormControl; // 表示金額

  public cbxShowMatchedCtrl: FormControl; // 消込完了データを表示

  public createAtFromCtrl: FormControl;  // 消込日
  public createAtToCtrl: FormControl;

  public searchKeyCtrl: FormControl;  // 検索

  public cbxDetailKesiCtrls: Array<FormControl>;

  //////////////////////////////////////////////////////////


  //////////////////////////////////////////////////////////
  // モーダルコンポーネントの参照
  //    複数のメソッドに処理が渡るため。
  //////////////////////////////////////////////////////////
  public confirmComponentRef: ComponentRef<any>;

  public sortItems: Array<string>;
  
  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public userInfoService: UserInfoService,
    public currencyService: CurrencyMasterService,
    public customerFeeService: CustomerFeeMasterService,
    public paymentAgencyFeeService: PaymentAgencyFeeMasterService,
    public collationSettingService: CollationSettingMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public dbService: DBService,
    public calendar: NgbCalendar,
    public matchingService: MatchingService,
    public departmentService: DepartmentMasterService,
    public sectionService: SectionMasterService,
    public processResultService: ProcessResultService,
    public customerMasterService: CustomerMasterService,
    public localStorageManageService:LocalStorageManageService,
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title,true);
      }
    });
  }

  ngOnInit() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);


    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      // ルートパラメータと同じ要領で Matrix URI パラメータが取得できる
      let param = params.get("process");

      if (!StringUtil.IsNullOrEmpty(param) && param == "back") {
        // 検索条件の詰め戻し
        this.MyFormGroup = this.matchingService.Pe0101myFormGroup;
        this.collationInfo = this.matchingService.collationInfo;

        this.readControlInit();
      }
      else {

        /*
          管理マスタの値
        */
        /// <summary>入金予定日 算出用 現在日付 + N 日の値 管理マスター：回収予定範囲 の値</summary>
        this.collationInfo.billingDueAtOffset = parseInt(GeneralSettingUtil.getGeneralSettingByCode(this.userInfoService.GeneralSettings, "回収予定範囲").value);
        /// <summary>手数料誤差範囲 通貨 JPY 日本円の場合に 管理マスター：手数料誤差 の値を利用</summary>
        this.collationInfo.bankFeeTolerance = parseInt(GeneralSettingUtil.getGeneralSettingByCode(this.userInfoService.GeneralSettings, "手数料誤差").value);
        /// <summary>消費税誤差 一括消込時の許容誤差。入金額±誤差まで消込可</summary>
        this.collationInfo.taxDifferenceTolerance = parseInt(GeneralSettingUtil.getGeneralSettingByCode(this.userInfoService.GeneralSettings, "消費税誤差").value);

        this.setControlInit();
        this.setValidator();
        this.setFormatter();
        this.clear();

      }

    });


    let collationSettingResponse = this.collationSettingService.Get();
    let juridicalPersonalityResponse = this.juridicalPersonalityService.GetItems();

    let billingOrderResponse = this.collationSettingService.GetMatchingBillingOrder();
    let receiptOrderResponse = this.collationSettingService.GetMatchingReceiptOrder();

    let dbResponse = this.dbService.GetClientKey();

    let customerFeeSearch = new CustomerFeeSearch();
    customerFeeSearch.companyId = this.userInfoService.Company.id;
    let customerFeeResponse = this.customerFeeService.Get(customerFeeSearch);

    let paymentAgencyFeeSearch = new PaymentAgencyFeeSearch();
    paymentAgencyFeeSearch.companyId = this.userInfoService.Company.id;
    let paymentAgencyFeeResponse = this.paymentAgencyFeeService.GetItems(paymentAgencyFeeSearch);

    forkJoin(
      collationSettingResponse,
      juridicalPersonalityResponse,
      billingOrderResponse,
      receiptOrderResponse,
      dbResponse,
      customerFeeResponse,
      paymentAgencyFeeResponse
    )
      .subscribe(responseList => {
        if (responseList != undefined && responseList.length == 7) {
          this.collationInfo.collationSetting = responseList[0];
          this.collationInfo.legalPersonalities = responseList[1];
          this.collationInfo.matchingBillingOrders = responseList[2];
          this.collationInfo.matchingReceiptOrders = responseList[3];
          this.collationInfo.clientKey = responseList[4];
          this.collationInfo.customerFees = responseList[5];
          this.collationInfo.paymentAgencyFees = responseList[6];

          this.loadingData = false;
        }
        modalRouterProgressComponentRef.destroy();
    
      });

    /////////////////////////////////////////
    // 請求・入金部門の検索結果の初期化
    /////////////////////////////////////////
    this.departmentService.GetItems().subscribe(response => {
      if (response != undefined) {
        this.departments = response;
        this.collationInfo.departments = this.departments;
        this.departmentService.GetItems("", this.userInfoService.LoginUser.id).subscribe(response => {
          if (response != undefined) {
            this.departmentsWithLoginUser = response;
            this.collationInfo.departmentsWithLoginUser = this.departmentsWithLoginUser;
            this.initializeDepartmentSelection();
          }
        });

      }
    });

    this.sectionService.GetItems().subscribe(response => {
      if (response != undefined) {
        this.sections = response;
        this.collationInfo.sections = this.sections;
        this.sectionService.GetItems("", this.userInfoService.LoginUser.id).subscribe(response => {
          if (response != undefined) {
            this.sectionsWithLoginUser = response;
            this.initializeSectionSelection();
          }
        });
      }
    });

  }

  ngAfterViewInit(){

    HtmlUtil.nextFocusByNames(this.elementRef,
      ['receiptRecordedAtFromCtrl','receiptRecordedAtToCtrl'], EVENT_TYPE.NONE);   
  }

  public setControlInit() {

    this.cbxCheckAllCtrl = new FormControl(""); // 全選択・全解除

    this.receiptRecordedAtFromCtrl = new FormControl(null, [Validators.maxLength(10)]);  // 入金日
    this.receiptRecordedAtToCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);

    this.billingDueAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 入金予定日
    this.billingDueAtToCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);

    this.currencyCodeCtrl = new FormControl("", [Validators.maxLength(3),]);  // 通貨コード

    this.departmentNameCtrl = new FormControl("");  // 請求部門

    this.sectionNameCtrl = new FormControl("");  // 入金部門

    this.cmbBillingDataCtrl = new FormControl("");  // 請求データ

    this.rdoDisplayTargetAmountCtrl = new FormControl("");  // 表示金額

    this.cbxShowMatchedCtrl = new FormControl("");  // 消込完了データを表示

    this.createAtFromCtrl = new FormControl("", [Validators.maxLength(10)]);  // 　消込日
    this.createAtToCtrl = new FormControl("", [Validators.maxLength(10)]);

    this.searchKeyCtrl = new FormControl(""); // 検索
  }

  public readControlInit() {

    this.cbxCheckAllCtrl = <FormControl>this.MyFormGroup.controls['cbxCheckAllCtrl']; // 全選択・全解除


    this.receiptRecordedAtFromCtrl = <FormControl>this.MyFormGroup.controls['receiptRecordedAtFromCtrl'];  // 入金日
    this.receiptRecordedAtToCtrl = <FormControl>this.MyFormGroup.controls['receiptRecordedAtToCtrl'];

    this.billingDueAtFromCtrl = <FormControl>this.MyFormGroup.controls['billingDueAtFromCtrl'];  // 入金予定日
    this.billingDueAtToCtrl = <FormControl>this.MyFormGroup.controls['billingDueAtToCtrl'];

    this.currencyCodeCtrl = <FormControl>this.MyFormGroup.controls['currencyCodeCtrl'];  // 通貨コード

    this.departmentNameCtrl = <FormControl>this.MyFormGroup.controls['departmentNameCtrl'];  // 請求部門

    this.sectionNameCtrl = <FormControl>this.MyFormGroup.controls['sectionNameCtrl'];  // 入金部門

    this.cmbBillingDataCtrl = <FormControl>this.MyFormGroup.controls['cmbBillingDataCtrl'];  // 請求データ

    this.rdoDisplayTargetAmountCtrl = <FormControl>this.MyFormGroup.controls['rdoDisplayTargetAmountCtrl'];  // 表示金額

    this.cbxShowMatchedCtrl = <FormControl>this.MyFormGroup.controls['cbxShowMatchedCtrl'];  // 消込完了データを表示

    this.createAtFromCtrl = <FormControl>this.MyFormGroup.controls['createAtFromCtrl'];  // 　消込日
    this.createAtToCtrl = <FormControl>this.MyFormGroup.controls['createAtToCtrl'];

    this.searchKeyCtrl = <FormControl>this.MyFormGroup.controls['searchKeyCtrl']; // 検索    
  }

  public setValidator() {



    this.MyFormGroup = new FormGroup({

      cbxCheckAllCtrl: this.cbxCheckAllCtrl,  // 全選択・全解除

      receiptRecordedAtFromCtrl: this.receiptRecordedAtFromCtrl,   // 入金日
      receiptRecordedAtToCtrl: this.receiptRecordedAtToCtrl,

      billingDueAtFromCtrl: this.billingDueAtFromCtrl,   // 入金予定日
      billingDueAtToCtrl: this.billingDueAtToCtrl,

      currencyCodeCtrl: this.currencyCodeCtrl, // 通貨コード

      departmentNameCtrl: this.departmentNameCtrl, // 請求部門

      sectionNameCtrl: this.sectionNameCtrl, // 入金部門

      cmbBillingDataCtrl: this.cmbBillingDataCtrl, // 請求データ

      rdoDisplayTargetAmountCtrl: this.rdoDisplayTargetAmountCtrl, // 表示金額

      cbxShowMatchedCtrl: this.cbxShowMatchedCtrl, // 消込完了データを表示

      createAtFromCtrl: this.createAtFromCtrl,
      createAtToCtrl: this.createAtToCtrl,

      searchKeyCtrl: this.searchKeyCtrl, // 検索

    });
  }

  public setFormatter() {
    // 通貨コード
    FormatterUtil.setAlphabetFormatter(this.currencyCodeCtrl);

  }



  public clear() {
    this.MyFormGroup.reset();
    this.panelOpenState = true;
    this.panel.open();

    /////////////////////////////////////////
    // エラーメッセージのクリア
    /////////////////////////////////////////
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.cbxCheckAllCtrl.setValue(null);

    /////////////////////////////////////////
    // 検索条件の初期化
    /////////////////////////////////////////
    this.cbxShowMatchedCtrl.setValue(false);
    this.createAtFromCtrl.setValue(null);
    this.createAtToCtrl.setValue(null);
    this.createAtFromCtrl.disable();
    this.createAtToCtrl.disable();
    this.cmbBillingDataCtrl.setValue(0);

    let today = this.calendar.getToday();

    // 入金日（終了日） 現在日時
    this.receiptRecordedAtToCtrl.setValue(today);

    // 入金予定日（終了日）現在日時＋billingDueAtOffset（管理マスタの回収予定範囲）
    this.billingDueAtToCtrl.setValue(this.calendar.getNext(today, 'd', this.collationInfo.billingDueAtOffset));

    // 表示金額
    this.rdoDisplayTargetAmountCtrl.setValue("0");

    /*
    /////////////////////////////////////////
    // ソート条件の初期化
    /////////////////////////////////////////
    // 請求・入金ソート情報件を請求にする。
    this.collationInfo.billingPriority = false;
    this.collationInfo.sortType = eSortType.None;

    /////////////////////////////////////////
    // 検索結果の初期化
    /////////////////////////////////////////
    this.collationInfo.collations = null;
    */

    /////////////////////////////////////////
    // ボタン名の変更
    /////////////////////////////////////////
    this.collationInfo.collateButtonName = "照合";
    this.collationInfo.matchingButtonName = "一括消込";
    this.collationInfo.resutlTitle = "照合結果";
    this.collationInfo.selectionAlias = '一括';
    this.collationInfo.noteAlias = '前受';

    /////////////////////////////////////////
    // 消込完了データを表示のチェックボックスを初期化
    //  照合・表示に無効にしているため。
    /////////////////////////////////////////
    this.cbxShowMatchedCtrl.enable();

    this.initializeDepartmentSelection();
    this.initializeSectionSelection();

    HtmlUtil.nextFocusByNames(this.elementRef,
      ['receiptRecordedAtFromCtrl','receiptRecordedAtToCtrl'], EVENT_TYPE.NONE);    
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

      case BUTTON_ACTION.COLLATE:
        this.collate();
        break;
      case BUTTON_ACTION.MATCHING:
        this.matching();
        break;
      case BUTTON_ACTION.EXPORT:
        this.export();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }

  }


  /////////////////////////////////////////////////////////////////////////////
  public collate() {

    // エラーメッセージのクリア
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.cbxShowMatchedCtrl.disable();

    if (this.cbxShowMatchedCtrl.value) {
      this.SearchMatchingHeaders();
    }
    else {
      this.collateInner();
    }

  }

  ///////////////////////////////////////////////
  // 表示 （入力チェック）
  //////////////////////////////////////////////////////
  public SearchMatchingHeaders() {
    this.collationInfo.resutlTitle = "消込結果";

    if (!this.validateInputValues()) {

      this.processResultService.processAtWarning(
        this.processCustomResult, "検索条件にエラーがあります。", this.partsResultMessageComponent);
      return false;
    }

    var option = (this.collationInfo.collationSearch = this.GetCollationOption());
    this.collationInfo.collations = null;

    this.SetMatchingHeadersAsync(option);
  }

  ///////////////////////////////////////////////
  // 表示  部署情報の設定
  //////////////////////////////////////////////////////
  public SetMatchingHeadersAsync(option: CollationSearch) {

    let responses: any[] = new Array<any>();

    let confirmComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.confirmComponentRef = this.viewContainerRef.createComponent(confirmComponentFactory);

    if (option.useDepartmentWork) {
      responses.push(
        this.matchingService.SaveWorkDepartmentTarget(this.collationInfo.clientKey, this.collationInfo.departmentIds));
    }
    if (option.useSectionWork) {
      responses.push(
        this.matchingService.SaveWorkSectionTarget(this.collationInfo.clientKey, this.collationInfo.sectionIds));
    }

    if (responses.length != 0) {
      forkJoin(responses)
        .subscribe(responseList => {
          if (responseList != undefined && responseList.length == responses.length) {
            this.GetMatchingHeadersAsync(option);
          }
          else {
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '表示'),
              this.partsResultMessageComponent);
          }
        });
    }
    else {
      this.GetMatchingHeadersAsync(option);

    }
  }

  ///////////////////////////////////////////////
  // 表示  実際の検索
  //////////////////////////////////////////////////////
  public GetMatchingHeadersAsync(option: CollationSearch) {
    this.matchingService.SearchMatchedData(option)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          if (response.length > 0) {
            this.panelOpenState = false;

            this.processResultService.processAtGetData(this.processCustomResult, response, true, this.partsResultMessageComponent);

            this.collationInfo.matchingHeaders = response;
            this.setCollationsDataFromMatchedData(this.collationInfo.matchingHeaders);
            //this.InitializeCheckBoxForm(); 
            // チェックの可否を設定する。
            this.InitializeGridCheckBoxEnabled();
            this.SortGridData(eSortType.None);


          }
          else {

            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);

            this.panelOpenState = true;
          }
        }
        else {
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '表示'),
            this.partsResultMessageComponent);

          this.panelOpenState = true;

        }

        this.confirmComponentRef.destroy();
      });
  }


  // 照合内部 検索結果を加工するためにCollationクラスに詰め替えている。（Angular固有の処理）
  public setCollationsDataFromMatchedData(tmpCollations: Array<MatchingHeader>) {
    this.collationInfo.collations = new Array<CollationData>();
    for (var i = 0; i < tmpCollations.length; i++) {
      var collationData = new CollationData();
      collationData.setDataFromMatchingHeader(tmpCollations[i]);
      this.collationInfo.collations.push(collationData);
    }

  }

  /*

  // 照合内部 検索結果の一括照合のチェックボックスの値の設定
  public InitializeCheckBoxForm(){

    for (var i = 0; i < this.collationInfo.collations.length; i++)
    {
      this.collationInfo.collations[i].checkable=true;
      this.collationInfo.collations[i].checkBox=CheckBoxStatus.OFF;
      this.collationInfo.collations[i].dupeCheck=0;
    }


  }  
  */

  ///////////////////////////////////////////////
  // 照合 入力チェック
  ///////////////////////////////////////////////
  public collateInner(): boolean {
    this.collationInfo.resutlTitle = "照合結果";

    if (!this.validateInputValues()) {

      this.processResultService.processAtWarning(
        this.processCustomResult, "検索条件にエラーがあります。", this.partsResultMessageComponent);
      return false;
    }

    var option = (this.collationInfo.collationSearch = this.GetCollationOption());
    this.collationInfo.collations = null;

    /*
      不要？
    var collationOrders;
    this.collationSettingService.GetCollationOrder()
    .subscribe(response=>{
      collationOrders = response[0];
    });
    */

    this.GetCollationsAsync(option);

  }

  ///////////////////////////////////////////////
  // 照合 部署情報の設定
  ///////////////////////////////////////////////
  public GetCollationsAsync(option: CollationSearch) {

    let responses: any[] = new Array<any>();

    let result: Array<CollationData> = null;

    let confirmComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.confirmComponentRef = this.viewContainerRef.createComponent(confirmComponentFactory);


    if (option.useDepartmentWork) {
      responses.push(
        this.matchingService.SaveWorkDepartmentTarget(this.collationInfo.clientKey, this.collationInfo.departmentIds));
    }
    if (option.useSectionWork) {
      responses.push(
        this.matchingService.SaveWorkSectionTarget(this.collationInfo.clientKey, this.collationInfo.sectionIds));
    }

    if (responses.length != 0) {
      forkJoin(responses)
        .subscribe(responseList => {
          if (responseList != undefined && responseList.length == responses.length) {
            this.collateInnerHtml();
          }
          else {

            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '照合処理'),
              this.partsResultMessageComponent);

            this.confirmComponentRef.destroy();
          }
        });
    }
    else {
      this.collateInnerHtml();

    }
  }

  ///////////////////////////////////////////////
  // 照合 照合内部 照合＋ソート＋検索結果の設定
  ///////////////////////////////////////////////
  public collateInnerHtml() {

    let tmpCollations: Array<CollationData>;

    this.matchingService.Collate(this.collationInfo.collationSearch)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          if (response.length > 0) {
            tmpCollations = response;

            this.setCollationsDataFromCollation(tmpCollations);

            // データあり
            this.panelOpenState = false;

            if (this.collationInfo.collationSetting.autoSortMatchingEnabledData == 1) {
              this.collationInfo.sortType = eSortType.CheckBox;
            }

            // チェックの可否を設定する。
            this.InitializeGridCheckBoxEnabled();

            // ソートする。
            this.SortGridData(this.collationInfo.sortType);

            if (this.collationInfo.collationSetting.autoMatching == 1) {
              this.MatchingInner(true);
            }
            else {
              this.processResultService.processAtSuccess(
                this.processCustomResult, MSG_INF.PROCESS_FINISH,this.partsResultMessageComponent);

            }
          }
          else {
            this.panelOpenState = true;

            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NOT_EXIST_SEARCH_DATA, this.partsResultMessageComponent);

          }
        }
        else {

          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '照合'),
            this.partsResultMessageComponent);

        }
        this.confirmComponentRef.destroy();

      });
  }

  // 照合内部 検索結果を加工するためにCollationクラスに詰め替えている。（Angular固有の処理）
  public setCollationsDataFromCollation(tmpCollations: Array<CollationData>) {
    this.collationInfo.collations = new Array<CollationData>();
    for (var i = 0; i < tmpCollations.length; i++) {
      var collationData = new CollationData();
      collationData.setDataFromCollation(tmpCollations[i]);
      this.collationInfo.collations.push(collationData);
    }

  }



  // 照合内部 検索結果の一括照合のチェックボックスの設定
  public InitializeGridCheckBoxEnabled() {

    this.cbxDetailKesiCtrls = new Array<FormControl>(this.collationInfo.collations.length);

    // TODO: ScheduledPayment(Income) and DisplayBillingAmount

    var taxTolerance = this.collationInfo.taxDifferenceTolerance;
    for (var i = 0; i < this.collationInfo.collations.length; i++) {

      let collationData = this.collationInfo.collations[i];

      if (!this.cbxShowMatchedCtrl.value) {
        var feeTolerance = this.userInfoService.ApplicationControl.useForeignCurrency
          ? collationData.currencyTolerance : this.collationInfo.bankFeeTolerance;
  
        var checkable = collationData.VerifyCheckable(
          this.collationInfo.collationSetting,
          feeTolerance,
          taxTolerance,
          this.userInfoService.ApplicationControl.useForeignCurrency == 1,
          this.IsBankTransferFeeRegistered(collationData, false),
          this.IsBankTransferFeeRegistered(collationData, true));
  
        collationData.checkable = checkable;
      }

      this.cbxDetailKesiCtrls[i] = new FormControl(null);
      this.MyFormGroup.removeControl("cbxDetailKesiCtrl"+i);
      this.MyFormGroup.addControl("cbxDetailKesiCtrl"+i,this.cbxDetailKesiCtrls[i]);


      if (collationData.checked) {
        collationData.checkBox = CheckBoxStatus.ON;
      }
      else {
        collationData.checkBox = CheckBoxStatus.OFF;
      }


    }
  }

  // 照合内部 検索結果の一括照合のチェックボックスの設定（手数料）
  public IsBankTransferFeeRegistered(collationData: CollationData, checkFee: boolean): boolean {

    if (collationData.customerId != 0) {
      let bRtn = false;
      if (this.collationInfo.customerFees == null || this.collationInfo.customerFees.length == 0) {
        bRtn = false;
      }
      else {
        this.collationInfo.customerFees.forEach(element => {
          if (element.customerId == collationData.customerId) {
            bRtn = !checkFee
              || (element.fee == collationData.different && element.currencyId == collationData.currencyId);
          }
        })
      }
      return bRtn;
    }

    if (collationData.paymentAgencyId != 0) {
      let bRtn = false;
      if (this.collationInfo.paymentAgencyFees == null || this.collationInfo.paymentAgencyFees.length == 0) {
        bRtn = false;
      }
      else {
        this.collationInfo.paymentAgencyFees.forEach(element => {
          if (element.paymentAgencyCode == collationData.paymentAgencyCode) {
            bRtn = !checkFee
              || (element.fee == collationData.different && element.currencyId == collationData.currencyId);
          }
        })

      }
      return bRtn;
    }

    return false;
  }

  ////////////////////////////////////
  // ソート処理 
  ////////////////////////////////////

  // ソート条件の設定
  public SortGridData(type: eSortType = eSortType.None) {
    let sortItems = new Array<SortItem>();

    this.collationInfo.sortType = type;

    sortItems.push(new SortItem("dupeCheck", SortOrder.Descending));

    switch (type) {
      case eSortType.CheckBox:
        sortItems.push(new SortItem("checked", SortOrder.Descending));
        sortItems.push(new SortItem("checkable", SortOrder.Descending));
        break;
      case eSortType.AdvanceReceive_ASC:
        sortItems.push(new SortItem("advanceReceivedCount", SortOrder.Ascending));
        break;
      case eSortType.AdvanceReceive_DESC:
        sortItems.push(new SortItem("advanceReceivedCount", SortOrder.Descending));
        break;
      default:
        break;
    }

    if (this.collationInfo.billingPriority) //請求
    {
      sortItems.push(new SortItem("billingPriority", SortOrder.Ascending));
      sortItems.push(new SortItem("billingDisplayOrder", SortOrder.Ascending));
      sortItems.push(new SortItem("displayOrder", SortOrder.Ascending));
    }
    else //入金
    {
      sortItems.push(new SortItem("receiptPriority", SortOrder.Ascending));
      sortItems.push(new SortItem("receiptDisplayOrder", SortOrder.Ascending));
      sortItems.push(new SortItem("displayOrder", SortOrder.Ascending));
    }


    if (this.collationInfo.collations == null || this.collationInfo.collations.length == 0) {
      return;
    }
    else {
      this.collationInfo.collations = SortUtil.Sort(sortItems, this.collationInfo.collations);

    }

  }


  ////////////////////////////////////
  // 入力チェック
  ////////////////////////////////////
  public validateInputValues(): boolean {

    return true;
  }

  ////////////////////////////////////
  // 検索条件の設定
  ////////////////////////////////////
  public GetCollationOption(): CollationSearch {
    let collationSearch: CollationSearch = new CollationSearch();

    collationSearch.companyId = this.userInfoService.Company.id;
    collationSearch.clientKey = this.collationInfo.clientKey;
    collationSearch.recordedAtFrom = DateUtil.ConvertFromDatepicker(this.receiptRecordedAtFromCtrl);
    collationSearch.recordedAtTo = DateUtil.ConvertFromDatepicker(this.receiptRecordedAtToCtrl);
    collationSearch.dueAtFrom = DateUtil.ConvertFromDatepicker(this.billingDueAtFromCtrl);
    collationSearch.dueAtTo = DateUtil.ConvertFromDatepicker(this.billingDueAtToCtrl);
    collationSearch.currencyId = this.userInfoService.Currency.id;
    collationSearch.billingType = (this.userInfoService.ApplicationControl.useCashOnDueDates
      || this.userInfoService.ApplicationControl.useFactoring) ? this.cmbBillingDataCtrl.value : 0;
    collationSearch.amountType = this.rdoDisplayTargetAmountCtrl.value;
    collationSearch.approved = !this.userInfoService.ApplicationControl.useAuthorization;
    collationSearch.useDepartmentWork = this.departmentNameCtrl.value != "すべて";
    collationSearch.useSectionWork = this.sectionNameCtrl.value != "すべて";
    collationSearch.loginUserId = this.userInfoService.LoginUser.id;
    collationSearch.doTransferAdvanceReceived = this.userInfoService.ApplicationControl.useScheduledPayment
      && !this.userInfoService.ApplicationControl.useDeclaredAmount
      && false;//this.rdoCarryReceive.Checked,
    collationSearch.recordedAtType = this.collationInfo.collationSetting.advanceReceivedRecordedDateType;
    //collationSearch.inputedRecordedAt   = null,
    collationSearch.useAdvanceReceived = this.collationInfo.collationSetting.useAdvanceReceived == 1,
    collationSearch.createAtFrom = DateUtil.ConvertFromDatepickerToStart(this.createAtFromCtrl);
    collationSearch.createAtTo = DateUtil.ConvertFromDatepickerToEnd(this.createAtToCtrl);

    return collationSearch;
  }



  ////////////////////////////////////
  // 一括消込または消込解除処理
  ////////////////////////////////////
  public matching() {

    // エラーメッセージのクリア
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);


    if (this.cbxShowMatchedCtrl.value) {
      this.CancelInner();
    }
    else {
      this.MatchingInner(false);
    }
  }

  ////////////////////////////////////
  // 消込解除処理
  ////////////////////////////////////
  public CancelInner(): boolean {

    const checkedIds = this.collationInfo.collations.filter(x => x.checked).map(x => x.id);
    const checkedHeaders = this.collationInfo.matchingHeaders.filter(x => checkedIds.includes(x.id));

    if (checkedHeaders.length == 0) {

      this.processResultService.processAtWarning(
        this.processCustomResult, "解除処理を行う明細を選択してください。", this.partsResultMessageComponent);

      return false;
    }


    // 確認ダイアログの表示
    let matchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let matchingComponentRef = this.viewContainerRef.createComponent(matchingComponentFactory);
    matchingComponentRef.instance.ActionName = "解除処理"

    matchingComponentRef.instance.Closing.subscribe(() => {

      if (matchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        this.CancelAsync(checkedHeaders);
      }
      else {

        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, '一括消込処理'),
          this.partsResultMessageComponent);
      }
      matchingComponentRef.destroy();
    });


  }

  ////////////////////////////////////
  // 消込解除処理
  ////////////////////////////////////
  public CancelAsync(checkMatchingHeaderDatas: Array<MatchingHeader>) {

    let result: MatchingResult = null;
    let mtchingCancelSource = new MatchingCancelSource();

    let cancelResult: MatchingResult;

    mtchingCancelSource.headers = checkMatchingHeaderDatas;
    mtchingCancelSource.loginUserId = this.userInfoService.LoginUser.id;
    mtchingCancelSource.connectionId = this.collationInfo.clientKey;


    // 処理中ダイアログの表示
    let matchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let matchingComponentRef = this.viewContainerRef.createComponent(matchingComponentFactory);

    matchingComponentRef.instance.Closing.subscribe(() => {
      matchingComponentRef.destroy();
    });

    this.matchingService.CancelMatching(mtchingCancelSource)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          cancelResult = response;

          let index = cancelResult.errorIndex;
          const header = checkMatchingHeaderDatas[index];
          let errorType = cancelResult.matchingErrorType;
          this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
          this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;

          if (errorType == MatchingErrorType.BillingOmitted) {
            // 対象行の選択
            this.processResultService.processAtWarning(this.processCustomResult, 
              MSG_WNG.CANCEL_MATCHING_ERROR.replace(MSG_ITEM_NUM.FIRST, `請求データ 得意先：${header.dispCustomerCode} ${header.dispCustomerName}`),
              this.partsResultMessageComponent);
          }
          else if (errorType == MatchingErrorType.ReceiptOmitted) {
            // 対象行の選択
            this.processResultService.processAtWarning(this.processCustomResult, 
              MSG_WNG.CANCEL_MATCHING_ERROR.replace(MSG_ITEM_NUM.FIRST, `入金データ 振込依頼人名：${header.payerName}`),
              this.partsResultMessageComponent);
          }
          else if (errorType == MatchingErrorType.CashOnDueDateOmitted) {
            // 対象行の選択
            this.processResultService.processAtWarning(this.processCustomResult, 
              MSG_WNG.CANCEL_MATCHING_ERROR.replace(MSG_ITEM_NUM.FIRST, `期日入金予定データ 得意先：${header.dispCustomerCode} ${header.dispCustomerName}`),
              this.partsResultMessageComponent);
          }
          else if (errorType == MatchingErrorType.PostProcessError) {
            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.POST_PROCESS_FAILURE, this.partsResultMessageComponent);
          }
          else {
            this.processResultService.processAtSuccess
              (this.processCustomResult, MSG_INF.MATCHING_PROCESS_FINISH, this.partsResultMessageComponent);

            this.SearchMatchingHeaders()
          }
        }
        else {
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SEQUENTIAL_MATCHING_ERROR, this.partsResultMessageComponent);
        }
        matchingComponentRef.destroy();

      });
  }

  ////////////////////////////////////
  // 消込処理 入力チェック
  ////////////////////////////////////
  public MatchingInner(silent: boolean): boolean {
    const collations = this.collationInfo.collations.filter(x => x.checked);

    if (collations.length == 0) {
      if (!silent) {
        this.processResultService.processAtWarning(
          this.processCustomResult, "一括消込を行う明細を選択してください。", this.partsResultMessageComponent);
      }
      return false;
    }

    let option = this.collationInfo.collationSearch;

    this.MatchingInnerHtml(collations, option, silent);

    return true;

  }

  ////////////////////////////////////
  // 消込処理 前受確認
  ////////////////////////////////////
  public MatchingInnerHtml(collations: CollationData[], option: CollationSearch, silent: boolean = false) {

    // 初期値はfalseにする。
    let hasAdvanceReceived: boolean = collations.some(x => x.advanceReceivedCount > 0);

    let recordedAtType = this.collationInfo.collationSetting.advanceReceivedRecordedDateType;

    let requireSelectionRecordedAt =
      hasAdvanceReceived
      && (recordedAtType == 0 || recordedAtType == 1);

    if (requireSelectionRecordedAt) {

      // 前受あり時の消込処理日時の入力
      let advancedComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMatchingRecordedAtComponent);
      let advancedComponentRef = this.viewContainerRef.createComponent(advancedComponentFactory);
      advancedComponentRef.instance.CollationSetting = this.collationInfo.collationSetting;

      advancedComponentRef.instance.Closing.subscribe(() => {

        if (advancedComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
          option.advanceReceivedRecordedAt = DateUtil.ConvertFromDatepicker(advancedComponentRef.instance.MatchingDateCtrl);

          advancedComponentRef.destroy();

          this.MatchingSequentialAsync(collations, option, silent);
        }
        else {
          advancedComponentRef.destroy();
          this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, '一括消込'),
            this.partsResultMessageComponent);
        }
      });
    }
    else {
      this.MatchingSequentialAsync(collations, option, silent);
    }

  }

  ////////////////////////////////////
  // 消込処理 Siletの確認
  ////////////////////////////////////  
  public MatchingSequentialAsync(collations: Array<CollationData>, option: CollationSearch, silent: boolean) {

    if (!silent) {
      // 確認ダイアログの表示
      let matchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
      let matchingComponentRef = this.viewContainerRef.createComponent(matchingComponentFactory);
      matchingComponentRef.instance.ActionName = "一括消込処理"

      matchingComponentRef.instance.Closing.subscribe(() => {
        if (matchingComponentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
          this.MatchingSequentialInner(collations, option);
        }
        else {
          this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, '一括消込'),
            this.partsResultMessageComponent);
        }
        matchingComponentRef.destroy();
      });
    }
    else {
      this.MatchingSequentialInner(collations, option);
    }

  }

  ////////////////////////////////////
  // 消込処理 実処理
  ////////////////////////////////////
  public MatchingSequentialInner(checkCollationDatas: Array<CollationData>, option: CollationSearch) {
    let matchingSequentialSource = new MatchingSequentialSource();
    let matchingResult: MatchingResult;

    matchingSequentialSource.collations = checkCollationDatas;
    matchingSequentialSource.option = option;

    let matchingComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let matchingComponentRef = this.viewContainerRef.createComponent(matchingComponentFactory);

    this.matchingService.SequentialMatching(matchingSequentialSource)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          matchingResult = response;

          let index = matchingResult.errorIndex;
          let errorType = matchingResult.matchingErrorType;
          this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
          this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;

          if (errorType == MatchingErrorType.BillingRemainChanged) {
            // 対象行の選択
            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.INCLUDE_OTHER_USER_MATCHED_DATA.replace(MSG_ITEM_NUM.FIRST, '請求'),
              this.partsResultMessageComponent);
          }
          else if (errorType == MatchingErrorType.ReceiptRemainChanged) {
            // 対象行の選択
            this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.INCLUDE_OTHER_USER_MATCHED_DATA.replace(MSG_ITEM_NUM.FIRST, '入金'),
              this.partsResultMessageComponent);
          }
          else if (errorType == MatchingErrorType.PostProcessError) {

            this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.POST_PROCESS_FAILURE, this.partsResultMessageComponent);
          }
          else {

            this.processResultService.processAtSuccess
              (this.processCustomResult, MSG_INF.MATCHING_PROCESS_FINISH, this.partsResultMessageComponent);

            this.collateInner();
          }

        }
        else {
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SEQUENTIAL_MATCHING_ERROR, this.partsResultMessageComponent);

          return false;
        }
        matchingComponentRef.destroy();
      });
  }



  ////////////////////////////////////
  // 印刷 
  ////////////////////////////////////  
  public print() {
    // エラーメッセージのクリア
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    this.cbxShowMatchedCtrl.disable();

    if (this.cbxShowMatchedCtrl.value) {
      this.printCanel();
    }
    else {
      this.printCollate();
    }
  }

  ////////////////////////////////////
  // 印刷 照合
  ////////////////////////////////////  
  public printCollate() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalCollatePrintComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        // 出力内容
        let chckType = componentRef.instance.rdoPrintDataCtrl.value;
        let result: boolean = false;

        let printCollationList = this.getOutputCollations(chckType);
        //印刷
        let matchingSequentialReportSource = new MatchingSequentialReportSource();
        matchingSequentialReportSource.companyId = this.userInfoService.Company.id;
        matchingSequentialReportSource.priorReceipt = this.collationInfo.collationSetting.billingReceiptDisplayOrder == 1;
        matchingSequentialReportSource.precision = 0;
        matchingSequentialReportSource.items = printCollationList;

        this.matchingService.GetSequentialReport(matchingSequentialReportSource)
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
      else {
        this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
        this.processCustomResult.message = MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, '印刷');

      }
      componentRef.destroy();
    });
  }

  ////////////////////////////////////
  // 印刷 消込履歴
  ////////////////////////////////////  
  public printCanel() {

    this.matchingService.collationInfo = this.collationInfo;
    this.matchingService.Pe0101myFormGroup = this.MyFormGroup;

    this.router.navigate(['main/PE0301', { "process": "from" }]);


  }


  ////////////////////////////////////
  // エクスポート 消込履歴
  ////////////////////////////////////  
  public export() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalCollatePrintComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        // 出力内容
        let chckType = componentRef.instance.rdoPrintDataCtrl.value;

        let collationList = this.getOutputCollations(chckType);
        let result: boolean = false;

        // 件数チェック
        if (collationList.length <= 0) {
          this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
          this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
          this.processCustomResult.message = MSG_WNG.NO_EXPORT_DATA;
          componentRef.destroy();
          return;
        }
        let tempHeader: string[];
        let useCurrency = this.userInfoService.ApplicationControl.useForeignCurrency == 1;

        tempHeader = REPORT_HEADER.MATCHING_SEQUENTIAL.filter(element => {
          if (element == "通貨コード") {
            componentRef.destroy();
            if (useCurrency) {
              return true;
            }
            else {
              return false;
            }
          }
        });


        let headers = FileUtil.encloseItemBySymbol(tempHeader);
        let data: string = headers.join(",") + LINE_FEED_CODE;

        for (let index = 0; index < collationList.length; index++) {
          let dataItem: Array<any> = [];
          dataItem.push(collationList[index].checked ? "レ" : "  ");
          if (useCurrency) {
            dataItem.push(collationList[index].currencyCode);
          }
          dataItem.push(collationList[index].dispCustomerCode);
          dataItem.push(collationList[index].dispCustomerName);
          dataItem.push(collationList[index].dispBillingCount);
          dataItem.push(collationList[index].dispBillingAmount);
          dataItem.push(collationList[index].payerName);
          dataItem.push(collationList[index].dispReceiptCount);
          dataItem.push(collationList[index].dispReceiptAmount);
          dataItem.push(collationList[index].dispShareTransferFee);
          dataItem.push(collationList[index].dispDifferent)
          dataItem.push(collationList[index].dispAdvanceReceivedCount);

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

      }
      else {
        this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
        this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
        this.processCustomResult.message = MSG_WNG.CANCEL_PROCESS.replace(MSG_ITEM_NUM.FIRST, 'エクスポート');
      }
      componentRef.destroy();
    });
  }

  public getOutputCollations(outputType: number): Array<CollationData> {
    let collations = new Array<CollationData>();
    collations = this.collationInfo.collations.filter(element => {
      if (outputType == 0 ||
        (outputType == 1 && element.checkBox == CheckBoxStatus.ON) ||
        (outputType == 2 && element.checkBox == CheckBoxStatus.OFF)) {
        return true;
      }
      return false;
    });

    return collations;
  }

  /////////////////////////////////////////////////////////////////////////////
  public checkAll() {
    if(this.collationInfo.collations!=null&&this.collationInfo.collations.length>0){
      this.collationInfo.collations.filter(x => x.checkable).map(x => x.checked = this.cbxCheckAllCtrl.value);
    }
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

  public diableExecution(): boolean {
    if (this.collationInfo == undefined ||
      this.collationInfo.collations == undefined ||
      this.collationInfo.collations.length === 0) return true;
    return this.collationInfo.collations.every(x => !x.checked);
  }

  public getOrderName(itemName: string, categoryType: number): string {
    return MatchingUtil.getMatchingOrderName(itemName, categoryType);
  }

  public getSortName(itemName: string, categoryType: number): string {

    let tmpOrders: Array<MatchingOrder>;
    let tmpOrder: MatchingOrder;

    if (categoryType == CategoryType.Billing) {
      tmpOrders = this.collationInfo.matchingBillingOrders;
    }
    else if (categoryType == CategoryType.Receipt) {
      tmpOrders = this.collationInfo.matchingReceiptOrders;
    }
    else {
      return "";
    }

    if (tmpOrders == null || tmpOrders.length == 0) return "";
    tmpOrders.forEach(element => {
      if (element.itemName == itemName) {
        tmpOrder = element;
      }
    })

    if (tmpOrder != null) {
      return MatchingUtil.getMatchingSortOrderName(tmpOrder.sortOrder);
    }
    else {
      return "";
    }

  }


  public openMultiMasterModal(table: TABLE_INDEX) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMultiMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;
    if(table==TABLE_INDEX.MASTER_DEPARTMENT){
      componentRef.instance.InitialIds = this.collationInfo.departmentIds;
    }
    else{
      componentRef.instance.InitialIds = this.collationInfo.sectionIds;
    }

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              if (componentRef.instance.isSelectedAll) {
                this.departmentNameCtrl.setValue("すべて");
                this.collationInfo.departmentIds = componentRef.instance.SelectedIds;
              }
              else {
                this.departmentNameCtrl.setValue("請求部門絞込有");
                this.collationInfo.departmentIds = componentRef.instance.SelectedIds;
              }
              break;
            }
          case TABLE_INDEX.MASTER_SECTION:
            {
              if (componentRef.instance.isSelectedAll) {
                this.sectionNameCtrl.setValue("すべて");
                this.collationInfo.sectionIds = componentRef.instance.SelectedIds;
              }
              else {
                this.sectionNameCtrl.setValue("入金部門絞込有");
                this.collationInfo.sectionIds = componentRef.instance.SelectedIds;
              }
              break;
            }
        }
      }
      componentRef.destroy();
    });
  }


  public openMasterModal(table: TABLE_INDEX) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_CURRENCY:
            {
              this.currencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              break;
            }
        }
      }
      componentRef.destroy();
    });
}

  /**
   * 選択した行の得意先データ表示
   * @param index 行番号
   */
  public openCustomerModal(index: number) {

    let collationData: CollationData = this.collationInfo.collations[index];

    if (StringUtil.IsNullOrEmpty(collationData.paymentAgencyCode)) {

      this.customerMasterService.GetItemsById(collationData.customerId)
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalCustomerDetailComponent);
            let componentRef = this.viewContainerRef.createComponent(componentFactory);

            componentRef.instance.Customer = response[0]; // 0～99のランダム値を渡す
            componentRef.instance.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
            componentRef.instance.fromPageId = ComponentId.PE0101;
            componentRef.instance.processModalCustomResult = this.processModalCustomResult;

            this.customerMasterService.selectCustmer = response[0];

            componentRef.instance.Closing.subscribe(() => {
              componentRef.destroy();
            });
          }
        });
    } else {
      // 別画面にいくため現在の情報を退避
      this.matchingService.collationInfo = this.collationInfo;
      this.matchingService.Pe0101myFormGroup = this.MyFormGroup;

      this.router.navigate(['main/PB1901', { paymentAgencyCode: collationData.paymentAgencyCode }]);
    }
  }

  public initializeDepartmentSelection() {

    if (this.departments == undefined) return;

    if (this.userInfoService.ApplicationControl.useReceiptSection == 0 || this.departmentsWithLoginUser.length == 0) {
      this.departmentNameCtrl.setValue("すべて");
      this.collationInfo.departmentIds = this.departments.map(x => x.id);
    }
    else {
      if (this.departments.length == this.departmentsWithLoginUser.length) {
        this.departmentNameCtrl.setValue("すべて");
      }
      else if (this.departmentsWithLoginUser.length == 1) {
        this.departmentNameCtrl.setValue(this.departmentsWithLoginUser[0].name);
      }
      else {
        this.departmentNameCtrl.setValue("請求部門絞込有");
      }
      this.collationInfo.departmentIds = this.departmentsWithLoginUser.map(x => x.id);
    }
  }

  public initializeSectionSelection() {

    if (this.sections == undefined || this.sectionsWithLoginUser == undefined) return;

    if (this.sectionsWithLoginUser.length == 0) {
      this.sectionNameCtrl.setValue("すべて");
      this.collationInfo.sectionIds = this.sections.map(x => x.id);
    }
    else {
      if (this.sections.length == this.sectionsWithLoginUser.length) {
        this.sectionNameCtrl.setValue("すべて");
      }
      else if (this.sectionsWithLoginUser.length == 1) {
        this.sectionNameCtrl.setValue(this.sectionsWithLoginUser[0].name);
      }
      else {
        this.sectionNameCtrl.setValue("入金部門絞込有");
      }
      this.collationInfo.sectionIds = this.sectionsWithLoginUser.map(x => x.id);
    }

  }

  public selectLine(lineNo: number) {

    // 選択された行Noを設定
    this.collationInfo.individualIndexNo = lineNo;

    // 検索対象の設定
    this.collationInfo.isMatched = this.cbxShowMatchedCtrl.value;

    // 別画面にいくため現在の情報を退避
    this.matchingService.collationInfo = this.collationInfo;
    this.matchingService.Pe0101myFormGroup = this.MyFormGroup;


    this.router.navigate(['main/PE0102', { customerCode: this.collationInfo.collations[lineNo].customerId }]);
  }


  ///////////////////////////////////////////////////////////////////////
  public setReceiptRecordedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef,
      'receiptRecordedAtToCtrl', eventType);
  }

  public setReceiptRecordedAtTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef,
      ['billingDueAtFromCtrl', 'billingDueAtToCtrl'], eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setBillingDueAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef,
      'billingDueAtToCtrl', eventType);
  }

  public setBillingDueAtTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef,
      ['currencyCodeCtrl', 'createAtFromCtrl', 'createAtToCtrl',
        'receiptRecordedAtFromCtrl', 'receiptRecordedAtToCtrl'], eventType);
  }


  ///////////////////////////////////////////////////////////////////////
  public setCurrencyCode(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.currencyCodeCtrl.value)) {

      this.currencyService.GetItems(this.currencyCodeCtrl.value)
        .subscribe(response => {
          if (response != undefined && response.length > 0) {
            this.currencyCodeCtrl.setValue(response[0].code);

            HtmlUtil.nextFocusByNames(this.elementRef,
              ['createAtFromCtrl', 'createAtToCtrl',
                'receiptRecordedAtFromCtrl', 'receiptRecordedAtToCtrl'], eventType);

          }
          else {
            this.currencyCodeCtrl.setValue("");
          }
        });
    }
    else {
      this.currencyCodeCtrl.setValue("");
      HtmlUtil.nextFocusByNames(this.elementRef,
        ['createAtFromCtrl', 'createAtToCtrl',
          'receiptRecordedAtFromCtrl', 'receiptRecordedAtToCtrl'], eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setCreateAtFrom(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef,
      ['createAtToCtrl', 'receiptRecordedAtFromCtrl', 'receiptRecordedAtToCtrl'], eventType);
  }

  public setCreateAtTo(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef,
      ['receiptRecordedAtFromCtrl', 'receiptRecordedAtToCtrl'], eventType);
  }



  ///////////////////////////////////////////////////////////////////////
  // 消込完了データーを表示のチェックボックス
  public setCbxShowMatched() {
    this.securityHideShow = true;
    this.createAtFromCtrl.setValue(null);
    this.createAtToCtrl.setValue(null);

    if (this.cbxShowMatchedCtrl.value) {
      this.createAtFromCtrl.enable();
      this.createAtToCtrl.enable();

      this.collationInfo.collateButtonName = "表示";
      this.collationInfo.matchingButtonName = "消込解除";
      this.collationInfo.selectionAlias = '解除';
      this.collationInfo.noteAlias = 'メモ';

      if (this.collationInfo.collationSetting.setSystemDateToCreateAtFilter == 1) {
        let today = this.calendar.getToday();

        // 入金日（終了日） 現在日時
        this.createAtFromCtrl.setValue(today);
        this.createAtToCtrl.setValue(today);

        this.createAtFromCtrl.enable();
        this.createAtToCtrl.enable();
      }

      if (!this.userInfoService.isFunctionAvailable(FunctionType.CancelMatching)) {
        this.securityHideShow = false;
      }

    }
    else {
      this.createAtFromCtrl.disable();
      this.createAtToCtrl.disable();

      this.collationInfo.collateButtonName = "照合";
      this.collationInfo.matchingButtonName = "一括消込";
      this.collationInfo.selectionAlias = '一括';
      this.collationInfo.noteAlias = '前受';

    }
  }

  ///////////////////////////////////////////////////////////////////////
  // 背景を変える方法は別途検討
  ///////////////////////////////////////////////////////////////////////
  public setSearchKeyCtrl(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {

      for (let index = 0; index < this.collationInfo.collations.length; index++) {
        if (this.collationInfo.collations[index].customerCode == this.searchKeyCtrl.value) {
          break;
        }
        if (this.collationInfo.collations[index].customerName == this.searchKeyCtrl.value) {
          break;
        }
        if (this.collationInfo.collations[index].payerName == this.searchKeyCtrl.value) {
          break;
        }
      }
    }
  }


  public setSort( sortPanelNo: number) {

    let sortType = eSortType.None
    if(sortPanelNo==0){
      if(this.collationInfo.sortType==eSortType.CheckBox){
        sortType = eSortType.None;
      }
      else{
        sortType = eSortType.CheckBox;
      }
    }
    else{
      if(this.collationInfo.sortType==eSortType.CheckBox || this.collationInfo.sortType==eSortType.None){
        sortType = eSortType.AdvanceReceive_ASC;
      }
      else if(this.collationInfo.sortType==eSortType.AdvanceReceive_ASC){
        sortType = eSortType.AdvanceReceive_DESC;
      }
      else if(this.collationInfo.sortType==eSortType.AdvanceReceive_DESC){
        sortType = eSortType.None;
      }      
    }

    this.openSortOptions[sortPanelNo] = !this.openSortOptions[sortPanelNo];

    this.SortGridData(sortType);
  }

  public getSortRow( sortPanelNo: number) {

    if(sortPanelNo==0){
      if(this.collationInfo.sortType==eSortType.CheckBox){
        return "▽";
      }
      else{
        return "";
      }
    }
    else{
      if(this.collationInfo.sortType==eSortType.AdvanceReceive_ASC){
        return "△";
      }
      else if(this.collationInfo.sortType==eSortType.AdvanceReceive_DESC){
        return "▽";
      }
      else {
        return "";
      }      
    }
  }


  public setHeaderSort(categoryType: CategoryType) {

    if (categoryType == CategoryType.Billing) {
      this.collationInfo.billingPriority = true;
    }
    else if (categoryType == CategoryType.Receipt) {
      this.collationInfo.billingPriority = false;
    }

    this.SortGridData(eSortType.None);

  }

  public getHeaderSortRow(categoryType: CategoryType) {

    if (categoryType == CategoryType.Billing) {
      if(this.collationInfo.billingPriority){
        return "▽";
      }
    }
    else if (categoryType == CategoryType.Receipt) {
      if(!this.collationInfo.billingPriority){
        return "▽";
      }
    }

    return "";
  }


  public openSortOptions: Array<boolean> = new Array<boolean>();
  public OpenSortOptions(index: number) {
    this.openSortOptions[index] = !this.openSortOptions[index]
  }



}
