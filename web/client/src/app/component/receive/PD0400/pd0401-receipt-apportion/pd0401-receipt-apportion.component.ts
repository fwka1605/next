import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { ReceiptService } from 'src/app/service/receipt.service';
import { ReceiptHeadersResult } from 'src/app/model/receipt-headers-result.model';
import { FormControl, FormGroup } from '@angular/forms';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoryType, ACCOUNT_TYPE_DICTIONARY, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { forkJoin } from 'rxjs';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { CollationSettingResult } from 'src/app/model/collation-setting-result.model';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { ReceiptApportion } from 'src/app/model/receipt-apportion.model';
import { ReceiptApportionsResult } from 'src/app/model/receipt-apportions-result.model';
import { ReceiptHeader } from 'src/app/model/receipt-header.model';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { RacCurrencyPipe } from 'src/app/pipe/currency.pipe';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_WNG, MSG_INF, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';

@Component({
  selector: 'app-pd0401-receipt-apportion',
  templateUrl: './pd0401-receipt-apportion.component.html',
  styleUrls: ['./pd0401-receipt-apportion.component.css']
})
export class Pd0401ReceiptApportionComponent extends BaseComponent implements OnInit {

  public ACCOUNT_TYPE_DICTIONARY: typeof ACCOUNT_TYPE_DICTIONARY = ACCOUNT_TYPE_DICTIONARY;

  public pipe = new RacCurrencyPipe();

  public selectedHeaderFlag: boolean = false;
  public apportionedFlag: boolean = false;
  public selectedHeaderIndex: number;

  public collationSettingResult: CollationSettingResult;
  public excludeCategoriesResult: CategoriesResult;
  public receiptHeadersResult: ReceiptHeadersResult;
  public receiptApportionsResult: ReceiptApportionsResult;
  public allReceiptApportionsResult: ReceiptApportionsResult;
  public sectionAssignReceiptApportionsResult: ReceiptApportionsResult;

  public selectedReceiptHeader: ReceiptHeader;

  public cbxLearnIgnoreKanaCtrl: FormControl; // 除外カナを学習
  public cbxSectionAssignCtrl: FormControl; // 入金部門未指定のデータのみ表示

  public headerExcludeCategoryIdCtrls: FormControl[]; // ヘッダー 対象外区分
  public detailExcludeCheckCtrls: FormControl[];
  public detailExcludeCategoryIdCtrls: FormControl[]; // 詳細 対象外区分
  public detailExcludeAmountCtrls: FormControl[]; // 詳細 対象外金額
  public detailCustomerCodeCtrls: FormControl[];
  public detailCustomerIdCtrls: FormControl[];
  public detailCustomerNameCtrls: FormControl[];
  public detailSectionCodeCtrls: FormControl[];
  public detailSectionNameCtrls: FormControl[];

  public undefineCtrl: FormControl; // 未定用

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public receiptService: ReceiptService,
    public categoryService: CategoryMasterService,
    public collationSettingService: CollationSettingMasterService,
    public customerService: CustomerMasterService,
    public processResultService: ProcessResultService,
    public sectionService: SectionMasterService
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

    this.setControlInit();
    this.setValidator();
    this.clear();

    let collationSettingResult =
      this.collationSettingService.Get();

    let excludeCategoryResponse =
      this.categoryService.GetItemsByCategoryType(CategoryType.Exclude);

    let receiptResponse =
      this.receiptService.GetHeaderItems();

    // ３つの処理の待機
    forkJoin(
      collationSettingResult,
      excludeCategoryResponse,
      receiptResponse,
    )
      .subscribe(responseList => {

        if (responseList.length != 3
          || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE
          || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE
          || responseList[2] == PROCESS_RESULT_RESULT_TYPE.FAILURE
        ) {
          modalRouterProgressComponentRef.destroy();
        }
        else {
          this.collationSettingResult = new CollationSettingResult();
          this.collationSettingResult.collationSetting = responseList[0];
          this.excludeCategoriesResult = new CategoriesResult();
          this.excludeCategoriesResult.categories = responseList[1];
          this.receiptHeadersResult = new ReceiptHeadersResult();
          this.receiptHeadersResult.receiptHeaders = responseList[2];

          this.headerExcludeCategoryIdCtrls = new Array<FormControl>(this.receiptHeadersResult.receiptHeaders.length);

          for (let index: number = 0; index < this.receiptHeadersResult.receiptHeaders.length; index++) {

            this.headerExcludeCategoryIdCtrls[index] = new FormControl("");
            if (this.receiptHeadersResult.receiptHeaders[index].existApportioned == 1) {
              this.headerExcludeCategoryIdCtrls[index].disable();
            }
            else {
              this.headerExcludeCategoryIdCtrls[index].enable();
            }

            this.MyFormGroup.removeControl("headerExcludeCategoryIdCtrl" + index);
            this.MyFormGroup.addControl("headerExcludeCategoryIdCtrl" + index, this.headerExcludeCategoryIdCtrls[index]);


          }

          modalRouterProgressComponentRef.destroy();

        }
      },
      error => {
          modalRouterProgressComponentRef.destroy();

        }
      );

  }

  public setControlInit() {

    this.cbxLearnIgnoreKanaCtrl = new FormControl(""); // 除外カナを学習
    this.cbxSectionAssignCtrl = new FormControl(""); // 入金部門未指定のデータのみ表示

    this.undefineCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      cbxLearnIgnoreKanaCtrl: this.cbxLearnIgnoreKanaCtrl,
      cbxSectionAssignCtrl: this.cbxSectionAssignCtrl,

      UndefineCtrl: this.undefineCtrl,

    });

  }

  public clear() {
    this.MyFormGroup.reset();
    this.receiptApportionsResult = null;
    this.selectedHeaderFlag = false;
    this.selectedHeaderIndex = 0;
    this.apportionedFlag = false;

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

  }

  public openMasterModal(table: TABLE_INDEX, index: number = -1) {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        switch (table) {
          case TABLE_INDEX.MASTER_CUSTOMER:
            this.detailCustomerCodeCtrls[index].setValue(componentRef.instance.SelectedCode);
            this.detailCustomerIdCtrls[index].setValue(componentRef.instance.SelectedId);
            this.detailCustomerNameCtrls[index].setValue(componentRef.instance.SelectedName);              
            break;
            
          case TABLE_INDEX.MASTER_SECTION:
            this.detailSectionCodeCtrls[index].setValue(componentRef.instance.SelectedCode);
            this.detailSectionNameCtrls[index].setValue(componentRef.instance.SelectedName);
            this.receiptApportionsResult.receiptApportion[index].sectionId = componentRef.instance.SelectedId;
            break;
        }
      }

      componentRef.destroy();
    });
  }

  public selectHeader(index: number) {

    this.selectedReceiptHeader = this.receiptHeadersResult.receiptHeaders[index];

    this.receiptService.GetApportionItems([this.receiptHeadersResult.receiptHeaders[index].id])
      .subscribe(response => {
        if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
        }
        else {

          this.selectedHeaderFlag = true;
          this.selectedHeaderIndex = index;
          this.apportionedFlag = this.receiptHeadersResult.receiptHeaders[index].existApportioned == 1;

          this.allReceiptApportionsResult = new ReceiptApportionsResult();
          this.allReceiptApportionsResult.receiptApportion = response;

          this.sectionAssignReceiptApportionsResult = new ReceiptApportionsResult();
          this.sectionAssignReceiptApportionsResult.receiptApportion = new Array<ReceiptApportion>();


          this.detailExcludeCheckCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailExcludeCategoryIdCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailExcludeAmountCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailCustomerCodeCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailCustomerIdCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailCustomerNameCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailSectionCodeCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);
          this.detailSectionNameCtrls = new Array<FormControl>(this.allReceiptApportionsResult.receiptApportion.length);

          for (let i = 0; i < this.allReceiptApportionsResult.receiptApportion.length; i++) {
            this.detailExcludeCheckCtrls[i] = new FormControl("");
            this.detailExcludeCategoryIdCtrls[i] = new FormControl("");
            this.detailExcludeAmountCtrls[i] = new FormControl("");
            this.detailCustomerCodeCtrls[i] = new FormControl("");
            this.detailCustomerIdCtrls[i] = new FormControl("");
            this.detailCustomerNameCtrls[i] = new FormControl("");
            this.detailSectionCodeCtrls[i] = new FormControl("");
            this.detailSectionNameCtrls[i] = new FormControl("");

            this.MyFormGroup.removeControl("detailExclideCheckCtrl" + i);
            this.MyFormGroup.removeControl("detailExcludeCategoryIdCtrl" + i);
            this.MyFormGroup.removeControl("detailExcludeAmountCtrl" + i);
            this.MyFormGroup.removeControl("detailCustomerCodeCtrl" + i);
            this.MyFormGroup.removeControl("detailCustomerIdCtrl" + i);
            this.MyFormGroup.removeControl("detailCustomerNameCtrl" + i);
            this.MyFormGroup.removeControl("detailSectionCodeCtrl" + i);
            this.MyFormGroup.removeControl("detailSectionNameCtrl" + i);

            this.MyFormGroup.addControl("detailExclideCheckCtrl" + i, this.detailExcludeCheckCtrls[i]);
            this.MyFormGroup.addControl("detailExcludeCategoryIdCtrl" + i, this.detailExcludeCategoryIdCtrls[i]);
            this.MyFormGroup.addControl("detailExcludeAmountCtrl" + i, this.detailExcludeAmountCtrls[i]);
            this.MyFormGroup.addControl("detailCustomerCodeCtrl" + i, this.detailCustomerCodeCtrls[i]);
            this.MyFormGroup.addControl("detailCustomerIdCtrl" + i, this.detailCustomerIdCtrls[i]);
            this.MyFormGroup.addControl("detailCustomerNameCtrl" + i, this.detailCustomerNameCtrls[i]);
            this.MyFormGroup.addControl("detailSectionCodeCtrl" + i, this.detailSectionCodeCtrls[i]);
            this.MyFormGroup.addControl("detailSectionNameCtrl" + i, this.detailSectionNameCtrls[i]);

            FormatterUtil.setCurrencyFormatter(this.detailExcludeAmountCtrls[i]);
            if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
              FormatterUtil.setNumberFormatter(this.detailCustomerCodeCtrls[i]);
              FormatterUtil.setNumberFormatter(this.detailSectionCodeCtrls[i]);
            }
            else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
              FormatterUtil.setCustomerCodeKanaFormatter(this.detailCustomerCodeCtrls[i]);
              FormatterUtil.setCustomerCodeKanaFormatter(this.detailSectionCodeCtrls[i]);
            }
            else {
              FormatterUtil.setCustomerCodeAlphaFormatter(this.detailCustomerCodeCtrls[i]);
              FormatterUtil.setCustomerCodeAlphaFormatter(this.detailSectionCodeCtrls[i]);
            }

            if (this.allReceiptApportionsResult.receiptApportion[i].apportioned == 1) {
              this.detailExcludeCheckCtrls[i].disable();
              this.detailExcludeCategoryIdCtrls[i].disable();
              this.detailExcludeAmountCtrls[i].disable();
              this.detailCustomerCodeCtrls[i].disable();
              this.detailCustomerIdCtrls[i].disable();
              this.detailCustomerNameCtrls[i].disable();
              this.detailSectionCodeCtrls[i].disable();

            }
            else {
              this.detailExcludeCheckCtrls[i].enable();

              if (this.allReceiptApportionsResult.receiptApportion[i].excludeFlag == 1) {
                this.detailExcludeCategoryIdCtrls[i].enable();
                this.detailExcludeAmountCtrls[i].enable();
              }
              else {
                this.detailExcludeCategoryIdCtrls[i].disable();
                this.detailExcludeAmountCtrls[i].disable();
              }
            }

            if (this.allReceiptApportionsResult.receiptApportion[i].excludeFlag == 1) {
              this.detailExcludeCheckCtrls[i].setValue(true);
            }
            else {
              this.detailExcludeCheckCtrls[i].setValue("");
            }
            
            if ( StringUtil.IsNullOrEmpty(this.headerExcludeCategoryIdCtrls[index].value) || this.headerExcludeCategoryIdCtrls[index].value == "0" ){              
              this.detailExcludeCategoryIdCtrls[i].setValue(this.allReceiptApportionsResult.receiptApportion[i].excludeCategoryId);
              this.detailExcludeAmountCtrls[i].setValue(this.pipe.transform(this.allReceiptApportionsResult.receiptApportion[i].excludeAmount));              
            }
            else{
              this.detailExcludeCheckCtrls[i].setValue(true);
              this.detailExcludeCategoryIdCtrls[i].enable();
              this.detailExcludeCategoryIdCtrls[i].setValue(this.headerExcludeCategoryIdCtrls[index].value);

              this.detailExcludeAmountCtrls[i].enable();
              this.detailExcludeAmountCtrls[i].setValue(this.pipe.transform(this.allReceiptApportionsResult.receiptApportion[i].receiptAmount));
            }
            this.detailCustomerCodeCtrls[i].setValue(this.allReceiptApportionsResult.receiptApportion[i].customerCode);
            this.detailCustomerIdCtrls[i].setValue(this.allReceiptApportionsResult.receiptApportion[i].customerId);
            this.detailCustomerNameCtrls[i].setValue(this.allReceiptApportionsResult.receiptApportion[i].customerName);

            if (StringUtil.IsNullOrEmpty(this.allReceiptApportionsResult.receiptApportion[i].sectionCode)) {
              this.sectionAssignReceiptApportionsResult.receiptApportion.push(this.allReceiptApportionsResult.receiptApportion[i]);

            } else {
              this.detailSectionCodeCtrls[i].setValue(this.allReceiptApportionsResult.receiptApportion[i].sectionCode);
              this.detailSectionNameCtrls[i].setValue(this.allReceiptApportionsResult.receiptApportion[i].sectionName);
            }
          }

          this.receiptApportionsResult = new ReceiptApportionsResult();
          this.receiptApportionsResult = this.allReceiptApportionsResult;
        }
      }
      );
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
        this.deleteAll();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }    

  public registry(){

    if (!this.validateInput()) return;

    let receiptApportions= new Array<ReceiptApportion>();

    // RequiredCustomerのRequiredCustomerが1の場合は顧客が必須

    for (let index = 0; index < this.receiptApportionsResult.receiptApportion.length; index++) {

      let receiptApportion = new ReceiptApportion();

      receiptApportion = this.receiptApportionsResult.receiptApportion[index];

      //receiptApportion.id
      //receiptApportion.receiptHeaderId
      //receiptApportion.companyId = this.userInfoService.Company.id;
      //receiptApportion.currencyId = this.selectedReceiptHeader.currencyId;

      receiptApportion.updateBy = this.userInfoService.LoginUser.id;
      //receiptApportion.updateAt

      receiptApportion.apportioned = 1;

      //receiptApportion.payerName
      //receiptApportion.sourceBankName
      //receiptApportion.sourceBranchName

      if (!StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value)) {
        receiptApportion.customerId = this.detailCustomerIdCtrls[index].value;
        receiptApportion.customerCode = this.detailCustomerCodeCtrls[index].value;
      }

      //receiptApportion.sectionId

      if (this.detailExcludeCheckCtrls[index].value) {
        receiptApportion.excludeFlag = 1;
        receiptApportion.excludeCategoryId = this.detailExcludeCategoryIdCtrls[index].value;
        receiptApportion.excludeAmount = parseInt(this.pipe.reverceTransform(this.detailExcludeAmountCtrls[index].value));
      }
      else {
        receiptApportion.excludeFlag = 0;
        receiptApportion.excludeAmount = 0;
      }

      //receiptApportion.excludeAmount = parseInt(this.pipe.reverceTransform(this.detailExcludeAmountCtrls[index].value));

      receiptApportion.doDelete = 0;

      if (this.cbxLearnIgnoreKanaCtrl.value) {
        receiptApportion.learnIgnoreKana = 1;
      }
      else {
        receiptApportion.learnIgnoreKana = 0;
      }

      //receiptApportion.payerNameRaw
      receiptApportion.learnKanaHistory = this.collationSettingResult.collationSetting.learnKanaHistory;

      receiptApportions.push(receiptApportion);
    }


    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });    

    let receiptApportionsResult: ReceiptApportionsResult;
    this.receiptService.Apportion(receiptApportions)
      .subscribe(responseSave=>{

        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, responseSave, true, this.partsResultMessageComponent);
        
        receiptApportionsResult=responseSave;
        this.receiptService.GetHeaderItems()
          .subscribe(responseHeader => {
            this.receiptHeadersResult = new ReceiptHeadersResult();
            this.receiptHeadersResult.receiptHeaders = responseHeader;

            for (let index: number = 0; index < this.receiptHeadersResult.receiptHeaders.length; index++) {
              if (this.receiptHeadersResult.receiptHeaders[index].existApportioned == 1) {
                this.headerExcludeCategoryIdCtrls[index].disable();
              }
              else {
                this.headerExcludeCategoryIdCtrls[index].enable();
              }
            }            

            modalRouterProgressComponentRef.destroy();
            this.clear();
          });

      });
  }

  public validateInput(): boolean {

    for (let index = 0; index < this.receiptApportionsResult.receiptApportion.length; index++) {

      let receiptApportion = this.receiptApportionsResult.receiptApportion[index];

      if (this.detailExcludeCheckCtrls[index].value) {
        // 入力チェック
        // 対象外をチェックしている場合は、対象外区分、対象外金額が必須。
        // 対象外金額は入金額を超えられれない。
        if (StringUtil.IsNullOrEmpty(this.detailExcludeCategoryIdCtrls[index].value) || 
            this.detailExcludeCategoryIdCtrls[index].value == "0"
          ) {
          this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '対象外区分'),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailExcludeCategoryIdCtrl' + index, EVENT_TYPE.NONE);
          return false;
        }

        if (StringUtil.IsNullOrEmpty(this.pipe.reverceTransform(this.detailExcludeAmountCtrls[index].value))) {
          this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '対象外金額'),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailExcludeAmountCtrl' + index, EVENT_TYPE.NONE);          
          return false;
        }

        if (receiptApportion.receiptAmount < parseInt(this.pipe.reverceTransform(this.detailExcludeAmountCtrls[index].value))) {
          this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '入金額以下の「対象外金額」'),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailExcludeAmountCtrl' + index, EVENT_TYPE.NONE);
          return false;
        }
      }

      if (this.collationSettingResult.collationSetting.requiredCustomer == 1) {
        if (StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value)) {
          this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '得意先コード'),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailCustomerCodeCtrl' + index, EVENT_TYPE.NONE);
          return false;
        }
      }

      // useReceiptSection==1の場合、入金部門コードが必須
      if (this.userInfoService.ApplicationControl.useReceiptSection == 1) {
        if (StringUtil.IsNullOrEmpty(this.detailSectionCodeCtrls[index].value)) {
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '入金部門コード'),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'detailSectionCodeCtrl' + index, EVENT_TYPE.NONE);
          return false;
        }
      }
    }

    return true;
  }

  public deleteAll() {
    let receiptApportions = new Array<ReceiptApportion>();

    for (let index = 0; index < this.receiptApportionsResult.receiptApportion.length; index++) {
      let tmpReceiptApportion = new ReceiptApportion();

      tmpReceiptApportion.receiptHeaderId = this.receiptApportionsResult.receiptApportion[index].receiptHeaderId;
      tmpReceiptApportion.id = this.receiptApportionsResult.receiptApportion[index].id;
      tmpReceiptApportion.companyId = this.receiptApportionsResult.receiptApportion[index].companyId;
      tmpReceiptApportion.currencyId = this.receiptApportionsResult.receiptApportion[index].currencyId;

      if (!StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value)) {
        tmpReceiptApportion.customerId = this.detailCustomerCodeCtrls[index].value;
        tmpReceiptApportion.customerCode = this.detailCustomerIdCtrls[index].value;
      }

      //入金部門：未実装
      // tmpReceiptApportion.sectionId;
      // tmpReceiptApportion.sectionCode;

      tmpReceiptApportion.excludeFlag = this.detailExcludeCheckCtrls[index].value ? 1 : 0;
      if (!StringUtil.IsNullOrEmpty(this.detailExcludeCategoryIdCtrls[index].value)) {
        tmpReceiptApportion.excludeCategoryId = parseInt(this.detailExcludeCategoryIdCtrls[index].value);
      }
      tmpReceiptApportion.excludeAmount = parseInt(this.pipe.transform(this.detailExcludeAmountCtrls[index].value));

      tmpReceiptApportion.updateAt = this.receiptApportionsResult.receiptApportion[index].updateAt;
      tmpReceiptApportion.updateBy = this.userInfoService.LoginUser.id;
      tmpReceiptApportion.apportioned = 1;
      tmpReceiptApportion.doDelete = 1;
      tmpReceiptApportion.learnIgnoreKana = 0;

      receiptApportions.push(tmpReceiptApportion);
    }

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      modalRouterProgressComponentRef.destroy();
    });    


    let receiptApportionsResult: ReceiptApportionsResult;
    this.receiptService.Apportion(receiptApportions)
    .subscribe(responseSave=>{

      if(responseSave != PROCESS_RESULT_RESULT_TYPE.FAILURE){
        
        receiptApportionsResult = responseSave;
        this.receiptService.GetHeaderItems()
        .subscribe(responseHeader=>{

          if(responseHeader!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
            this.processCustomResult = this.processResultService.processAtSuccess(
              this.processCustomResult, MSG_INF.DELETE_SUCCESS, this.partsResultMessageComponent);
            this.processResultService.createdLog(this.processCustomResult.logData);
            modalRouterProgressComponentRef.destroy();

            this.receiptHeadersResult = new ReceiptHeadersResult();
            this.receiptHeadersResult.receiptHeaders = responseHeader;
    
            for (let index: number = 0; index < this.receiptHeadersResult.receiptHeaders.length; index++) {
              this.headerExcludeCategoryIdCtrls[index] = new FormControl("");
    
              if (this.receiptHeadersResult.receiptHeaders[index].existApportioned == 1) {
                this.headerExcludeCategoryIdCtrls[index].disable();
              }
              else {
                this.headerExcludeCategoryIdCtrls[index].enable();
              }
    
              this.MyFormGroup.removeControl("headerExcludeCategoryIdCtrl" + index);
              this.MyFormGroup.addControl("headerExcludeCategoryIdCtrl" + index, this.headerExcludeCategoryIdCtrls[index]);
            }
    
            this.clear();
  
          }
          else{
            modalRouterProgressComponentRef.destroy();
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.DELETE_ERROR, this.partsResultMessageComponent);
          }
        });
      }
      else{
        modalRouterProgressComponentRef.destroy();

        this.processCustomResult = this.processResultService.processAtFailure(
          this.processCustomResult, MSG_ERR.DELETE_ERROR, this.partsResultMessageComponent);
      }
    });
  }

  public apportionAll() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "一括振分"
    this.processCustomResult.logData.operationName = "一括振分";
    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus != MODAL_STATUS_TYPE.OK) {

        this.processCustomResult = this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);

        componentRef.destroy();
        return;
      }
      else {
        let receiptHeaderIdList = new Array<number>();
        for (let index = 0; index < this.receiptHeadersResult.receiptHeaders.length; index++) {
          if (this.receiptHeadersResult.receiptHeaders[index].isAllApportioned == 0) {
            receiptHeaderIdList.push(this.receiptHeadersResult.receiptHeaders[index].id);
          }
        }

        this.receiptService.GetApportionItems(receiptHeaderIdList)
          .subscribe(response => {
            if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '入金データの取得'),
                this.partsResultMessageComponent);
              componentRef.destroy();
              return;
            }
            else {
              let receiptApportions = new Array<ReceiptApportion>();
              receiptApportions = response;

              for (let index = 0; index < receiptApportions.length; index++) {
                receiptApportions[index].apportioned = 1;
                receiptApportions[index].updateBy = this.userInfoService.LoginUser.id;

                let headerIndex = this.receiptHeadersResult.receiptHeaders
                  .findIndex(header => {
                    return header.id == receiptApportions[index].receiptHeaderId
                  });

                if (!StringUtil.IsNullOrEmpty(this.headerExcludeCategoryIdCtrls[headerIndex].value)) {
                  receiptApportions[index].excludeFlag = 1;
                  receiptApportions[index].excludeAmount = receiptApportions[index].receiptAmount;
                  receiptApportions[index].excludeCategoryId = parseInt(this.headerExcludeCategoryIdCtrls[headerIndex].value);
                }

                if (this.collationSettingResult.collationSetting.requiredCustomer == 1 && receiptApportions[index].customerId == null) {
                  this.processCustomResult = this.processResultService.processAtWarning(
                    this.processCustomResult, MSG_WNG.NOT_APPORTIONED_CUSTOMER, this.partsResultMessageComponent);
                  componentRef.destroy();
                  return;
                }
              }

              let receiptApportionsResult: ReceiptApportionsResult;
              this.receiptService.Apportion(receiptApportions)
                .subscribe(responseSave => {
                  if (responseSave != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

                    receiptApportionsResult = responseSave;
                    this.receiptService.GetHeaderItems()
                      .subscribe(responseHeader => {

                        if (responseHeader != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

                          this.processCustomResult = this.processResultService.processAtSuccess(
                            this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);
                          this.processResultService.createdLog(this.processCustomResult.logData);
                          
                          this.receiptHeadersResult = new ReceiptHeadersResult();
                          this.receiptHeadersResult.receiptHeaders = responseHeader;
  
                          for (let index: number = 0; index < this.receiptHeadersResult.receiptHeaders.length; index++) {
                            this.headerExcludeCategoryIdCtrls[index] = new FormControl("");
  
                            if (this.receiptHeadersResult.receiptHeaders[index].existApportioned == 1) {
                              this.headerExcludeCategoryIdCtrls[index].disable();
                            }
                            else {
                              this.headerExcludeCategoryIdCtrls[index].enable();
                            }
  
                            this.MyFormGroup.removeControl("headerExcludeCategoryIdCtrl" + index);
                            this.MyFormGroup.addControl("headerExcludeCategoryIdCtrl" + index, this.headerExcludeCategoryIdCtrls[index]);
                          }
  
                          this.clear();
                        }
                        else {
                          this.processCustomResult = this.processResultService.processAtFailure(
                            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '一括振分'),
                            this.partsResultMessageComponent);
                        }
      
                      });
                  }
                  else {
                    this.processCustomResult = this.processResultService.processAtFailure(
                      this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '一括振分'),
                      this.partsResultMessageComponent);
                  }
                });
            }
          });
      }
      componentRef.destroy();
    });
  }

  public setDetailExcludeCheck(index: number) {
    if (this.detailExcludeCheckCtrls[index].value) {
      this.detailExcludeCategoryIdCtrls[index].enable();

      if (!StringUtil.IsNullOrEmpty(this.headerExcludeCategoryIdCtrls[this.selectedHeaderIndex].value) || this.headerExcludeCategoryIdCtrls[this.selectedHeaderIndex].value != "0" ) {
        this.detailExcludeCategoryIdCtrls[index].setValue(this.headerExcludeCategoryIdCtrls[this.selectedHeaderIndex].value);
      }

      this.detailExcludeAmountCtrls[index].enable();
      this.detailExcludeAmountCtrls[index].setValue(this.pipe.transform(this.receiptApportionsResult.receiptApportion[index].receiptAmount));

    }
    else {
      this.detailExcludeCategoryIdCtrls[index].disable();
      this.detailExcludeCategoryIdCtrls[index].setValue("");

      this.detailExcludeAmountCtrls[index].disable();
      this.detailExcludeAmountCtrls[index].setValue("0");

    }
  }

  //////////////////////////////////////////////////////////////////
  public setDetailExcludeAmount(eventType: string, index: number) {

  }

  public inputDetailExcludeAmount() {

  }

  public setCurrencyForDetailExcludeAmount(index: number) {
    this.detailExcludeAmountCtrls[index].setValue(this.pipe.transform(this.detailExcludeAmountCtrls[index].value));
  }

  public initCurrencyForDetailExcludeAmount(index: number) {
    this.detailExcludeAmountCtrls[index].setValue(this.pipe.reverceTransform(this.detailExcludeAmountCtrls[index].value));
  }


  public setDetailCustomerCode(eventType: string, index: number) {
    if (!StringUtil.IsNullOrEmpty(this.detailCustomerCodeCtrls[index].value)) {

      if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
        this.detailCustomerCodeCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.detailCustomerCodeCtrls[index].value, this.userInfoService.ApplicationControl.customerCodeLength));
      }      

      this.loadStart();
      this.customerService.GetItems(this.detailCustomerCodeCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.detailCustomerCodeCtrls[index].setValue(response[0].code);
            this.detailCustomerIdCtrls[index].setValue(response[0].id);
            this.detailCustomerNameCtrls[index].setValue(response[0].name);
          }
          else {
            let msg = MSG_WNG.MASTER_NOT_EXIST.split(MSG_ITEM_NUM.FIRST).join("得意先");
            msg = msg.replace(MSG_ITEM_NUM.SECOND, this.detailCustomerCodeCtrls[index].value);
            
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, msg, this.partsResultMessageComponent);
            this.detailCustomerCodeCtrls[index].setValue("");
            this.detailCustomerIdCtrls[index].setValue("");
            this.detailCustomerNameCtrls[index].setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'detailCustomerCodeCtrl'+index, eventType);
          }
        });

    }
    else {
      this.detailCustomerCodeCtrls[index].setValue("");
      this.detailCustomerIdCtrls[index].setValue("");
      this.detailCustomerNameCtrls[index].setValue("");
    }
  }

  public getApportionFlag(status: number):string {
    return status == 1 ? "済" : "末";
  }

  public setDetailSectionCode(eventType: string, index: number) {
    if (!StringUtil.IsNullOrEmpty(this.detailSectionCodeCtrls[index].value)) {
      this.loadStart();
      this.sectionService.GetItems(this.detailSectionCodeCtrls[index].value)
      .subscribe(response => {
        this.loadEnd();
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
          // this.detailCustomerIdCtrls[index].setValue(response[0].id);
          this.detailSectionCodeCtrls[index].setValue(response[0].code);
          this.detailSectionNameCtrls[index].setValue(response[0].name);
        }
        else {
          this.detailSectionCodeCtrls[index].setValue("");
          this.detailSectionNameCtrls[index].setValue("");
        }
      });

    } else {
      this.detailSectionNameCtrls[index].setValue("");
    }
  }

  public setSectionAssignCheck() {
    if (this.cbxSectionAssignCtrl.value) {
      this.receiptApportionsResult = this.sectionAssignReceiptApportionsResult;
    } else {
      this.receiptApportionsResult = this.allReceiptApportionsResult;
    }
  }

  public hideSectionAssign (index: number): boolean {
    return this.cbxSectionAssignCtrl.value && !StringUtil.IsNullOrEmpty(this.detailSectionCodeCtrls[index].value)
  }

}
